using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingsMediator : BaseMediator
{
    [Inject] public UISettingsView view { get; set; }

    public override void OnRegister()
    {
        view.LoadView();
    }

    public override void OnRemove()
    {
        view.RemoveView();
    }
}
