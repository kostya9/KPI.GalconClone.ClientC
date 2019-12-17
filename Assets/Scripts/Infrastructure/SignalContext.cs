
using Assets.Scripts;
using Assets.Scripts.Client;
using Assets.Scripts.Infrastructure;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.signal.impl;
using System.Net.Sockets;
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
            injectionBinder.Bind<MainThreadCommandExecutor>().To(new MainThreadCommandExecutor());
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();

            injectionBinder.Bind<PlanetLayoutStore>().ToSingleton();


            // Bind communication with server
            injectionBinder.Bind<TcpClient>().To(new TcpClient("127.0.0.1", 10800));
            injectionBinder.Bind<ServerClient>().ToSingleton();
        }
    
        protected override void mapBindings() {
            base.mapBindings();
 
            commandBinder.Bind<MapGenerated>().To<RenderMapCommand>();
        }

        public override void Launch() {
            base.Launch();
        }
    }
}
