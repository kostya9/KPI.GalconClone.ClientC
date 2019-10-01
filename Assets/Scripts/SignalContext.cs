
using DefaultNamespace;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using strange.extensions.signal.impl;
using UnityEngine;

public class SignalContext : MVCSContext {
 
    /**
     * Constructor
     */
    public SignalContext (MonoBehaviour contextView) : base(contextView) {
    }
 
    protected override void addCoreComponents() {
        base.addCoreComponents();
 
        // bind signal command binder
        injectionBinder.Unbind<ICommandBinder>();
        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();

    }
    
    protected override void mapBindings() {
        base.mapBindings();
 
        // Root command
        commandBinder.Bind<GameStartedSignal>().To<StartGameCommand>();
    }
 
    public override void Launch() {
        base.Launch();
        Signal startSignal = injectionBinder.GetInstance<GameStartedSignal>();
        startSignal.Dispatch();
    }
 
}
