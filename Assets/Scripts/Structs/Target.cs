using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Shape
{
    Sphere,
    Cube
}

[System.Serializable]
public struct TargetConfig
{
    public Shape shape;
    public Vector3 size;
    public Color color;
}
