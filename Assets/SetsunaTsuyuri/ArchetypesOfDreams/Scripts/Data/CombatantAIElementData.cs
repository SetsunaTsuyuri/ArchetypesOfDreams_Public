using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// AI条件タイプ
    /// </summary>
    public enum AIConditionType
    {
        None = 0,

        /// <summary>
        /// 無条件
        /// </summary>
        Unconditional = 1,

        /// <summary>
        /// ターン
        /// </summary>
        Turn = 2,
    }

    /// <summary>
    /// AI対象タイプ
    /// </summary>
    public enum AITargetType
    {
        None = 0,

        /// <summary>
        /// ランダム
        /// </summary>
        Random = 1,
    }

    /// <summary>
    /// 戦闘者AI要素データ
    /// </summary>
    [System.Serializable]
    public class CombatantAIElementData
    {
        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority = 0;

        /// <summary>
        /// 条件タイプ
        /// </summary>
        public AIConditionType Condition = AIConditionType.None;

        /// <summary>
        /// 条件パラメータA
        /// </summary>
        public int ConditionParameterA = 0;

        /// <summary>
        /// 条件パラメータB
        /// </summary>
        public int ConditionParameterB = 0;

        /// <summary>
        /// 対象タイプ
        /// </summary>
        public AITargetType Target = AITargetType.None;

        /// <summary>
        /// スキルID
        /// </summary>
        public int SkillId = 0;
    }
}
