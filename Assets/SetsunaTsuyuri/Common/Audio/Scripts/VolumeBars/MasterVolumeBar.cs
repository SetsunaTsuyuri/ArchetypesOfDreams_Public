using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// マスター音量バー
    /// </summary>
    public class MasterVolumeBar : VolumeBar
    {
        protected override System.IObservable<float> GetAudioVolumeObervable()
        {
            return AudioManager.MasterVolume.Observable;
        }
    }
}
