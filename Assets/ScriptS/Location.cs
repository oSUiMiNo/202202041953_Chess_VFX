using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Location : MonoBehaviour 
{
    [SerializeField]public Vector2Int Square;
    [SerializeField]public Vector3 Position;
    [SerializeField]public Vector3 Pixel;
    
    private void Update()
    {
        Square = Calculate_Position.Square_From_Pixel(transform.position);
        Pixel = transform.position;
        Position = Calculate_Position.Position_From_Square(Square);
    }
}
