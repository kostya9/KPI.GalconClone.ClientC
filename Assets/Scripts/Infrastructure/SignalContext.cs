
using Assets.Scripts;
using Assets.Scripts.Client;
using Assets.Scripts.Config;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.Players;
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
            // make sure all commands from signals are executed on the main thread
            // Because signals can be triggered from server communicating thread
            injectionBinder.Bind<MainThreadCommandExecutor>().To(new MainThreadCommandExecutor());
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();

            injectionBinder.Bind<PlanetLayoutStore>().ToSingleton();
            injectionBinder.Bind<UnitLayoutStore>().ToSingleton();
            injectionBinder.Bind<PlayerTable>().ToSingleton();

            injectionBinder.Bind<ServerToClientCoordinateTranslator>().ToSingleton();

            // Bind communication with server
            injectionBinder.Bind<TcpClient>().To(new TcpClient(Server.Address, Server.Port));
            injectionBinder.Bind<ServerClient>().ToSingleton();
        }
    
        protected override void mapBindings() {
            base.mapBindings();
 
            commandBinder.Bind<MapGenerated>().To<RenderMapCommand>();
            commandBinder.Bind<PlanetSelected>().To<RenderUnitsCommand>();
            commandBinder.Bind<HpAdded>().To<AddHpCommand>();
        }

        public override void Launch() {
            base.Launch();
        }
    }
}
