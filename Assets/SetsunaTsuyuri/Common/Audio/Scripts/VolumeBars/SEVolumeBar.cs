using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// SE音量バー
    /// </summary>
    public class SEVolumeBar : VolumeBar
    {
        protected override System.IObservable<float> GetAudioVolumeObervable()
        {
            return AudioManager.SEVolume.Observable;
        }
    }
}
