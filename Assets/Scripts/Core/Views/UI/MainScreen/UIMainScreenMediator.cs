using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainScreenMediator : BaseMediator
{
    [Inject] public UIMainScreenView view { get; set; }

    public override void OnRegister()
    {
        view.LoadView();
    }

    public override void OnRemove()
    {
        view.RemoveView();
    }
}
