using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 音量
    /// </summary>
    public class AudioVolume : ReactiveRangeValue<float>
    {
        public AudioVolume() : base(1.0f, 0.0f, 1.0f) { }
    }
}
