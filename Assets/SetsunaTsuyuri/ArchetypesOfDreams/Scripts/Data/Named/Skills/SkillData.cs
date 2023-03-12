using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 基本スキルID
    /// </summary>
    public enum BasicSkillId
    {
        /// <summary>
        /// 攻撃
        /// </summary>
        Attack = 1,

        /// <summary>
        /// 防御
        /// </summary>
        Defense = 2,

        /// <summary>
        /// 浄化
        /// </summary>
        Purification = 3,

        /// <summary>
        /// 交代
        /// </summary>
        Change = 4
    }

    /// <summary>
    /// 技のデータ
    /// </summary>
    /// <typeparam name="T">行動内容</typeparam>
    [System.Serializable]
    public class SkillData : NameDescriptionData
    {
        /// <summary>
        /// 消費DP
        /// </summary>
        public int Cost = 0;

        /// <summary>
        /// 効果
        /// </summary>
        [field: SerializeField]
        public EffectData Effect { get; private set; }
    }
}
