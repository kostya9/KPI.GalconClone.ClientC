using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainScreenView : BaseView
{

    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    
    public void LoadView()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }

    public void RemoveView()
    {

    }

    private void OnPlayButtonClick()
    {
        dispatcher.Dispatch(Events.GameLoad);
        dispatcher.Dispatch(Events.UIMainScreenRemove);
    }

    private void OnSettingsButtonClick()
    {
        dispatcher.Dispatch(Events.UISettingsLoad);
        dispatcher.Dispatch(Events.UIMainScreenRemove);
    }
}
