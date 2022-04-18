using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 状態異常の有効性データ
    /// </summary>
    [System.Serializable]
    public class EffectivenessDataOfStatusAbnormalities
    {
        /// <summary>
        /// 毒系の有効性
        /// </summary>
        [field: SerializeField]
        public Attribute.Effectiveness Poison { get; private set; }

        /// <summary>
        /// 封印系の有効性
        /// </summary>
        [field: SerializeField]
        public Attribute.Effectiveness Seal { get; private set; }

        /// <summary>
        /// 睡眠系の有効性
        /// </summary>
        [field: SerializeField]
        public Attribute.Effectiveness Sleep { get; private set; }

        /// <summary>
        /// 混乱系の有効性
        /// </summary>
        [field: SerializeField]
        public Attribute.Effectiveness Confusion { get; private set; }
    }
}
