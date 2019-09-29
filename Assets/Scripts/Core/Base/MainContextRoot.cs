using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainContextRoot : MVCSContext
{

    public MainContextRoot(MonoBehaviour view) : base(view)
    {

    }

    public MainContextRoot(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
        injectionBinder.Bind<IExecutor>().To<Executor>().ToSingleton();
        
        //The START event is fired as soon as mappings are complete.
        //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
        commandBinder.Bind(ContextEvent.START).To<AppStartCommand>().Once().Pooled();

        // ui
        mediationBinder.BindView<UIMainScreenView>().ToMediator<UIMainScreenMediator>();
        mediationBinder.BindView<UISettingsView>().ToMediator<UISettingsMediator>();



        commandBinder.Bind(Events.UIMainScreenLoad).To<UIMainScreenLoadCommand>();
        commandBinder.Bind(Events.UIMainScreenRemove).To<UIMainScreenRemoveCommand>();
        commandBinder.Bind(Events.UISettingsLoad).To<UISettingsLoadCommand>();
        commandBinder.Bind(Events.UISettingsRemove).To<UISettingsRemoveCommand>();

        // ui end
        mediationBinder.BindView<SFXSoundVolumeBehaviour>().ToMediator<SFXSoundVolumeMediator>();
        mediationBinder.BindView<TouchEventSystemView>().ToMediator<TouchEventSystemMediator>();
        mediationBinder.BindView<GameView>().ToMediator<GameMediator>();
        commandBinder.Bind(Events.BGMusicLoad).To<BGMusicLoadCommand>();
        commandBinder.Bind(Events.GameLoad).To<GameLoadCommand>();
        commandBinder.Bind(Events.GameRemove).To<GameRemoveCommand>();
        commandBinder.Bind(Events.TouchEventSystemLoad).To<TouchEventSystemLoadCommand>();
        commandBinder.Bind(Events.GenerateLevel).To<GenerateLevelCommand>();
    }


    //You can safely ignore this bit. Since changing our default to Signals from Events, this is now necessary in this example.
    protected override void addCoreComponents()
    {
        base.addCoreComponents();
        injectionBinder.Unbind<ICommandBinder>(); //Unbind to avoid a conflict!
        injectionBinder.Bind<ICommandBinder>().To<EventCommandBinder>().ToSingleton();
        injectionBinder.Bind<SFXManager>().ToSingleton();
        injectionBinder.Bind<LevelGeneratorService>().ToSingleton();
        



    }
}
