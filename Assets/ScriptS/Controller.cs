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

    public GameObject CurrentLight_0, CurrentLight_1, Light, Pendant_Light;
    public void Objects()
    {
        Light = GameObject.Find("Light");
        Pendant_Light = GameObject.Find("Light_Pendant");
        CurrentLight_0 = GameObject.Find("Light_Current_0");
        CurrentLight_1 = GameObject.Find("Light_Current_1");
    }


    public float Times;
    public float StateTime;
    public void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (ChessAgent.enabled == false)
        {
            Human();
        }
        //else if (ChessAgent.enabled == true)
        //{
        //    Agent();
        //}

        if (animator != null && animator.enabled == true)
        {
            if (PieceState.Bool1 == false)
            {
                Times = PieceState.Times;
                StateTime = PieceState.StateTime;
                //Debug.Log("Co  " + "StateTime : " + StateTime + "Times : " + Times);

                if (StateTime >= Times && Aniamtion_Active == true)
                {
                    //Debug.Log("a");
                    StartCoroutine(FixPosition_Piece());
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
    public void Human()
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

    public void Agent()
    {
        ray = ChessAgent.ray;
        Hit = ChessAgent.Hit;
        Point = ChessAgent.Point;
        Click = true;

        if (Point)
        {
            GameObject HitObject = Hit.collider.gameObject;
            string Tag = HitObject.tag;
            //Debug.Log(Tag);
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
            //Debug.DrawRay(ray.origin, ray.direction * 50, Color.blue, 3f, true);
            //Debug.Log(Hit.collider.gameObject.name);

            if (Tag == "PieceS_White" || Tag == "PieceS_Black" )
            {
                //Debug.Log("Select_Selector Select");
                DeSelector_Piece(Piece_Current);
                Piece_Selected = HitObject;
                Selector_Piece(Piece_Selected);
                return;
            }
            else if (Piece_Current != Hit.collider)
            {
                if(Hit.collider.gameObject.tag == "Highlight_Blue")
                {
                    //Debug.Log("Select_Selector Move");
                    DeSelector_Piece(Piece_Current);
                    Move_Piece(Piece_Current);
                    return;
                }
                else if(Hit.collider.gameObject.tag == "Highlight_Red")
                {
                    //Debug.Log("Select_Selector Attack");
                    DeSelector_Piece(Piece_Current);
                    Attack(Piece_Current);
                    return;
                }
                else
                {
                    //Debug.Log("Select_Selector else");
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
        List<Vector2Int> SquareMove = piece.Square_Move(Square_Select());
        List<Vector2Int> SquareAttack = piece.Square_Attack(Square_Select());
        for (int a = 0; a < SquareMove.Count; a++)
        {
            pool_Highlight_Blue.Object_Discharge((Calculate_Position.Position_From_Square(SquareMove[a])));
            pool_Highlight_Selected.Object_Discharge(Position_Select());
        }
        for (int a = 0; a < SquareAttack.Count; a++)
        {
            pool_Highlight_Red.Object_Discharge((Calculate_Position.Position_From_Square(SquareAttack[a])));
        }
        
        Piece_Current = Piece_Selected;
        //Debug.Log("Selector_Piece");
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
        if(ChessAgent.enabled == false)
        {
            Select_Animation(Piece_Current);
        }
        else
        {
            Piece_Current.transform.position = Position_Select();
            store_Piece.Update_Square_Piece();
            store_Piece.Update_Flag_Current();
            StartCoroutine(Change_Turn());
        }

        Piece piece = Piece_Current.GetComponent<Piece>();
        if (piece.type == PieceType.Pawn)
        {
            piece.first = false;
        }
        
        //Debug.Log("Move_Piece");
    }


    public void Attack(GameObject Piece_Current)
    {
        Piece piece = Piece_Current.GetComponent<Piece>();
        GameObject subject = store_Piece.Piece_Square(Square_Select());

        if (ChessAgent.enabled == false)
        {
            Select_Animation(Piece_Current);
        }
        else
        {
            Piece_Current.transform.position = Position_Select();
            store_Piece.Update_Square_Piece();
            store_Piece.Update_Flag_Current();
            StartCoroutine(Change_Turn());
        }

        if (subject.GetComponent<Piece>().type == PieceType.King)
        {
            GameManager.instance.CHECKMATE();
        }
        else if (Piece_Current.GetComponent<Piece>().type == PieceType.Pawn)
        {
            piece.first = false;
        }
        subject.SetActive(false);
        //Debug.Log("Attack");
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
            return;
        }
        else if (DX == 2 && DY == 1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(2, 1)";
            PieceState.Bool1 = true;
            return;
        }
        else if (DX == 2 && DY == -1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(2, -1)";
            PieceState.Bool1 = true;
            return;
        }
        else if (DX == 1 && DY == -2)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(1, -2)";
            PieceState.Bool1 = true;
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
            return;
        }
        else if (DX == -2 && DY == 1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(-2, 1)";
            PieceState.Bool1 = true;
            return;
        }
        else if (DX == -1 && DY == 2)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(-1, 2)";
            PieceState.Bool1 = true;
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
                    break;
                }
                else if (DX == -1 && DY == 0)
                {
                    PieceState.Times = DeltaX * -1;
                    PieceState.Flag1 = "(-1, 0)";
                    PieceState.Bool1 = true;
                    break;
                }
                else if (DX == 0 && DY == 1)
                {
                    PieceState.Times = DeltaY;
                    PieceState.Flag1 = "(0, 1)";
                    PieceState.Bool1 = true;
                    break;
                }
                else if (DX == 0 && DY == -1)
                {
                    PieceState.Times = DeltaY * -1;
                    PieceState.Flag1 = "(0, -1)";
                    PieceState.Bool1 = true;
                    break;
                }
                //斜め
                else if (DX == 1 && DY == 1)
                {
                    PieceState.Times = DeltaX;
                    PieceState.Flag1 = "(1, 1)";
                    PieceState.Bool1 = true;
                    break;
                }
                else if (DX == 1 && DY == -1)
                {
                    PieceState.Times = DeltaX;
                    PieceState.Flag1 = "(1, -1)";
                    PieceState.Bool1 = true;
                    break;
                }
                else if (DX == -1 && DY == 1)
                {
                    PieceState.Times = DeltaX * -1;
                    PieceState.Flag1 = "(-1, 1)";
                    PieceState.Bool1 = true;
                    break;
                }
                else if (DX == -1 && DY == -1)
                {
                    PieceState.Times = DeltaX * -1;
                    PieceState.Flag1 = "(-1, -1)";
                    PieceState.Bool1 = true;
                    break;
                }
                //else
                //{
                //    Debug.Log("わー" + a);
                //}
            }
        }
    }



    
    public IEnumerator FixPosition_Piece()
    {
        yield return new WaitForSeconds(0.1f);
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
        StartCoroutine(Change_Turn());
    }




    //ターン交代
    public bool Turn;
    public void Initialize_Game()
    {
        Turn = true;
        FixPosition_Camera();
        Light.SetActive(true);
        Pendant_Light.SetActive(true);
        CurrentLight_0.SetActive(false);
        CurrentLight_1.SetActive(true);
    }
    public IEnumerator Change_Turn()
    {
        Turn = !Turn;

        if(ChessAgent.enabled == false)
        {
            if (Turn == true)
            {
                yield return new WaitForSeconds(2f);
                cameraState.Turn_White = true;
                Debug.Log(1);

                yield return new WaitForSeconds(0.4f);
                Pendant_Light.SetActive(false);
                CurrentLight_0.SetActive(false);
                Debug.Log(2);

                yield return new WaitForSeconds(1f);
                cameraState.Turn_White = false;
                cameraState.Turn_Black = false;
                Debug.Log(3);

                yield return new WaitForSeconds(4f);
                Pendant_Light.SetActive(true);
                CurrentLight_1.SetActive(true);
                Debug.Log(4);
            }
            else
            {
                yield return new WaitForSeconds(2f);
                cameraState.Turn_Black = true;
                Debug.Log(1);

                yield return new WaitForSeconds(0.4f);
                Pendant_Light.SetActive(false);
                CurrentLight_1.SetActive(false);
                Debug.Log(2);

                yield return new WaitForSeconds(1f);
                cameraState.Turn_White = false;
                cameraState.Turn_Black = false;
                Debug.Log(3);

                yield return new WaitForSeconds(4f);
                Pendant_Light.SetActive(true);
                CurrentLight_0.SetActive(true);
                Debug.Log(4);
            }
            yield return new WaitForSeconds(2f);
            FixPosition_Camera();
        }
        else
        {
            yield return new WaitForSeconds(0f);
            ChessAgent.UpdateAgent();
        }
        //Debug.Log("Turn was Changed is : " + Turn);
    }


    public void FixPosition_Camera()
    {
        if (Turn == true)
        {
            Camera.main.GetComponent<Transform>().position = new Vector3(-3, 12, -12);
            Camera.main.GetComponent<Transform>().rotation = Quaternion.Euler(45, 0, 0);
            //Debug.Log("Camera was Fixed");
        }
        else
        {
            Camera.main.GetComponent<Transform>().position = new Vector3(3, 12, 12);
            Camera.main.GetComponent<Transform>().rotation = Quaternion.Euler(45, 180, 0);
            //Debug.Log("Camera was Fixed");
        }
    }
 

    
    
    //マウスでの選択から諸一の計算
    public Vector2Int Square_Select()
    {
        Vector2Int Square = Calculate_Position.Square_From_Pixel(Hit.point);
        return Square;
    }
  
    public Vector3 Position_Select()
    {
        Vector3 Position = Calculate_Position.Position_From_Square(Square_Select());
        return Position;
    }

    public Vector3 Plot_Select(int x, int y)
    {
        Vector3 Plot = Calculate_Position.PlotPosition_From_Position(Position_Select())[x, y];
        return Plot;
    }


    //駒を指定して諸位置の計算
    public Vector2Int Square_Piece(GameObject Piece)
    {
        Vector2Int Square = Calculate_Position.Square_From_Pixel(Piece.transform.position);
        return Square;
    }

    public Vector3 Position_Piece(GameObject Piece)
    {
        Vector3 Position = Calculate_Position.Position_From_Square(Square_Piece(Piece));
        return Position;
    }

    public Vector3 Plot_Piece(GameObject Piece, int x, int y)
    {
        Vector3 Plot = Calculate_Position.PlotPosition_From_Position(Position_Piece(Piece))[x, y];
        return Plot;
    }
}
