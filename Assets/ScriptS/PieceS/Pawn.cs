using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> Square_Move(Vector2Int Square)
    {
        List<Vector2Int> CanMove = new List<Vector2Int>();
        
        int Direction = 1;
        if (color == PicceColor.White)
        {
            Direction = 1;
        }
        if (color == PicceColor.Black)
        {
            Direction = -1;
        }

        Vector2Int Move1 = Square + new Vector2Int(0, 1) * Direction;
        Vector2Int Move2 = Square + new Vector2Int(0, 2) * Direction;
        
        if (CantMove(Move1) == false)
        {
            CanMove.Add(Move1);
        }
        
        if (CantMove(Move2) == false)
        {
            CanMove.Add(Move2);
        }
        if (first == false)
        {
            CanMove.Remove(Move2);
        }
        return CanMove;
    }


    public override List<Vector2Int> Square_Attack(Vector2Int Square)
    {
        List<Vector2Int> Attack = new List<Vector2Int>();

        int Direction = 1;
        if (color == PicceColor.White)
        {
            Direction = 1;
        }
        if (color == PicceColor.Black)
        {
            Direction = -1;
        }

        Vector2Int MoveR = Square + new Vector2Int(1, 1) * Direction;
        Vector2Int MoveL = Square + new Vector2Int(-1, 1) * Direction;

        CantMove(MoveR);
        if (!Out_of_Borad && !Exist_Player && Exist_Oponent)
        {
            Attack.Add(MoveR);
        }

        CantMove(MoveL);
        if (!Out_of_Borad && !Exist_Player && Exist_Oponent)
        {
            Attack.Add(MoveL);
        }
        return Attack;
    }
}