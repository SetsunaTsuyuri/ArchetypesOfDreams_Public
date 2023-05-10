using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 能力タイプ
    /// </summary>
    public enum AbilityType
    {
        None = 0,

        /// <summary>
        /// 致死ダメージをHP1で耐える
        /// </summary>
        Survivor = 1,

        /// <summary>
        /// スキル封印
        /// </summary>
        SkillSealing = 2,
    }

    /// <summary>
    /// 能力データ
    /// </summary>
    [System.Serializable]
    public class AbilityData
    {
        /// <summary>
        /// タイプ
        /// </summary>
        public AbilityType Type = AbilityType.None;

        /// <summary>
        /// パラメータA
        /// </summary>
        public float ParameterA = 0.0f;

        /// <summary>
        /// パラメータB
        /// </summary>
        public float ParameterB = 0.0f;

        /// <summary>
        /// パラメータC
        /// </summary>
        public float ParameterC = 0.0f;
    }
}
