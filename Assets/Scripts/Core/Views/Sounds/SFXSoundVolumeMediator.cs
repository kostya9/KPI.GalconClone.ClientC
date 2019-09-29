using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSoundVolumeMediator : BaseMediator
{

    [Inject] public SFXSoundVolumeBehaviour view { get; set; }

    public override void OnRegister()
    {
        view.LoadView();
    }

    public override void OnRemove()
    {
        view.RemoveView();
    }
}
