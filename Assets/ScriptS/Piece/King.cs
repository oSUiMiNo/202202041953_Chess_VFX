using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> Square_Move(Vector2Int Square)
    {
        List<Vector2Int> CanMove = new List<Vector2Int>();
        Vector2Int[][] Direction_King = new Vector2Int[][]
        {
            Direction_Rook,
            Direction_Bishop
        };

        foreach (Vector2Int[] item in Direction_King)
        {
            foreach (Vector2Int Direction in item)
            {
                Vector2Int CanMove_Next = Square + Direction;

                if (!CantMove(CanMove_Next)) 
                {
                    CanMove.Add(CanMove_Next);
                }
            }
        }
        return CanMove;
    }


    public override List<Vector2Int> Square_Attack(Vector2Int Square)
    {
        List<Vector2Int> Attack = new List<Vector2Int>();
        Vector2Int[][] Direction_King = new Vector2Int[][]
        {
            Direction_Rook,
            Direction_Bishop
        };

        foreach (Vector2Int[] item in Direction_King)
        {
            foreach (Vector2Int Direction in item)
            {
                Vector2Int CanMove_Next = Square + Direction;

                CantMove(CanMove_Next);
                if (!Exist_Player && !Out_of_Borad && Exist_Oponent)
                {
                    Attack.Add(CanMove_Next);
                }
            }
        }
        return Attack;
    }
}