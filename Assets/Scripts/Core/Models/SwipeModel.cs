
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeModel
{
    public Vector2 previousPosition;
    public Vector2 position;

    public SwipeModel(Vector2 previousPosition, Vector2 position)
    {
        this.previousPosition = previousPosition;
        this.position = position;
    }
}
