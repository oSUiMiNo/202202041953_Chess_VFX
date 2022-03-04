using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public void Start()
    {
        Component();
        Initialize_Game();
    }

    



    [SerializeField] GameObject Null_Object;
    public Pool_Highlight_Yellow pool_Highlight_Yellow = null;
    public Pool_Highlight_Blue pool_Highlight_Blue = null;
    public Pool_Highlight_Red pool_Highlight_Red = null;
    public Pool_Highlight_Selected pool_Highlight_Selected = null;
    public GameObject Pool;
    public Store_Piece store_Piece = null;
    public Text text;
    public ChessAgent ChessAgent;
    private void Component()
    {
        Pool = GameObject.Find("Pool");
        pool_Highlight_Yellow = Pool.GetComponent<Pool_Highlight_Yellow>();
        pool_Highlight_Blue = Pool.GetComponent<Pool_Highlight_Blue>();
        pool_Highlight_Red = Pool.GetComponent<Pool_Highlight_Red>();
        pool_Highlight_Selected = Pool.GetComponent<Pool_Highlight_Selected>();
        store_Piece = GameObject.Find("Store_Piece").GetComponent<Store_Piece>();
        text = GameObject.Find("Inform").GetComponent<Text>();
        ChessAgent = GameObject.Find("ChessAgent").GetComponent<ChessAgent>();
    }


    public bool AIMode;

    public float Times;
    public float StateTime;
    public void Update()
    {
        if(ChessAgent.enabled == false)
        {
            Human();
        }
        else if (ChessAgent.enabled == true)
        {
            Agent();
        }

        if (animator != null && animator.enabled == true)
        {
            if (PieceState.Bool1 == false)
            {
                Times = PieceState.Times;
                StateTime = PieceState.StateTime;
                //Debug.Log("Co  " + "StateTime : " + StateTime + "Times : " + Times);

                if (StateTime >= Times && Aniamtion_Active == true)
                {
                    Debug.Log("a");
                    StartCoroutine(FixPosition());
                    Aniamtion_Active = false;
                }
            }
        }
    }


    //レイキャスト
    public Ray ray;
    RaycastHit Hit;
    public bool Point, Click, Click_AI;
    //public string Tag;
    private void Human()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Point = Physics.Raycast(ray, out Hit);
        Click = Input.GetMouseButtonDown(0);

        if (Point)
        {
            GameObject HitObject = Hit.collider.gameObject;
            string Tag = HitObject.tag;
            Slect_Selector(HitObject, Tag);
        }
    }

    private void Agent()
    {
        ray = ChessAgent.ray;
        Hit = ChessAgent.Hit;
        Point = ChessAgent.Point;
        Click = true;

        if (Point)
        {
            GameObject HitObject = Hit.collider.gameObject;
            string Tag = HitObject.tag;
            Slect_Selector(HitObject, Tag);
        }

        Click = false;
    }







    public GameObject Piece_Selected, Piece_Current = null;
    public void Slect_Selector(GameObject HitObject, string Tag)
    {
        Selector_Tile();

        if (Click)
        {
            if (Tag == "PieceS_White" || Tag == "PieceS_Black" )
            {
                DeSelector_Piece(Piece_Current);
                Piece_Selected = HitObject;
                Selector_Piece(Piece_Selected);
                return;
            }
            else if (Piece_Current != Hit.collider)
            {
                if(Hit.collider.gameObject.tag == "Highlight_Blue")
                {
                    DeSelector_Piece(Piece_Current);
                    Move_Piece(Piece_Current);
                    return;
                }
                else if(Hit.collider.gameObject.tag == "Highlight_Red")
                {
                    DeSelector_Piece(Piece_Current);
                    Attack(Piece_Current);
                    return;
                }
                
                else
                {
                    DeSelector_Piece(Piece_Current);
                    return;
                }
            }
            else
            {
                Piece_Selected = null;
            }
        }
    }


    public void Selector_Tile()
    {
        pool_Highlight_Yellow.Object_Hide();
        pool_Highlight_Yellow.Object_Discharge(Position_Select());
    }

    
    public void Selector_Piece(GameObject Piece_Selected)
    {
        Piece_Selected.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
        Piece_Selected.transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = true;
        
        Piece piece = Piece_Selected.GetComponent<Piece>();
        for (int a = 0; a < piece.Square_Move(Square_Select()).Count; a++)
        {
            pool_Highlight_Blue.Object_Discharge((Calculate_Position.Position_From_Square(piece.Square_Move(Square_Select())[a])));
            pool_Highlight_Selected.Object_Discharge(Position_Select());
        }
        for (int a = 0; a < piece.Square_Attack(Square_Select()).Count; a++)
        {
            pool_Highlight_Red.Object_Discharge((Calculate_Position.Position_From_Square(piece.Square_Attack(Square_Select())[a])));
        }
        
        Piece_Current = Piece_Selected;
    }
    
    public void DeSelector_Piece(GameObject Piece_Current)
    {
        if(Piece_Current != null)
        {
            Piece_Current.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = true;
            Piece_Current.transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = false;
            pool_Highlight_Blue.Object_Hide();
            pool_Highlight_Selected.Object_Hide();
            pool_Highlight_Red.Object_Hide();
        }
    }

    
    public void Move_Piece(GameObject Piece_Current)
    {
        Select_Animation(Piece_Current);

        //Piece_Current.transform.position = Position();
        
        //store_Piece.Update_Square_Piece();
        //store_Piece.Update_Flag_Current();


        Piece piece = Piece_Current.GetComponent<Piece>();
        if (piece.type == PieceType.Pawn)
        {
            piece.first = false;
        }
    }

    private int Delay; 
    public void Attack(GameObject Piece_Current)
    {
        Select_Animation(Piece_Current);

        store_Piece.Piece_Square(Square_Select()).SetActive(false);

        Piece piece = Piece_Current.GetComponent<Piece>();
        if (piece.type == PieceType.Pawn)
        {
            piece.first = false;
        }
    }




    //動かすアニメーションの選択
    public Camera_State cameraState;
    public bool Aniamtion_Active;
    Animator animator;
    Piece_State PieceState;
    Vector2Int D;
    //Vector3 D;
    float DX;
    float DY;
    public void Select_Animation(GameObject Piece_Current)
    {

        //カメラのアニメーション
        cameraState = Camera.main.GetComponent<Animator>().GetBehaviour<Camera_State>();

        //駒のアニメーション
        Aniamtion_Active = true;  //Currentフラグのアップデート等を1回しか行いたくないが、ステートマシーンビヘイビアのTimesを使うと、アップデートが二回実行されてしまうのでこのフラグで1回しか実行できないようにする。

        animator = Piece_Current.GetComponent<Animator>();
        PieceState = animator.GetBehaviour<Piece_State>();
        animator.enabled = true;


        //駒の、方向の判定 & アニメーションフラグの指定
        Vector2Int Delta = Square_Select() - Square_Piece(Piece_Current);
        int DeltaX = Delta.x;
        int DeltaY = Delta.y;

        //Debug.Log("Delta : " + Delta);


        D = Delta;
        DX = D.x;
        DY = D.y;
        //ナイト
        if (DX == 1 && DY == 2)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(1, 2)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (DX == 2 && DY == 1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(2, 1)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (DX == 2 && DY == -1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(2, -1)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (DX == 1 && DY == -2)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(1, -2)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (DX == -1 && DY == -2)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(-1, -2)";
            PieceState.Bool1 = true;

            return;
        }
        else if (DX == -2 && DY == -1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(-2, -1)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (DX == -2 && DY == 1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(-2, 1)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (DX == -1 && DY == 2)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(-1, 2)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        //ナイト以外
        else
        {
            for (int a = 1; a < 16; a++)
            {
                D = Delta / a;
                DX = D.x;
                DY = D.y;
                //Debug.Log("D : " + D);
                //まっすぐ
                if (DX == 1 && DY == 0)
                {
                    PieceState.Times = DeltaX;
                    PieceState.Flag1 = "(1, 0)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (DX == -1 && DY == 0)
                {
                    PieceState.Times = DeltaX * -1;
                    PieceState.Flag1 = "(-1, 0)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (DX == 0 && DY == 1)
                {
                    PieceState.Times = DeltaY;
                    PieceState.Flag1 = "(0, 1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (DX == 0 && DY == -1)
                {
                    PieceState.Times = DeltaY * -1;
                    PieceState.Flag1 = "(0, -1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                //斜め
                else if (DX == 1 && DY == 1)
                {
                    PieceState.Times = DeltaX;
                    PieceState.Flag1 = "(1, 1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (DX == 1 && DY == -1)
                {
                    PieceState.Times = DeltaX;
                    PieceState.Flag1 = "(1, -1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (DX == -1 && DY == 1)
                {
                    PieceState.Times = DeltaX * -1;
                    PieceState.Flag1 = "(-1, 1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (DX == -1 && DY == -1)
                {
                    PieceState.Times = DeltaX * -1;
                    PieceState.Flag1 = "(-1, -1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                //else
                //{
                //    Debug.Log("わー" + a);
                //}
            }
        }
    }



    
    public IEnumerator FixPosition()
    {
        yield return new WaitForSeconds(0.01f);
        animator.enabled = false;
        
        int X = Mathf.FloorToInt(Piece_Current.transform.position.x);
        int Y = Mathf.FloorToInt(Piece_Current.transform.position.z);
        if (DX == 1 || DX == 2)
        {
            X = Mathf.FloorToInt(Piece_Current.transform.position.x);
        }
        if(DY == 1 || DY == 2)
        {
            Y = Mathf.FloorToInt(Piece_Current.transform.position.z);
        }
        if (DX == -1 || DX == -2)
        {
            X = Mathf.FloorToInt(Piece_Current.transform.position.x + 0.8f);
        }
        if (DY == -1 || DY == -2)
        {
            Y = Mathf.FloorToInt(Piece_Current.transform.position.z + 0.8f);
        }

        Piece_Current.transform.position = new Vector3(X, 0.5f, Y);
        Piece_Current.transform.position = new Vector3(X, 0.5f, Y);
        Piece_Current.transform.position = new Vector3(X, 0.5f, Y);

        store_Piece.Update_Square_Piece();
        store_Piece.Update_Flag_Current();
        
        //ターン交代
        Change_Turn();
    }




    //ターン交代
    public bool Turn;
    public void Initialize_Game()
    {
        Turn = true;
    }
    public void Change_Turn()
    {
        //Debug.Log("Turn was Changed is : " + Turn);

        if (Turn == true)
        {
            cameraState.Turn_Black = true;
        }
        if (Turn == false)
        {
            cameraState.Turn_White = true;
        }
        Invoke("A", 2f);
    }
    public void A()
    {
        cameraState.Turn_White = false;
        cameraState.Turn_Black = false;
    }


    
    
    //マウスでの選択から諸一の計算
    private Vector2Int Square_Select()
    {
        Vector2Int Square = Calculate_Position.Square_From_Pixel(Hit.point);
        return Square;
    }
  
    private Vector3 Position_Select()
    {
        Vector3 Position = Calculate_Position.Position_From_Square(Square_Select());
        return Position;
    }

    private Vector3 Plot_Select(int x, int y)
    {
        Vector3 Plot = Calculate_Position.PlotPosition_From_Position(Position_Select())[x, y];
        return Plot;
    }


    //駒を指定して諸位置の計算
    private Vector2Int Square_Piece(GameObject Piece)
    {
        Vector2Int Square = Calculate_Position.Square_From_Pixel(Piece.transform.position);
        return Square;
    }

    private Vector3 Position_Piece(GameObject Piece)
    {
        Vector3 Position = Calculate_Position.Position_From_Square(Square_Piece(Piece));
        return Position;
    }

    private Vector3 Plot_Piece(GameObject Piece, int x, int y)
    {
        Vector3 Plot = Calculate_Position.PlotPosition_From_Position(Position_Piece(Piece))[x, y];
        return Plot;
    }
}
