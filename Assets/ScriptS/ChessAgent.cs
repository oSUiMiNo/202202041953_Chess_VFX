using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAgent : MonoBehaviour
{
    public void Start()
    {
        Invoke("Component", 1f);
        Invoke("InitializeAgent", 2f);
    }


    public void Update()
    {
        if (action == true && cameraState.Turn_White == false && cameraState.Turn_Black == false)
        {
            Action();
        }
    }


    Controller controller;
    Store_Piece store_Piece;
    Camera_State cameraState;
    public void Component()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
        store_Piece = GameObject.Find("Store_Piece").GetComponent<Store_Piece>();
        cameraState = Camera.main.GetComponent<Animator>().GetBehaviour<Camera_State>();
    }


    System.DateTime date = System.DateTime.Now;
    public bool action = false;
    public int index_White = 0;
    public int index_Black = 0;
    public GameObject[] White = null;
    public GameObject[] Black = null;
    public void InitializeAgent()
    {
        White = new GameObject[store_Piece.PieceS.Length / 2];
        Black = new GameObject[store_Piece.PieceS.Length / 2];
        foreach (var item in store_Piece.PieceS)
        {
            Piece EachPiece = item.GetComponent<Piece>();
                if (EachPiece.color == PicceColor.White)
                {
                    index_White++;
                    White[index_White - 1] = item;
                }
                else if (EachPiece.color == PicceColor.Black)
                {
                    index_Black++;
                    Black[index_Black - 1] = item;
                }
                //else { Debug.Log("なんでやねん"); }
        }

        int seed = date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond;
        Random.InitState(seed);

        action = true;
    }

    public void UpdateAgent()
    {
        index_White = index_Black = 0;
        foreach (var item in store_Piece.PieceS)
        {
            if (item.activeSelf == true)
            {
                Piece EachPiece = item.GetComponent<Piece>();
                if (EachPiece.color == PicceColor.White)
                {
                    index_White++;
                    White[index_White - 1] = item;
                }
                else if (EachPiece.color == PicceColor.Black)
                {
                    index_Black++;
                    Black[index_Black - 1] = item;
                }
                //else { Debug.Log("なんでやねん"); }
            }
        }
        action = true;

        Debug.Log("UpdateAgent");
    }


    int a = 0;
    public Ray ray;
    public RaycastHit Hit;
    public string Tag;
    public bool Point;
    public int ActionCount;
    public bool piece;
    public bool move;
    public GameObject PSelection;
    public Vector3 MSelection;
    public void Action()
    {
        StartCoroutine(Piece_Action());
        action = false;
    }


    public IEnumerator Piece_Action()
    {
        piece = true;
        yield return new WaitForSeconds(1f);
        //Debug.Log("1  " + "Piece_Action IN");

        //駒選択
        if (cameraState.Turn_White == false && cameraState.Turn_Black == false)//ターン遷移中じゃない時
        {
            //yield return new WaitForSeconds(1f);
            //Debug.Log("2  " + "1回目");

            PSelection = Piece_Select();
            ray = new Ray(Camera.main.transform.position, PSelection.transform.position - Camera.main.transform.position);
            Point = Physics.Raycast(ray, out Hit);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 7f, true);

            while (Judge(PSelection) == false)
            {
                yield return new WaitForSeconds(0.5f);
                
                PSelection = Piece_Select();
                ray = new Ray(Camera.main.transform.position, PSelection.transform.position - Camera.main.transform.position);
                Point = Physics.Raycast(ray, out Hit);
                Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 7f, true);
            }
            //Debug.Log("5  " + "Ultimate Selection is : " + PSelection);
        }
        //Debug.Log("   " + "Do Start");
        Do();
        //Debug.Log("   " + "Do End");

        piece = false;
        //Debug.Log("   " + "Piece_Action OUT");

        StartCoroutine(Move_Action());
    }

    public IEnumerator Move_Action()
    {
        yield return new WaitForSeconds(1f);
        //Debug.Log("6  " + "Move_Action IN");

        //行先選択
        MSelection = Move_Select();

        ray = new Ray(Camera.main.transform.position, MSelection - Camera.main.transform.position);
        Point = Physics.Raycast(ray, out Hit);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.green, 6, true);
        //foreach (var item in Physics.RaycastAll(ray))
        //{
        //Debug.Log("Hitした : " + item.transform.name);
        //}

        if(Point == false)
        {
            Debug.Log("   " + "No Hit");

            ray = new Ray(Camera.main.transform.position, PSelection.transform.position - Camera.main.transform.position);
            Point = Physics.Raycast(ray, out Hit);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 7f, true);
            
            Debug.Log("   " + "Do Start");
            Do();
            Debug.Log("   " + "Do End");
        }

        //while (Point == false)
        //{
        //    MSelection = Move_Select();
        
        //    ray = new Ray(Camera.main.transform.position, MSelection - Camera.main.transform.position);
        //    Point = Physics.Raycast(ray, out Hit);
        //    Debug.DrawRay(ray.origin, ray.direction * 50, Color.green, 6, false);
        //}
        //if (Point)
        //{
        //    Tag = Hit.collider.gameObject.tag;
        //}
        //Debug.Log(Hit.collider.gameObject.tag);

        //Debug.Log("   " + "Do Start");
        Do();
        //Debug.Log("   " + "Do End");

        //Debug.Log("7  " + "Move_Action OUT");
    }

    
    public GameObject Piece_Select()
    {
        GameObject Selection;
        if (controller.Turn == true)
        {
            Selection = White[Random.Range(0, White.Length)];
            //Debug.Log(Selection);
        }
        else if (controller.Turn == false)
        {
            Selection = Black[Random.Range(0, Black.Length)];
            //Debug.Log(Selection);
        }
        else
        {
            Selection = null;
            //Debug.Log("No Selection");
        }

        //Debug.Log("   " + "Piece_Select.  Selection is : " + Selection);
        return Selection;
    }


    public List<Vector2Int> MoveSquare;
    public List<Vector2Int> AttackSquare;
    public bool Judge(GameObject Selection)
    {
        //Debug.Log("3  " + "Juge_IN");

        Piece piece = Selection.GetComponent<Piece>();
        Vector2Int square = Calculate_Position.Square_From_Pixel(Selection.transform.position);
        MoveSquare = piece.Square_Move(square);
        AttackSquare = piece.Square_Attack(square);

        //Debug.Log(MoveSquare.Count + "," + AttackSquare.Count);

        if ((MoveSquare.Count >= 1 || AttackSquare.Count >= 1) && Tag != null)
        {
            //Debug.Log("4  " + "Judge_OUT : true");
            return true;
        }
        else
        {
            //Debug.Log("4  " + "Judge_OUT : false");
            return false;
        }
    }



    public Vector3 Move_Select()
    {
        Vector2Int Selection;

        int Move_or_Attack = Random.Range(0, 2);
        //Debug.Log(Move_or_Attack);

        if((Move_or_Attack == 0 || AttackSquare.Count < 1) && MoveSquare.Count >= 1)
        {
            Selection = MoveSquare[Random.Range(0, MoveSquare.Count)];
        }
        else
        {
            Selection = AttackSquare[Random.Range(0, AttackSquare.Count)];
        }

        //Debug.Log("   " + "Move_Select");
        return Calculate_Position.Position_From_Square(Selection);  //vector3座標に変換して返す。
    }



    public void Do()
    {
        controller.Agent();
    }
}
