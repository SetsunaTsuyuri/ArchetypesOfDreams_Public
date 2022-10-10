﻿using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// オーディオデータ
    /// </summary>
    [System.Serializable]
    public class AudioData : Data
    {
        /// <summary>
        /// 名前
        /// </summary>
        [field: SerializeField]
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// AudioClip
        /// </summary>
        [field: SerializeField]
        public AudioClip Clip { get; private set; } = null;

        /// <summary>
        /// 音量
        /// </summary>
        [field: SerializeField, Range(0.0f, 1.0f)]
        public float Volume { get; private set; } = 1.0f;
    }
}
