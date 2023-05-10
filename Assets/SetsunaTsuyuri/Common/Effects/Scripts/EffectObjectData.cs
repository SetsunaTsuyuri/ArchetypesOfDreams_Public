using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// エフェクトオブジェクトデータ
    /// </summary>
    [System.Serializable]
    public class EffectObjectData : IdData
    {
        /// <summary>
        /// パーティクルシステム
        /// </summary>
        [field: SerializeField]
        public ParticleSystem ParticleSystem { get; private set; } = null;

        /// <summary>
        /// 最初にプールする数
        /// </summary>
        [field: SerializeField]
        public int PoolCaptity { get; private set; } = 10;
    }
}
