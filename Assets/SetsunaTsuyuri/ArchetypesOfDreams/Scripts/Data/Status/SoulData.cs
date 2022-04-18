using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 精神データ
    /// </summary>
    [System.Serializable]
    public class SoulData
    {
        /// <summary>
        /// 最大値
        /// </summary>
        [field: SerializeField]
        public int Max { get; private set; }

        /// <summary>
        /// 感情属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Emotion Emotion { get; private set; }

        /// <summary>
        /// 状態異常に対する有効性
        /// </summary>
        [field: SerializeField]
        public EffectivenessDataOfStatusAbnormalities Effectiveness { get; private set; }
    }
}
