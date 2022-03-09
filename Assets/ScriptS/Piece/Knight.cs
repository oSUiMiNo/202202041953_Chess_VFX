using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override List<Vector2Int> Square_Move(Vector2Int Square)
    {
        List<Vector2Int> CanMove = new List<Vector2Int>();

        Vector2Int[] CanMove_Next = new Vector2Int[]    
        {
            Square + new Vector2Int(-1, 2),
            Square + new Vector2Int(1, 2),
            Square + new Vector2Int(2, 1),
            Square + new Vector2Int(-2, 1),
            Square + new Vector2Int(2, -1),
            Square + new Vector2Int(-2, -1),
            Square + new Vector2Int(1, -2),
            Square + new Vector2Int(-1, -2)
        };

        for(int a = 0; a < 8; a++)
        {
            if(CantMove(CanMove_Next[a]) == false)
            {
                CanMove.Add(CanMove_Next[a]);
            }
        }
        return CanMove;
    }


    public override List<Vector2Int> Square_Attack(Vector2Int Square)
    {
        List<Vector2Int> Attack = new List<Vector2Int>();

        Vector2Int[] CanMove_Next = new Vector2Int[]
        {
            Square + new Vector2Int(-1, 2),
            Square + new Vector2Int(1, 2),
            Square + new Vector2Int(2, 1),
            Square + new Vector2Int(-2, 1),
            Square + new Vector2Int(2, -1),
            Square + new Vector2Int(-2, -1),
            Square + new Vector2Int(1, -2),
            Square + new Vector2Int(-1, -2)
        };

        for (int a = 1; a < 8; a++)
        {
            CantMove(CanMove_Next[a]);
            if(!Exist_Player && !Out_of_Borad && Exist_Oponent)
            {
                Attack.Add(CanMove_Next[a]);
            }
        }
        return Attack;
    }
}