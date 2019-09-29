using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadCommand : BaseCommand
{
    private const string NAME = "Game";
    private const string PATH = "Prefabs/Game";

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

        var view = GO.GetComponent<GameView>();

        if (view == null)
        {
            Debug.LogError("GO has no view at " + PATH);
            Fail();
        }

    }
}
