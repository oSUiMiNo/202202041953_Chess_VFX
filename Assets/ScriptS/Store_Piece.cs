using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Store_Piece : MonoBehaviour
{
    Controller controller = null;
    private void Component()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
    }




    [SerializeField]private GameObject Null_Object;
    [SerializeField] public GameObject[,,] PieceS = new GameObject[2, 2, 8];
    [SerializeField] public Vector2Int[,,] Position_PieceS = new Vector2Int[2, 2, 8];

    public Dictionary<int, string> Exprore_color = new Dictionary<int, string>
    {
        {0, "White"},
        {1,"Black"}
    };
    public Dictionary<int, string> Exprore_category = new Dictionary<int, string>
    {
        {0, "Special"},
        {1,"Pawn"}
    };




    private void Start()
    {
        Component();
        Initialize();
    }




    private void Initialize()
    {
        Store_PieceS();
        for (int a = 0; a < 2; a++)
        {
            Init_Position(a);
            for (int b = 0; b < 2; b++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Store_Square_Piece(a, b, c);
                    Current(a, b, c);
                }
            }
        }
    }

    public void Update_Square_Piece()
    {
        for (int a = 0; a < 2; a++)
        {
            for (int b = 0; b < 2; b++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Store_Square_Piece(a, b, c);
                }
            }
        }
    }

    public void Update_Flag_Current()
    {
        for (int a = 0; a < 2; a++)
        {
            for (int b = 0; b < 2; b++)
            {
                for (int c = 0; c < 8; c++)
                {
                    PieceS[a, b, c].GetComponent<Piece>().Current = !PieceS[a, b, c].GetComponent<Piece>().Current;
                }
            }
        }
    }




    private void Store_PieceS()
    {
        PieceS = new GameObject[2, 2, 8]
        {
            {
                {
                    GameObject.Find("White_Rook0"),
                    GameObject.Find("White_Knight0"),
                    GameObject.Find("White_Bishop0"),
                    GameObject.Find("White_Queen0"),
                    GameObject.Find("White_King0"),
                    GameObject.Find("White_Bishop1"),
                    GameObject.Find("White_Knight1"),
                    GameObject.Find("White_Rook1")
                },
                {
                    GameObject.Find("White_Pawn0"),
                    GameObject.Find("White_Pawn1"),
                    GameObject.Find("White_Pawn2"),
                    GameObject.Find("White_Pawn3"),
                    GameObject.Find("White_Pawn4"),
                    GameObject.Find("White_Pawn5"),
                    GameObject.Find("White_Pawn6"),
                    GameObject.Find("White_Pawn7")
                },
             },
            {
                {
                    GameObject.Find("Black_Rook0"),
                    GameObject.Find("Black_Knight0"),
                    GameObject.Find("Black_Bishop0"),
                    GameObject.Find("Black_King0"),
                    GameObject.Find("Black_Queen0"),
                    GameObject.Find("Black_Bishop1"),
                    GameObject.Find("Black_Knight1"),
                    GameObject.Find("Black_Rook1")
                },
                {
                    GameObject.Find("Black_Pawn0"),
                    GameObject.Find("Black_Pawn1"),
                    GameObject.Find("Black_Pawn2"),
                    GameObject.Find("Black_Pawn3"),
                    GameObject.Find("Black_Pawn4"),
                    GameObject.Find("Black_Pawn5"),
                    GameObject.Find("Black_Pawn6"),
                    GameObject.Find("Black_Pawn7")
                }
            }
         };
    }

    public void Store_Square_Piece(int category ,int color, int specific)
    {
        Position_PieceS[category, color, specific] = (Square(PieceS[category, color, specific].transform.position));
        //return Position_PieceS[category, color, specific];
    }

    public void Init_Position(int Shift)
    {
        PieceS[0, 0, Shift].transform.position = Position(Shift, 0);
        PieceS[0, 1, Shift].transform.position = Position(Shift, 1);
        PieceS[1, 1, Shift].transform.position = Position(7 - Shift, 6);
        PieceS[1, 0, Shift].transform.position = Position(7 - Shift, 7);
    }

    public void Current(int category, int color, int specific)
    {
        if (PieceS[category, color, specific].GetComponent<Piece>().color == PicceColor.Black)
        {
            PieceS[category, color, specific].GetComponent<Piece>().Current = false;
        }
        if (PieceS[category, color, specific].GetComponent<Piece>().color == PicceColor.White)
        {
            PieceS[category, color, specific].GetComponent<Piece>().Current = true;
        }
    }

    
    public GameObject Piece_Square(Vector2Int Square)
    {
        for (int a = 0; a < 2; a++)
        {
            for (int b = 0; b < 2; b++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (Position_PieceS[a, b, c] == Square)
                    {
                        return PieceS[a, b, c];
                    }
                }
            }
        }
        //return controller.Piece_Selected;
        return null;
    }






















    private Vector3 Position(int X, int Y)
    {
        Vector3 Position = Calculate_Position.Position_Square(Calculate_Position.Square(X, Y));
        return Position;
    }

    private Vector2Int Square(Vector3 Pixel)
    {
        Vector2Int Square = Calculate_Position.Square_Pixel(Pixel);
        return Square;
    }

    private void Attackdirection()
    {
        //駒の動きを作るときに考える。
    } 
}
