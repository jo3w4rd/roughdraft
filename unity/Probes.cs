using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public struct LaunchCommand : IComponentData
{
    public Entity MotherShip;
    public Entity ProbePrefab;
    public float3 StartingPoint;
    public float3 Direction;
}

public struct TrackedProbe : ISystemStateComponentData
{
    public Entity MotherShip;
}

public struct ProbeData : IComponentData
{
    public float  FuelRemaining;
    public float3 Direction;
    public float  Speed;
}


public class ProbeTrackingSystem : JobComponentSystem
{
    private EntityQuery m_newProbes;
    private EntityQuery m_activeProbes;
    private EntityQuery m_spentProbes;
    private EntityCommandBufferSystem m_ECBSource;

    [NativeDisableParallelForRestrictionAttribute]
    private NativeHashMap<Entity,int> m_probeList;
    protected override void OnCreate()
    {
        m_newProbes = GetEntityQuery(new EntityQueryDesc()
        {
            All  = new ComponentType[] { ComponentType.ReadOnly<LaunchCommand>()},
            None = new ComponentType[] { ComponentType.ReadWrite<TrackedProbe>() }
        });

        m_activeProbes = GetEntityQuery(new EntityQueryDesc()
        {
            All = new ComponentType[]
            {
                ComponentType.ReadWrite<ProbeData>(),
                ComponentType.ReadWrite<Translation>()
            }
        });

        m_spentProbes = GetEntityQuery(new EntityQueryDesc()
        {
            All = new ComponentType[] {ComponentType.ReadWrite<TrackedProbe>()},
            None = new ComponentType[]
            {
                ComponentType.ReadOnly<ProbeData>()
            }
        });

        m_ECBSource = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        m_probeList = new NativeHashMap<Entity, int>(50, Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        m_probeList.Dispose();
    }

    struct NewProbeJob : IJobForEachWithEntity<LaunchCommand>
    {
        public EntityCommandBuffer.Concurrent ConcurrentECB;
        [NativeDisableParallelForRestrictionAttribute]
        public NativeHashMap<Entity, int> ProbeList;

        public void Execute(Entity entity, int jobIndex, ref LaunchCommand launchCommand)
        {
            var motherShip = launchCommand.MotherShip;
            ProbeList.TryGetValue(motherShip, out var probes);
                if (probes < 5)
                {
                    ProbeList.Remove(motherShip);
                    ProbeList.TryAdd(motherShip, probes + 1);
                    var newProbe = ConcurrentECB.Instantiate(jobIndex, launchCommand.ProbePrefab);
                    ConcurrentECB.AddComponent(jobIndex, newProbe, new TrackedProbe(){MotherShip = motherShip});
                    ConcurrentECB.AddComponent(jobIndex, newProbe, new ProbeData()
                    {
                        FuelRemaining = 5,
                        Direction = launchCommand.Direction,
                        Speed = 1
                    });
                    ConcurrentECB.SetComponent<Translation>(jobIndex, newProbe,
                        new Translation() {Value = launchCommand.StartingPoint});
                }
                ConcurrentECB.DestroyEntity(jobIndex, entity);
        }
    }

    struct ActiveProbeJob : IJobForEachWithEntity<ProbeData, Translation>
    {
        public float elapsedSeconds;
        public EntityCommandBuffer.Concurrent ConcurrentECB;

        public void Execute(Entity entity, int jobIndex, ref ProbeData probeData, ref Translation translation)
        {
            //Debug.Log(translation.Value);
            probeData.FuelRemaining -= elapsedSeconds;
            if (probeData.FuelRemaining <= 0)
            {
                ConcurrentECB.DestroyEntity(jobIndex, entity);
                return;
            }
            translation.Value += probeData.Direction * probeData.Speed * elapsedSeconds;
        }
    }

    struct SpentProbesJob : IJobForEachWithEntity<TrackedProbe>
    {
        public EntityCommandBuffer.Concurrent ConcurrentECB;
        [NativeDisableParallelForRestrictionAttribute]
        public NativeHashMap<Entity, int> ProbeList;

        public void Execute(Entity entity, int jobIndex, ref TrackedProbe trackedProbe)
        {
            var motherShip = trackedProbe.MotherShip;
            if (ProbeList.TryGetValue(motherShip, out var probes))
            {
                if (probes > 0)
                {
                    ProbeList.Remove(motherShip);
                    ProbeList.TryAdd(motherShip, probes - 1);
                }
            }
            ConcurrentECB.RemoveComponent<TrackedProbe>(jobIndex, entity);
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var jobNewProbes = new NewProbeJob()
        {
            ConcurrentECB = m_ECBSource.CreateCommandBuffer().ToConcurrent(),
            ProbeList = m_probeList
        };
        var newProbeHandle = jobNewProbes.Schedule(m_newProbes, inputDependencies);
        m_ECBSource.AddJobHandleForProducer(newProbeHandle);

        var jobActiveProbes = new ActiveProbeJob(){
            elapsedSeconds = Time.deltaTime,
            ConcurrentECB = m_ECBSource.CreateCommandBuffer().ToConcurrent()
        };
        var activeProbeHandle = jobActiveProbes.Schedule(m_activeProbes, newProbeHandle);
        m_ECBSource.AddJobHandleForProducer(activeProbeHandle);

        var jobSpentProbes = new SpentProbesJob(){
            ConcurrentECB = m_ECBSource.CreateCommandBuffer().ToConcurrent(),
            ProbeList = m_probeList
        };
        var spentProbeHandle = jobSpentProbes.Schedule(m_spentProbes, activeProbeHandle);
        m_ECBSource.AddJobHandleForProducer(spentProbeHandle);

        return spentProbeHandle;
    }
}

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class Probes : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{

    public GameObject ProbePrefab;
    private Entity m_MotherShip;
    private Entity m_ConvertedProbePrefab;

    public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
    {
        gameObjects.Add(ProbePrefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        m_MotherShip = entity;
        m_ConvertedProbePrefab = conversionSystem.GetPrimaryEntity(ProbePrefab);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var em = World.Active.EntityManager;
            var motherShipPosition = transform.position;
            var launchPositionScreen = Camera.main.WorldToScreenPoint(motherShipPosition);
            var direction = Vector3.Normalize(Input.mousePosition - launchPositionScreen);
            var launchCommand = new LaunchCommand()
            {
                StartingPoint = motherShipPosition,
                Direction = direction,
                MotherShip = m_MotherShip,
                ProbePrefab = m_ConvertedProbePrefab
            };
            var launch = em.CreateEntity();
            em.AddComponentData(launch, launchCommand);
        }
    }


}

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateBefore(typeof(EndPresentationEntityCommandBufferSystem))]
public class ProbeCheckSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach<Translation>((ref Translation translation) =>
        {
            Debug.Log("Pos: " + translation.Value);
        });
    }
}