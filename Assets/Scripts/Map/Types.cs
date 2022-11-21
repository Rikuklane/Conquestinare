using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Country
{
    public string name;

    public Province[] provinces;
}

public struct Province
{
    public string name;

    public int ownerID;

    public Shape shape;
}

public struct Shape
{
    public Vector2[] points;

    public Shape(Vector2[] points) {
        this.points = points;
    }

    public int NumPoints
    {
        get
        {
            return points.Length;
        }
    }
}
