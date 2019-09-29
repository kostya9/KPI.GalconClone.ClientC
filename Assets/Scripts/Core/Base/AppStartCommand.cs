using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppStartCommand : BaseCommand {

    public override void Execute()
    {
        executor.Execute(WaitAndExecute());
    }
	
    private IEnumerator WaitAndExecute()
    {
        yield return null;
        dispatcher.Dispatch(Events.UIMainScreenLoad);
        dispatcher.Dispatch(Events.BGMusicLoad);
        dispatcher.Dispatch(Events.TouchEventSystemLoad);
    }
}
