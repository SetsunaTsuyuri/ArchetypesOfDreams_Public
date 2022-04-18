using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 感情属性の有効性データ
    /// </summary>
    [System.Serializable]
    public class EffectivenessDataOfEmotions
    {
        /// <summary>
        /// 攻撃側の属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Emotion Attack { get; private set; }

        /// <summary>
        /// 守備側の属性毎の有効性
        /// </summary>
        [field: SerializeField]
        public KeyAndValue<Attribute.Emotion, Attribute.Effectiveness>[] Effectiveness { get; private set; }
    }
}
