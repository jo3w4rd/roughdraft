using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class TTTEnviroSpawn : MonoBehaviour {

    public GameObject prefab;
    public int Count = 1;
    public Vector3 Dimensions = Vector3.one;
    public Brain XBrain;
    public Brain OBrain;

    private bool done = false;
    public int delay = 5000;

    void Update(){
        if(!done){
            if(delay-- < 0){
                Populate();
                done = true;
            }
        }
    }

    public void Populate(){
        int sideCount = (int)Mathf.Pow(Count, 1.0f/3) + 1;

        Vector3 origin = new Vector3(Dimensions.x - (sideCount * Dimensions.x)/2,
                                     Dimensions.y - (sideCount * Dimensions.y)/2,
                                     Dimensions.z - (sideCount * Dimensions.z)/2);
        int created = 0;
        for(int i = 0; i < sideCount; i++){
            for(int j = 0; j < sideCount; j++){
                for(int k = 0; k < sideCount; k++){
                    if(created++ >= Count) break;

                    GameObject instance = Instantiate<GameObject>(prefab, origin + new Vector3(k * Dimensions.x, 
                                                                                               j * Dimensions.y, 
                                                                                               i * Dimensions.z), 
                                                                  Quaternion.identity);
                    Agent[] agents = instance.GetComponentsInChildren<Agent>();
                    for (int a = 0; a < agents.Length; a++)
                    {
                        Agent agent = agents[a];
                        if (agent.name == "XAgent")
                            agent.GiveBrain(XBrain);
                        else
                            agent.GiveBrain(OBrain);
                    }

                }
                if(created >= Count) break;
            }
            if(created >= Count) break;
        }
    }
}
