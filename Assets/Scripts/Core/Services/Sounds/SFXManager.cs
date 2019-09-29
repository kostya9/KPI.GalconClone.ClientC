using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager
{
    private string MUSIC_KEY = "music";

    public float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetInt(MUSIC_KEY, 1);
        }

        private set
        {
            PlayerPrefs.SetInt(MUSIC_KEY, Mathf.RoundToInt(Mathf.Clamp01(value)));
        }
    }

    public void SetMusic(bool value)
    {
        MusicVolume = value ? 1f : 0f;
    }

}
