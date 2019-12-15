
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class SignalContext : MVCSContext 
    {
 
        /**
         * Constructor
         */
        public SignalContext (MonoBehaviour contextView, ContextStartupFlags flags) : base(contextView, flags) {
        }
 
        protected override void addCoreComponents() {
            base.addCoreComponents();
 
            // bind signal command binder
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
            injectionBinder.Bind<PlanetLayoutStore>().ToSingleton();

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
}
