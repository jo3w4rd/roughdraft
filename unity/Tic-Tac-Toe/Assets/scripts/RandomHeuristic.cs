using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RandomHeuristic : MonoBehaviour, Decision {
    public float[] Decide(
            List<float>
                vectorObs,
            List<Texture2D> visualObs,
            float reward,
            bool done,
        List<float> memory){

        float[] action = new float[1];
        action[0] = Mathf.Floor(Random.Range(0, 9));
        return action;
    }

    public List<float> MakeMemory(
            List<float> vectorObs,
            List<Texture2D> visualObs,
            float reward,
            bool done,
        List<float> memory){
        return new List<float>();
    }
}
