using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;


public struct Impulse : IComponentData
{
    public float Value;
}

public struct Decay : IComponentData
{
    public float Value;
}

public struct State : ISystemStateComponentData {}

public class SystemWithState : JobComponentSystem
{
    private EntityQuery m_newEntities;
    private EntityQuery m_activeEntities;
    private EntityQuery m_destroyedEntities;
    private EntityCommandBufferSystem m_ECBSource;
    protected override void OnCreate()
    {
        m_newEntities = GetEntityQuery(new EntityQueryDesc()
        {
            All = new ComponentType[]
            {
                ComponentType.ReadWrite<Impulse>(),
                ComponentType.ReadWrite<Decay>()
            },
            None = new ComponentType[] {ComponentType.ReadWrite<State>()}
        });

        m_activeEntities = GetEntityQuery(new EntityQueryDesc()
        {
            All = new ComponentType[]
            {
                ComponentType.ReadWrite<Impulse>(),
                ComponentType.ReadOnly<State>(),
                ComponentType.ReadWrite<Decay>()
            }
        });

        m_destroyedEntities = GetEntityQuery(new EntityQueryDesc()
        {
            All = new ComponentType[] {ComponentType.ReadWrite<State>()},
            None = new ComponentType[]
            {
                ComponentType.ReadOnly<Impulse>(),
                ComponentType.ReadWrite<Decay>()
            }
        });

        m_ECBSource = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    struct NewEntityJob : IJobForEachWithEntity<Impulse, Decay>
    {
        public EntityCommandBuffer.Concurrent ConcurrentECB;
        public void Execute(Entity entity, int jobIndex, ref Impulse impulse, ref Decay decay)
        {
                ConcurrentECB.AddComponent(jobIndex, entity, new State());
                ConcurrentECB.SetComponent(jobIndex, entity, new Decay(){ Value = impulse.Value});
                ConcurrentECB.SetComponent(jobIndex, entity, new Impulse(){ Value = 0.0f});
        }
    }

    struct ProcessEntityJob : IJobForEachWithEntity<Impulse, Decay, State>
    {
        public EntityCommandBuffer.Concurrent ConcurrentECB;

        public void Execute(Entity entity,
                            int jobIndex,
                            ref Impulse impulse,
                            ref Decay decay,
                            [ReadOnly] ref State state)
        {
            decay.Value = .05f * impulse.Value + (1 - .05f) * decay.Value;
            impulse.Value = 0.0f;

            if(decay.Value < 0.01f)
            {
                ConcurrentECB.DestroyEntity(jobIndex, entity);
            }
        }
    }

    struct CleanupEntityJob : IJobForEachWithEntity<State>
    {
        public EntityCommandBuffer.Concurrent ConcurrentECB;
        public void Execute(Entity entity, int jobIndex, [ReadOnly] ref State state)
        {
            ConcurrentECB.RemoveComponent<State>(jobIndex, entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var newEntityJob = new NewEntityJob(){ ConcurrentECB = m_ECBSource.CreateCommandBuffer().ToConcurrent() };
        var newJobHandle = newEntityJob.Schedule(m_newEntities, inputDependencies);
        m_ECBSource.AddJobHandleForProducer(newJobHandle);

        var processEntityJob = new ProcessEntityJob() { ConcurrentECB = m_ECBSource.CreateCommandBuffer().ToConcurrent() };
        var processJobHandle = processEntityJob.Schedule(m_activeEntities, newJobHandle);
        m_ECBSource.AddJobHandleForProducer(processJobHandle);

        var cleanupEntityJob = new CleanupEntityJob() { ConcurrentECB = m_ECBSource.CreateCommandBuffer().ToConcurrent() };
        var cleanupJobHandle = cleanupEntityJob.Schedule(m_destroyedEntities, processJobHandle);
        m_ECBSource.AddJobHandleForProducer(cleanupJobHandle);

        return cleanupJobHandle;
    }
}

// For testing the System State Example
public class SystemStateExample : MonoBehaviour
{
    private EntityManager entityManager;
    private EntityArchetype newEntityArchetype;
    private EntityQuery query;
    void Start()
    {
        entityManager = World.Active.EntityManager;
        newEntityArchetype = entityManager.CreateArchetype(typeof(Impulse), typeof(Decay));
        query = entityManager.CreateEntityQuery(typeof(Decay));
    }

    // Click & drag mouse to add entities
    private void Update()
    {
        if (!Input.GetMouseButton(0))
            return;

        var mousePos = Input.mousePosition;
        if (query.CalculateLength() <= mousePos.x / 10)
        {
            var entity = entityManager.CreateEntity(newEntityArchetype);
            entityManager.SetComponentData<Impulse>(entity,
                new Impulse() {Value = mousePos.y / Screen.height});
        }
    }

    // Draw bar heights proportional to entity Output component values.
    private void OnGUI()
    {
        if (!Event.current.type.Equals(EventType.Repaint))
            return;

        var outputs = query.ToComponentDataArray<Decay>(Allocator.TempJob);
        for (var i = 0; i < outputs.Length; i++)
        {
            Graphics.DrawTexture(new Rect (
                    i * 10,Screen.height,8,
                    -Screen.height * outputs[i].Value),
                Texture2D.whiteTexture);
        }
        outputs.Dispose();
    }
}