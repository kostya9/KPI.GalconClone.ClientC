using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.api;
using strange.extensions.sequencer.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCommand : Command
{

    [Inject]
    public IMediationBinder mediationBinder { get; set; }

    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    [Inject]
    public IExecutor executor { get; set; }

    [Inject]
    public IEvent eventData { get; set; }

    public override void Execute()
    {

    }
}
