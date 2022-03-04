using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChessAgent : MonoBehaviour
{
    public void Start()
    {
        Invoke("Component", 1f);
        Invoke("Initialize", 2f);
        //Initialize();
    }


    
    public void Update()
    {
        if (action && cameraState.Turn_White == false && cameraState.Turn_Black == false) 
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


    DateTime date = DateTime.Now;
    public bool action = false;
    public int index_White = 0;
    public int index_Black = 0;
    public GameObject[] White = null;
    public GameObject[] Black = null;
    public void Initialize()
    {
        White = new GameObject[store_Piece.PieceS.Length / 2];
        Black = new GameObject[store_Piece.PieceS.Length / 2];
        foreach (var item in store_Piece.PieceS)
        {
            Piece EachPiece = item.GetComponent<Piece>();
            if (EachPiece.color == PicceColor.White)
            {
                index_White ++;
                White[index_White - 1] = item;
            }
            else if (EachPiece.color == PicceColor.Black)
            {
                index_Black ++;
                Black[index_Black - 1] = item;
            }
            //else { Debug.Log("なんでやねん"); }
        }

        int seed = date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond;
        UnityEngine.Random.InitState(seed);
        
        action = true;
    }



    public Ray ray;
    public RaycastHit Hit;
    public bool Point;
    public GameObject Selection;

    public void Action()
    {
        if (cameraState.Turn_White == false && cameraState.Turn_Black == false)//ターン遷移中じゃない時
        {
            Selector();
            while (Judge() == false)
            {
                Selector();
            }    
        }
       
        ray = new Ray(Camera.main.transform.position, Selection.transform.position - Camera.main.transform.position);
        Point = Physics.Raycast(ray, out Hit);

        //action = false;

        if (Selection != null)
        {
            action = false;
        }
    }

    
    public void Selector()
    {
        if (controller.Turn == true)
        {
            Selection = White[UnityEngine.Random.Range(0, White.Length)];
            Debug.Log(Selection);
        }
        else if (controller.Turn == false)
        {
            Selection = Black[UnityEngine.Random.Range(0, Black.Length)];
        }
        else
        {
            Debug.Log("No Selection");
            Selection = null;
        }

        //if(Selection != null)
        //{
        //}
    }


    public bool Judge()
    {
        Piece piece = Selection.GetComponent<Piece>();
        Vector2Int square = Calculate_Position.Square_From_Pixel(Selection.transform.position);

        List<Vector2Int> Move = piece.Square_Move(square);
        List<Vector2Int> Attack = piece.Square_Attack(square);

        if(Move.Count != 0 || Attack.Count != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
