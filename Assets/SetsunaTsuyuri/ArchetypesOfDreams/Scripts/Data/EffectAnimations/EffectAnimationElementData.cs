using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 効果アニメーションの再生位置タイプ
    /// </summary>
    public enum EffectAnimationPositionType
    {
        None,

        /// <summary>
        /// 対象
        /// </summary>
        Targets = 1,

        /// <summary>
        /// 使用者
        /// </summary>
        User = 2,
    }

    /// <summary>
    /// 効果アニメーションの要素データ
    /// </summary>
    [System.Serializable]
    public class EffectAnimationElementData
    {
        /// <summary>
        /// タイミング
        /// </summary>
        public float Timing = 0.0f;

        /// <summary>
        /// エフェクトID
        /// </summary>
        public int EffectId = 0;

        /// <summary>
        /// 再生位置
        /// </summary>
        public EffectAnimationPositionType EffectPosition = EffectAnimationPositionType.None;

        /// <summary>
        /// SEID
        /// </summary>
        public int SEId = 0;

        /// <summary>
        /// SE音量
        /// </summary>
        public float SEVolume = 1.0f;
    }
}
