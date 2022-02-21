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
    private void Component()
    {
        Pool = GameObject.Find("Pool");
        pool_Highlight_Yellow = Pool.GetComponent<Pool_Highlight_Yellow>();
        pool_Highlight_Blue = Pool.GetComponent<Pool_Highlight_Blue>();
        pool_Highlight_Red = Pool.GetComponent<Pool_Highlight_Red>();
        pool_Highlight_Selected = Pool.GetComponent<Pool_Highlight_Selected>();
        store_Piece = GameObject.Find("Store_Piece").GetComponent<Store_Piece>();
        text = GameObject.Find("Inform").GetComponent<Text>();
    }




    public float Times;
    public float StateTime;
    public void Update()
    {
        Pointer();

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
    public RaycastHit[] Hit_All;  //rayが衝突した全オブジェクトの情報
    public RaycastHit Hit;
    public bool Point, Click;
    //public string Tag;
    private void Pointer()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Hit_All = Physics.RaycastAll(ray);
        Point = Physics.Raycast(ray, out Hit);
        Click = Input.GetMouseButtonDown(0);

        if (Point)
        {
            GameObject HitObject = Hit.collider.gameObject;
            string Tag = HitObject.tag;
            Slect_Selector(HitObject, Tag);

            //foreach (RaycastHit Hit in Hit_All)
            //{
            //    Tag = Hit.collider.gameObject.tag;
            //    //Debug.Log(Tag);
            //    Slect_Selector();
            //}
        }
    }







    public GameObject Piece_Selected, Piece_Current = null;
    private void Slect_Selector(GameObject HitObject, string Tag)
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
        pool_Highlight_Yellow.Object_Discharge(Position());
    }

    
    public void Selector_Piece(GameObject Piece_Selected)
    {
        Piece_Selected.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
        Piece_Selected.transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = true;
        
        Piece piece = Piece_Selected.GetComponent<Piece>();
        for (int a = 0; a < piece.Square_Move(Square()).Count; a++)
        {
            pool_Highlight_Blue.Object_Discharge((Calculate_Position.Position_Square(piece.Square_Move(Square())[a])));
            pool_Highlight_Selected.Object_Discharge(Position());
        }
        for (int a = 0; a < piece.Square_Attack(Square()).Count; a++)
        {
            pool_Highlight_Red.Object_Discharge((Calculate_Position.Position_Square(piece.Square_Attack(Square())[a])));
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

        store_Piece.Piece_Square(Square()).SetActive(false);

        Piece piece = Piece_Current.GetComponent<Piece>();
        if (piece.type == PieceType.Pawn)
        {
            piece.first = false;
        }
    }




    //動かすアニメーションの選択
    Camera_State CameraState;
    public bool Aniamtion_Active;
    Animator animator;
    Piece_State PieceState;
    Vector3 D;
    public void Select_Animation(GameObject Piece_Current)
    {

        //カメラのアニメーション
        CameraState = Camera.main.GetComponent<Animator>().GetBehaviour<Camera_State>();

        //駒のアニメーション
        Aniamtion_Active = true;  //Currentフラグのアップデート等を1回しか行いたくないが、ステートマシーンビヘイビアのTimesを使うと、アップデートが二回実行されてしまうのでこのフラグで1回しか実行できないようにする。

        animator = Piece_Current.GetComponent<Animator>();
        PieceState = animator.GetBehaviour<Piece_State>();
        animator.enabled = true;


        //駒の、方向の判定 & アニメーションフラグの指定
        Vector3 Delta = Position() - Piece_Current.transform.position;
        //Debug.Log("Delta : " + Delta);

        D = Delta / 2;
        //ナイト
        if (D.x == 1 && D.z == 2)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(1, 2)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (D.x == 2 && D.z == 1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(2, 1)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (D.x == 2 && D.z == -1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(2, -1)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (D.x == 1 && D.z == -2)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(1, -2)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (D.x == -1 && D.z == -2)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(-1, -2)";
            PieceState.Bool1 = true;

            return;
        }
        else if (D.x == -2 && D.z == -1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(-2, -1)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (D.x == -2 && D.z == 1)
        {
            PieceState.Times = 1;
            PieceState.Flag1 = "K(-2, 1)";
            PieceState.Bool1 = true;
            Delay = 3;
            return;
        }
        else if (D.x == -1 && D.z == 2)
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
                //Debug.Log("D : " + D);
                //まっすぐ
                if (D.x == 1 && D.z == 0)
                {
                    PieceState.Times = Delta.x / 2;
                    PieceState.Flag1 = "(1, 0)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (D.x == -1 && D.z == 0)
                {
                    PieceState.Times = Delta.x / 2 * -1;
                    PieceState.Flag1 = "(-1, 0)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (D.x == 0 && D.z == 1)
                {
                    PieceState.Times = Delta.z / 2;
                    PieceState.Flag1 = "(0, 1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (D.x == 0 && D.z == -1)
                {
                    PieceState.Times = Delta.z / 2 * -1;
                    PieceState.Flag1 = "(0, -1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                //斜め
                else if (D.x == 1 && D.z == 1)
                {
                    PieceState.Times = Delta.x / 2;
                    PieceState.Flag1 = "(1, 1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (D.x == 1 && D.z == -1)
                {
                    PieceState.Times = Delta.x / 2;
                    PieceState.Flag1 = "(1, -1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (D.x == -1 && D.z == 1)
                {
                    PieceState.Times = Delta.x / 2 * -1;
                    PieceState.Flag1 = "(-1, 1)";
                    PieceState.Bool1 = true;
                    Delay = a;
                    break;
                }
                else if (D.x == -1 && D.z == -1)
                {
                    PieceState.Times = Delta.x / 2 * -1;
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
        if (D.x == 1 || D.x == 2)
        {
            X = Mathf.FloorToInt(Piece_Current.transform.position.x);
        }
        if(D.z == 1 || D.z == 2)
        {
            Y = Mathf.FloorToInt(Piece_Current.transform.position.z);
        }
        if (D.x == -1 || D.x == -2)
        {
            X = Mathf.FloorToInt(Piece_Current.transform.position.x + 0.8f);
        }
        if (D.z == -1 || D.z == -2)
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
            CameraState.Turn_Black = true;
        }
        if (Turn == false)
        {
            CameraState.Turn_White = true;
        }
        Invoke("A", 2f);
    }
    public void A()
    {
        CameraState.Turn_White = false;
        CameraState.Turn_Black = false;
    }


    //マス座標から実際に配置する位置を計算
    private Vector3 Position()
    {
        Vector3 Position = Calculate_Position.Position_Square(Square());
        return Position;
    }

    //レイのhit位置から、マスの座標を判定する
    private Vector2Int Square()
    {
        Vector2Int Square = Calculate_Position.Square_Pixel(Hit.point);
        return Square;
    }
}
