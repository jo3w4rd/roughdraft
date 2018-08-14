using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe
{
    //Used for multiple purposes.
    //State of a Game or line within a game use all values
    //State of a square doesn't use Draw
    //Contested means a square is unmarked, a line still has open squares
    // and a game is not finished.
    public enum GameState { Contested, Draw, X, O };

    int[] board = { -1, -1, -1, -1, -1, -1, -1, -1, -1 };


    public TicTacToe(){
        for (int i = 0; i < 9; i++){
            board[i] = (int)GameState.Contested;
        }
    }

    public TicTacToe(int[] squares)
    {
        Overwrite(squares);
    }

    public void Overwrite(int[] squares)
    {
        for (int i = 0; i < 9; i++)
        {
            board[i] = squares[i];
        }
    }

    public GameState StateOfSquare(int row, int col){
        return (GameState)board[row * 3 + col];
    }

    public GameState StateOfSquare(int index)
    {
        return (GameState)board[index];
    }

    public bool TryMove(int player, int square){
        //Debug.Log("Player: " + player + ", " + square);
        if(board[square] == (int)GameState.Contested) {
            //Debug.Log("Set square");
            board[square] = player;
            return true;
        }
        return false;
    }

    public float[] GetObservation(){
        float[] squares = new float[9];
        for (int i = 0; i < 9; i++)
        {
            squares[i] = (float)board[i];
        }
        return squares;
    }

    public GameState Evaluate(){
        // Get state of lines (rows, columns, diagonals
        GameState r0 = checkRow(0);
        GameState r1 = checkRow(1);
        GameState r2 = checkRow(2);
        GameState c0 = checkCol(0);
        GameState c1 = checkCol(1);
        GameState c2 = checkCol(2);
        GameState d0 = checkDiagLR();
        GameState d1 = checkDiagRL();

        // Check for any winning line
        if (r0 == GameState.X ||
           r1 == GameState.X ||
           r2 == GameState.X ||
           c0 == GameState.X ||
           c1 == GameState.X ||
           c2 == GameState.X ||
           d0 == GameState.X ||
           d1 == GameState.X)
            return GameState.X;

        if (r0 == GameState.O ||
            r1 == GameState.O ||
            r2 == GameState.O ||
            c0 == GameState.O ||
            c1 == GameState.O ||
            c2 == GameState.O ||
            d0 == GameState.O ||
            d1 == GameState.O)
            return GameState.O;

        //Check for draw
        if (r0 == GameState.Draw &&
            r1 == GameState.Draw &&
            r2 == GameState.Draw &&
            c0 == GameState.Draw &&
            c1 == GameState.Draw &&
            c2 == GameState.Draw &&
            d0 == GameState.Draw &&
            d1 == GameState.Draw)
            return GameState.Draw;

        return GameState.Contested;
    }

    public override string ToString()
    {
        string obs = "";
        for (int i = 0; i < 9; i++)
        {
            obs += board[i] + ", ";
        }

        return base.ToString();
    }

    GameState checkRow(int row){
        if( (board[row * 3 + 0] == board[row * 3 + 1]) &&
           (board[row * 3 + 1] == board[row * 3 + 2])){
            return (GameState)board[row * 3 + 0];
        }
        //Is row drawn? -- really need to check if there are both X and O
        if (!(board[row * 3 + 0] == (int)GameState.Contested ||
              board[row * 3 + 1] == (int)GameState.Contested || 
              board[row * 3 + 2] == (int)GameState.Contested))
        {
            return GameState.Draw;
        }

        return GameState.Contested;
    }

    GameState checkCol(int col)
    {
        if( (board[col] == board[col + 3]) &&
           (board[col + 3] == board[col + 6])){
            return (GameState)board[col];
        }
        //Is col drawn?
        if (!(board[col]     == (int)GameState.Contested ||
              board[col + 3] == (int)GameState.Contested ||
              board[col + 6] == (int)GameState.Contested))
        {
            return GameState.Draw;
        }
        return GameState.Contested;
    }

    GameState checkDiagLR()
    {
        if ((board[0] == board[4]) &&
           (board[4] == board[8]))
        {
            return (GameState)board[0];
        }
        //Is diag drawn?
        if (!(board[0] == (int)GameState.Contested ||
              board[4] == (int)GameState.Contested ||
              board[8] == (int)GameState.Contested))
        {
            return GameState.Draw;
        }
        return GameState.Contested;
    }

    GameState checkDiagRL()
    {
        if ((board[2] == board[4]) &&
           (board[4] == board[6]))
        {
            return (GameState)board[2];
        }
        //Is diag drawn?
        if (!(board[2] == (int)GameState.Contested ||
              board[4] == (int)GameState.Contested ||
              board[6] == (int)GameState.Contested))
        {
            return GameState.Draw;
        }
        return GameState.Contested;

    }

}
