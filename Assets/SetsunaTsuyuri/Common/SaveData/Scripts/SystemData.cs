using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// システムデータ
    /// </summary>
    public class SystemData
    {
        /// <summary>
        /// マスター音量
        /// </summary>
        [field: SerializeField]
        public float MasterVolume { get; set; } = 1.0f;

        /// <summary>
        /// BGM音量
        /// </summary>
        [field: SerializeField]
        public float BgmVolume { get; set; } = 1.0f;

        /// <summary>
        /// SE音量
        /// </summary>
        [field: SerializeField]
        public float SEVolume { get; set; } = 1.0f;

        /// <summary>
        /// セーブする
        /// </summary>
        public void Save()
        {
            MasterVolume = AudioManager.MasterVolume.Value;
            BgmVolume = AudioManager.BgmVolume.Value;
            SEVolume = AudioManager.SEVolume.Value;
        }

        /// <summary>
        /// ロードする
        /// </summary>
        public void Load()
        {
            AudioManager.MasterVolume.Value = MasterVolume;
            AudioManager.BgmVolume.Value = BgmVolume;
            AudioManager.SEVolume.Value = SEVolume;
        }
    }
}
