using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicLoadCommand : BaseCommand
{
    private const string NAME = "BGMusic";
    private const string PATH = "Prefabs/BGMusic";

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

        var view = GO.GetComponent<SFXSoundVolumeBehaviour>();

        if (view == null)
        {
            Debug.LogError("GO has no view at " + PATH);
            Fail();
        }

    }
}
