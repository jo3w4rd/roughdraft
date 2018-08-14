using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTPiece : MonoBehaviour {

    public GameObject Blank;
    public GameObject X;
    public GameObject O;
    public int BoardIndex;

    private TicTacToeController controller;

    public void SetState(TicTacToe.GameState state){
        switch(state){
            case TicTacToe.GameState.Contested:
                Blank.SetActive(true);
                X.SetActive(false);
                O.SetActive(false);
                break;
            case TicTacToe.GameState.X:
                Blank.SetActive(false);
                X.SetActive(true);
                O.SetActive(false);
                break;
            case TicTacToe.GameState.O:
                Blank.SetActive(false);
                X.SetActive(false);
                O.SetActive(true);
                break;
            default:
                Debug.LogWarning("Setting square to unsupported state.");
                break;
        }
    }

    private void Start()
    {
        controller = GetComponentInParent<TicTacToeController>();
    }

    void OnMouseUp () {
        Debug.Log("Clicked square " + BoardIndex);
        controller.TakeMouseTurn(BoardIndex);
	}
}
