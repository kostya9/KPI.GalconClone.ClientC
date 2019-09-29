﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainScreenLoadCommand : BaseCommand
{

    private const string NAME = "UIMainScreen";
    private const string PATH = "Prefabs/UI/UIMainScreen";

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

        var canvas = GO.GetComponent<Canvas>();

        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
        }

        var view = GO.GetComponent<UIMainScreenView>();

        if (view == null)
        {
            Debug.LogError("GO has no view at " + PATH);
            Fail();
        }

    }

}
