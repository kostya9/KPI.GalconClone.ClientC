using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRemoveCommand : BaseCommand
{
    private const string VIEW_NAME = "Game";
    public override void Execute()
    {
        var view = GameObject.Find(VIEW_NAME);

        if (view == null)
        {
            Fail();
            return;
        }

        GameObject.Destroy(view);
        view = null;
    }
}
