using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct CrosshairConfig
{
    // Width and height of the dot in pixels
    public float dotSize;

    // Length of lines in pixels
    public float lineLength;

    // Thickness of lines in pixels
    public float lineThickness;

    // Gap between lines in pixels
    public float gap;

    // Color of crosshair
    public Color color;

    public static string DefaultJson()
    {
        CrosshairConfig def = new()
        {
            dotSize = 4f,
            lineLength = 20f,
            lineThickness = 4f,
            gap = 10f,
            color = Color.white
        };
        return JsonUtility.ToJson(def);

    }
}
