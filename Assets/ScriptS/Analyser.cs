using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Analyser : MonoBehaviour
{
    Text[] Analyse;
    [SerializeField] Text Analyse0;
    [SerializeField] Text Analyse1;
    [SerializeField] Text Analyse2;
    [SerializeField] Text Analyse3;
    //[SerializeField] Text Analyse4;
    //[SerializeField] Text Analyse5;
    //[SerializeField] Text Analyse6;
    //[SerializeField] Text Analyse7;
    //[SerializeField] Text Analyse8;
    //[SerializeField] Text Analyse9;
    private void Start()
    {
        Analyse = new Text[]
        {
            Analyse0,
            Analyse1,
            Analyse2,
            Analyse3,
            //Analyse4,
            //Analyse5,
            //Analyse6,
            //Analyse7,
            //Analyse8,
            //Analyse9,
        };
        Initialize();
        Component();
    }
    bool[] Switch = new bool[10];

    void Switch_ON()
    {
        for (int a = 0; a < 10; a++)
        {
            if (Input.GetKeyDown(a.ToString()))
            {
                for (int b = 0; b < a; b++)
                {
                    Switch[b] = false;
                }
                for (int b = a + 1; b < 10; b++)
                {
                    Switch[b] = false;
                }
                Switch[a] = true;
            }
        }
    }
    void Initialize()
    {
        for (int a = 0; a < Analyse.Length; a++)
        {
            Analyse[a].text = " ";
        }
        for (int a = 0; a < 10; a++)
        {
            Switch[a] = false;
        }
    }






    Controller controller;

    void Component()
    {
        controller = GameObject.Find("Controller").GetComponent<Controller>();
    }
    void Update()
    {
        Switch_ON();

        if (Input.GetKeyDown(KeyCode.Space)) { Initialize(); }
        if (Switch[0]) { Analyse_Geometry(); }
        if (Switch[1]) { Analyse_Current_Piece(); }
        if (Switch[2]) { }
        if (Switch[3]) { }
        if (Switch[4]) { }
        if (Switch[5]) { }
        if (Switch[6]) { }
        if (Switch[7]) { }
        if (Switch[8]) { }
        if (Switch[9]) { }
    }

    void Analyse_Geometry()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point1 = hit.point;
            Vector2Int gridPoint = Calculate_Position.Square_From_Pixel(point1);
            Vector3 point2 = Calculate_Position.Position_From_Square(gridPoint);

            Analyse[0].text = "Grid  : " + gridPoint.ToString();
            Analyse[1].text = "Point1 : " + point1.ToString();
            Analyse[2].text = "Point2 : " + point2.ToString(); ;
        }
    }

    void Analyse_Current_Piece()
    {
        Analyse[0].text = "Current : " + controller.Piece_Current.ToString();
        Analyse[1].text = "Select : " + controller.Piece_Selected.ToString();
    }
}
