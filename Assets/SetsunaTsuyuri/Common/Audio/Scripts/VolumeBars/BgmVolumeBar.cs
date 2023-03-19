using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// BGM音量バー
    /// </summary>
    public class BgmVolumeBar : VolumeBar
    {
        protected override System.IObservable<float> GetAudioVolumeObervable()
        {
            return AudioManager.BgmVolume.Observable;
        }
    }
}
