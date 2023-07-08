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

        /// <summary>
        /// 力依存の被ダメージ倍率
        /// </summary>
        PowerDamageScale = 11,

        /// <summary>
        /// 技依存の被ダメージ倍率
        /// </summary>
        TechniqueDamageScale = 12,

        /// <summary>
        /// ステータス効果耐性
        /// </summary>
        StatusEffectResistance = 21,
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
        public int ParameterA = 0;

        /// <summary>
        /// パラメータB
        /// </summary>
        public int ParameterB = 0;

        /// <summary>
        /// パラメータC
        /// </summary>
        public int ParameterC = 0;
    }
}
