using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MLAgents;
public class TicTacToeController : MonoBehaviour
{

    public TextMeshPro XWinsText;
    public TextMeshPro DrawsText;
    public TextMeshPro OWinsText;
    public TextMeshPro ResultText;
    public TextMeshPro TurnIndicator;
    public TTTPiece[] squares;

    public bool XisAgent = false;
    public Agent AgentX;
    public bool OisAgent = false;
    public Agent AgentO;
    public int AgentDelay = 0;

    TicTacToe game = new TicTacToe();
    int XCounter = 0;
    int DrawCounter = 0;
    int OCounter = 0;
    bool Xturn = true;
    bool GameOver = false;
    string gameMessage = "";
    TicTacToe.GameState _gameState = TicTacToe.GameState.Contested;
    public TicTacToe.GameState State{
        get{ return _gameState; }
        private set{
            _gameState = value;
        }

    }
	void Start () {
        DrawBoard();
        StartCoroutine(AgentMove());
    }
	
    IEnumerator AgentMove(){
        
        while (true)
        {
            if (Xturn && XisAgent)
            {
                AgentX.RequestDecision();
                //random move
                //TakeTurn(TicTacToe.GameState.X, (int)Random.Range(0, 9));
            }
            else if (!Xturn && OisAgent)
            {
                AgentO.RequestDecision();

                //random move
                //TakeTurn(TicTacToe.GameState.O, (int)Random.Range(0, 9));
            }
            yield return new WaitForSeconds(AgentDelay);
        }
    }

    public float[] GetObservation(){
        return game.GetObservation();
    }

    public bool TakeMouseTurn(int square)
    {
        if(!(Xturn && XisAgent) && !(!Xturn && OisAgent))
          return   TakeTurn(Xturn ? TicTacToe.GameState.X : TicTacToe.GameState.O, square);

        return false;
    }

    public bool TakeTurn(int square) {
        return TakeTurn(Xturn ? TicTacToe.GameState.X : TicTacToe.GameState.O, square);
    }

    int illegals = 0;
    public bool TakeTurn(TicTacToe.GameState player, int square)
    {

        if (GameOver)
        {
            Reset();
            return true;
        }
        if (game.TryMove((int)player, square))
        {
            Xturn = !Xturn;
            State = game.Evaluate();
            switch (State)
            {
                case TicTacToe.GameState.X:
                    XCounter++;
                    gameMessage = "X Wins!";
                    GameOver = true;
                    break;
                case TicTacToe.GameState.O:
                    OCounter++;
                    gameMessage = "O Wins!";
                    GameOver = true;
                    break;
                case TicTacToe.GameState.Draw:
                    DrawCounter++;
                    gameMessage = "Draw.";
                    GameOver = true;
                    break;
                case TicTacToe.GameState.Contested:
                    break;
            }
            //    Debug.Log(game.Observation);
        } else {
            Debug.Log("Illegal move attempts: " + illegals++);
            return false;
        }
        DrawBoard();
        return true;
    }

    void DrawBoard(){
        for (int i = 0; i < 9; i++){
            squares[i].SetState(game.StateOfSquare(i));
        }
        XWinsText.text = "X: " + XCounter.ToString();
        OWinsText.text = "O: " + OCounter.ToString();
        DrawsText.text = "Draws: " + DrawCounter.ToString();
        ResultText.text = gameMessage;
        TurnIndicator.text = Xturn ? "X" : "O";
    }


    private void Reset()
    {
        //Debug.Log("Reset controller");
        game = new TicTacToe();
        Xturn = true;
        GameOver = false;
        gameMessage = "";
        DrawBoard();
    }

}
