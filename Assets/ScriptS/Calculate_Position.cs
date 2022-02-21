using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Calculate_Position
{
    static public Vector2Int Square(int X, int Y)
    {
        return new Vector2Int(X, Y);
    }
   
    static public Vector2Int Square_Pixel(Vector3 Pixel)
    {
        int X = Mathf.FloorToInt(4.0f + Pixel.x / 2);   //タイル座標は(-4, -4)スタートだが、分かり易いように(0, 0)スタートにした。
        int Y = Mathf.FloorToInt(4.0f + Pixel.z / 2);
        return new Vector2Int(X, Y);
    }

    //タイル座標から実際の位置(Vector3)を計算
    static public Vector3 Position_Square(Vector2Int Square)
    {
        float x = -7.0f + 2.0f * Square.x;
        float z = -7.0f + 2.0f * Square.y;
        return new Vector3(x, 0.5f, z);
    }
}
