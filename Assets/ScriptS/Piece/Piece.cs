using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType { King, Queen, Bishop, Knight, Rook, Pawn };
public enum PicceColor { White, Black };

public abstract class Piece : MonoBehaviour
{
    public PieceType type;
    public PicceColor color;
    public bool Current;


   
    public void Start()
    {
        Component();
    }



    Store_Piece store_piece;
    Controller controller;
    Collider collider;
    public void Component()
    {
        store_piece = GameObject.Find("Store_Piece").GetComponent<Store_Piece>();
        controller = GameObject.Find("Controller").GetComponent<Controller>();
        collider = this.gameObject.GetComponent<Collider>();
    }


    protected Vector2Int[] Direction_Rook = new Vector2Int[4]
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0)
    };
    protected Vector2Int[] Direction_Bishop = new Vector2Int[4]
    {
        new Vector2Int(1, 1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(1, -1)
    };



    public abstract List<Vector2Int> Square_Move(Vector2Int Square);
    public abstract List<Vector2Int> Square_Attack(Vector2Int Square);




    [SerializeField] public bool Exist_Player;
    [SerializeField] public bool Exist_Oponent;
    [SerializeField] public bool Out_of_Borad;
    public bool CantMove(Vector2Int CanMove_Next)
    {
        Vector3 Position = Calculate_Position.Position_From_Square(CanMove_Next);

        Exist_Oponent = false;
        Exist_Player = false;
        Out_of_Borad = false;

        if (Position.x < -7 || Position.x > 7 || Position.z < -7 || Position.z > 7)
        {
            Out_of_Borad = true;
        }
        if(store_piece.Piece_Square(CanMove_Next) != null)
        {
            if (store_piece.Piece_Square(CanMove_Next).GetComponent<Piece>().Current == Current)
            {
                Exist_Player = true;
            }
            if (store_piece.Piece_Square(CanMove_Next).GetComponent<Piece>().Current == !Current)
            {
                Exist_Oponent = true;
            }
        }
        else
        {
            Exist_Oponent = false;
            Exist_Player = false;
        }
        

        if (Out_of_Borad == false && Exist_Oponent == false && Exist_Player == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    private void Update()
    {
        if (Current == false)
        {
            collider.enabled = false;
        }
        else
        {
            collider.enabled = true;
        }
    }

    public bool first = true;
}

