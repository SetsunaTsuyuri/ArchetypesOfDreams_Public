using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 精神力データ
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
        public GameAttribute.Emotion Emotion { get; private set; }
    }
}
