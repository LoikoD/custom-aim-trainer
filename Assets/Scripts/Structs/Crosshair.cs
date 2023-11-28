using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Crosshair
{
    public RectTransform dot;
    public RectTransform leftLine;
    public RectTransform rightLine;
    public RectTransform topLine;
    public RectTransform bottomLine;

    public Crosshair(GameObject obj)
    {
        dot = obj.transform.Find("Dot").GetComponent<RectTransform>();
        Transform lines = obj.transform.Find("Lines");
        leftLine = lines.Find("Left").GetComponent<RectTransform>();
        rightLine = lines.Find("Right").GetComponent<RectTransform>();
        topLine = lines.Find("Top").GetComponent<RectTransform>();
        bottomLine = lines.Find("Bottom").GetComponent<RectTransform>();
    }
}

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
}
