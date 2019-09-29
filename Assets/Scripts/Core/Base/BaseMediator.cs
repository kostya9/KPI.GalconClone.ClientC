using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMediator : Mediator
{

    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    protected bool wasHidden = false;
    private bool wasUnregistered = false;

    public void Disable()
    {
        wasHidden = true;
        OnSleep();

    }


    private void OnEnable()
    {
        if (wasUnregistered)
        {
            wasUnregistered = false;
            OnRegister();
        }    }

    private void OnDisable()
    {
        if (!wasUnregistered)
        {
            wasUnregistered = true;
            OnRemove();
        }
    }

    public override void OnRegister()
    {
        base.OnRegister();
        wasUnregistered = false;

    }

    public override void OnRemove()
    {
        base.OnRemove();
        wasUnregistered = true;
    }

    protected virtual void OnEnabled()
    {
        OnRegister();
    }
    protected virtual void OnSleep()
    {

    }
    public void Enable()
    {
        if (wasHidden)
        {
            OnEnabled();
            wasHidden = false;
        }
    }
}
