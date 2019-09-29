using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXSoundVolumeBehaviour : BaseView
{

    [Inject] public SFXManager sfxManager { get; set; }

    private AudioSource audioSource;


    public void LoadView()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = sfxManager.MusicVolume;
        dispatcher.AddListener(Events.SFXOnVolumeChanged, OnVolumeChanged);
    }

    public void RemoveView()
    {
        dispatcher.RemoveListener(Events.SFXOnVolumeChanged, OnVolumeChanged);
    }


    private void OnVolumeChanged()
    {
        audioSource.volume = sfxManager.MusicVolume;
    }
}
