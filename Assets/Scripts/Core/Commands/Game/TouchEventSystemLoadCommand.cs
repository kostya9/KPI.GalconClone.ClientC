using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEventSystemLoadCommand : BaseCommand
{
    private const string NAME = "TouchEventSystem";
    private const string PATH = "Prefabs/TouchEventSystem";

    public override void Execute()
    {
        var prefab = Resources.Load<GameObject>(PATH);
        if (prefab == null)
        {
            Debug.LogError("prefab is null at " + PATH);
            Fail();
        }

        var GO = GameObject.Instantiate<GameObject>(prefab);
        GO.name = NAME;

        var view = GO.GetComponent<TouchEventSystemView>();

        if (view == null)
        {
            Debug.LogError("GO has no view at " + PATH);
            Fail();
        }

    }
}
