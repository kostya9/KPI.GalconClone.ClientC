using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Events {

    None,
    //ui

    UIMainScreenLoad,
    UIMainScreenRemove,
    UISettingsLoad,
    UISettingsRemove,

    //uiend

    SFXOnVolumeChanged,
    BGMusicLoad,
    GameLoad,
    GameRemove,
    AppBack,

    TouchEventSystemLoad,
    SwipeEnd,
    Swipe,
    Tap,

    GenerateLevel,


    End
}
