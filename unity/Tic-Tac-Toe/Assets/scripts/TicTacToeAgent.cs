using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class TicTacToeAgent : Agent {

    public TicTacToeController gameController;
    public bool isXPlayer = true;

    public override void CollectObservations()
    {
        AddVectorObs(OneHotinize(gameController.GetObservation()));
        //AddVectorObs(gameController.GetObservation());
    }

    public float[] OneHotinize(float[] original){
        float[] onehot = new float[21];
        for (int i = 0; i < 9; i++){
            switch ((int)original[i]){
                case (int) TicTacToe.GameState.Contested:
                    onehot[i] = 1.0f;
                    onehot[i+1] = 0.0f;
                    onehot[i+2] = 0.0f;
                    break;
                case (int)TicTacToe.GameState.X:
                    onehot[i] = 0.0f;
                    onehot[i + 1] = 1.0f;
                    onehot[i + 2] = 0.0f;
                    break;
                case (int)TicTacToe.GameState.O:
                    onehot[i] = 0.0f;
                    onehot[i + 1] = 0.0f;
                    onehot[i + 2] = 1.0f;
                    break;
            }
        }
        return onehot;
    }
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        switch (gameController.State)
        {
            case TicTacToe.GameState.X:
                if (isXPlayer)
                    AddReward(1.0f);
                Done();
                break;
            case TicTacToe.GameState.O:
                if (!isXPlayer)
                    AddReward(1.0f);
                Done();
                break;
            case TicTacToe.GameState.Draw:
                AddReward(0.5f);
                Done();
                break;
            case TicTacToe.GameState.Contested:
                break;
        }
        if(!gameController.TakeTurn((int)vectorAction[0])){
            AddReward(-0.33f);
        }
    }
}
