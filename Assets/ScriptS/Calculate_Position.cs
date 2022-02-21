using UnityEngine;

static class Calculate_Position
{
    static public Vector3 Position_Square(Vector2Int Square)
    {
        float x = -7.0f + 2.0f * Square.x;
        float z = -7.0f + 2.0f * Square.y;
        return new Vector3(x, 0.5f, z);
    }

    static public Vector2Int Square(int X, int Y)
    {
        return new Vector2Int(X, Y);
    }

    static public Vector2Int Square_Pixel(Vector3 Pixel)
    {
        int X = Mathf.FloorToInt(4.0f + Pixel.x / 2);
        int Y = Mathf.FloorToInt(4.0f + Pixel.z / 2);
        return new Vector2Int(X, Y);
    }
}
