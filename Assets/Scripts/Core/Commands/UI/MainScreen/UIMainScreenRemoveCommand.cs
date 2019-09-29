using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainScreenRemoveCommand : BaseCommand
{
    private const string VIEW_NAME = "UIMainScreen";
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
