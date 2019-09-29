using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPartCell : MonoBehaviour
{
    [SerializeField] List<Renderer> renderers;
    public bool defaultColor = true;
    private ColorModel colorModel;

    public void SetColor(ColorModel colorModel)
    {
        this.colorModel = colorModel;
        foreach (var renderer in renderers)
        {
            renderer.material.color = colorModel.color;
            renderer.gameObject.tag = colorModel.tagName;
        }
    }
}
