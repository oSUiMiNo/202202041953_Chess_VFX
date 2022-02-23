using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Calculate_Position
{
    static public Vector2Int Square(int X, int Y)
    {
        return new Vector2Int(X, Y);
    }
   
    static public Vector2Int Square_From_Pixel(Vector3 Pixel)
    {
        int X = Mathf.FloorToInt(4.0f + Pixel.x / 2);   //タイル座標は(-4, -4)スタートだが、分かり易いように(0, 0)スタートにした。
        int Y = Mathf.FloorToInt(4.0f + Pixel.z / 2);
        return new Vector2Int(X, Y);
    }

    //タイル座標から実際の位置(Vector3)を計算
    static public Vector3 Position_From_Square(Vector2Int Square)
    {
        float x = -7.0f + 2.0f * Square.x;
        float z = -7.0f + 2.0f * Square.y;
        return new Vector3(x, 0.5f, z);
    }
 
    static public Vector3[,] PlotPosition_From_Position(Vector3 Position)
    {
        Vector3[,] PlotPosition = new Vector3[3, 3];
        for (int a = 0; a < 3; a++)
        {
            for (int b = 0; b < 3; b++)
            {
                PlotPosition[a, b] = Position - new Vector3(0.5f, 0, 0.5f) + new Vector3(a * 0.5f, 0, b * 0.5f);
            }
        }

        return PlotPosition;
    }
}
