using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMediator : BaseMediator
{
    [Inject] public GameView view { get; set; }

    public override void OnRegister()
    {
        view.LoadView();
    }

    public override void OnRemove()
    {
        view.RemoveView();
    }
}
