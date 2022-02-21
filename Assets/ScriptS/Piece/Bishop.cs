using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override List<Vector2Int> Square_Move(Vector2Int Square)
    {
        List<Vector2Int> CanMove = new List<Vector2Int>();
        
        foreach(Vector2Int Direction in Direction_Bishop)
        {
            for(int a = 1; a < 8; a++)
            {
                Vector2Int CanMove_Next = Square + a * Direction;

                if (CantMove(CanMove_Next)) { break; }
                CanMove.Add(CanMove_Next);
            }
        }
        return CanMove;
    }


    public override List<Vector2Int> Square_Attack(Vector2Int Square)
    {
        List<Vector2Int> Attack = new List<Vector2Int>();

        foreach (Vector2Int Direction in Direction_Bishop)
        {
            for (int a = 1; a < 8; a++)
            {
                Vector2Int CanMove_Next = Square + a * Direction;

                CantMove(CanMove_Next);
                if (Exist_Player || Out_of_Borad) { break; }
                if (Exist_Oponent) { Attack.Add(CanMove_Next); break; }
            }
        }
        return Attack;
    }
}
