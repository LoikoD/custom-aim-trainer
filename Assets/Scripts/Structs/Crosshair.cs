using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Crosshair
{
    public GameObject crosshairObj;
    public RectTransform dot;
    public RectTransform leftLine;
    public RectTransform rightLine;
    public RectTransform topLine;
    public RectTransform bottomLine;

    public Crosshair(GameObject obj)
    {
        crosshairObj = obj;
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
    public float dotSize;
    public float lineLength;
    public float lineWidth;
    public float gap;
    public Color color;
}
