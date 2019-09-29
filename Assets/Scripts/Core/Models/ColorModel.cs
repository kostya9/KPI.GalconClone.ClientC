
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorModel
{
    public Color color;
    public string tagName;
    public bool canTouch;

    public ColorModel(Color color, string tagName, bool canTouch)
    {
        this.canTouch = canTouch;
        this.color = color;
        this.tagName = tagName;
    }
}
