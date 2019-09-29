using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsView : BaseView
{

    [Inject] public SFXManager sfxManager { get; set; }

    [SerializeField] Button musicOnButton;
    [SerializeField] Button musicOffButton;
    [SerializeField] Button backButton;
     
    [SerializeField] GameObject musicOnEnabledImage;
    [SerializeField] GameObject musicOnDisabledImage;
    [SerializeField] GameObject musicOffEnabledImage;
    [SerializeField] GameObject musicOffDisabledImage;

    

    public void LoadView()
    {
        musicOnButton.onClick.AddListener(OnMusicOnClick);
        musicOffButton.onClick.AddListener(OnMusicOffClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        SetImages(sfxManager.MusicVolume > float.Epsilon);

    }

    public void RemoveView()
    {
        
    }

    private void OnBackButtonClick()
    {
        dispatcher.Dispatch(Events.UIMainScreenLoad);
        dispatcher.Dispatch(Events.UISettingsRemove);
    }

    private void OnMusicOnClick()
    {
        sfxManager.SetMusic(true);
        SetImages(true);
        dispatcher.Dispatch(Events.SFXOnVolumeChanged);
    }

    private void OnMusicOffClick()
    {
        sfxManager.SetMusic(false);
        SetImages(false);
        dispatcher.Dispatch(Events.SFXOnVolumeChanged);
    }

    private void SetImages(bool value)
    {
        musicOnEnabledImage.SetActive(value);
        musicOnDisabledImage.SetActive(!value);
        musicOffEnabledImage.SetActive(!value);
        musicOffDisabledImage.SetActive(value);
    }



}
