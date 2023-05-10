using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 効果アニメーションデータ
    /// </summary>
    [System.Serializable]
    public class EffectAnimationData : NameData
    {
        /// <summary>
        /// 時間
        /// </summary>
        public float Duration = 0.0f;

        /// <summary>
        /// 要素配列
        /// </summary>
        public EffectAnimationElementData[] Elements = { };
    }
}
