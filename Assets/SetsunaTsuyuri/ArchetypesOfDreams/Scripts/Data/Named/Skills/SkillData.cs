using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 基本スキルタイプ
    /// </summary>
    public enum BasicSkillType
    {
        /// <summary>
        /// 攻撃
        /// </summary>
        Attack = 0,

        /// <summary>
        /// 防御
        /// </summary>
        Defense = 1,

        /// <summary>
        /// 浄化
        /// </summary>
        Purification = 2,

        /// <summary>
        /// 交代
        /// </summary>
        Change = 3
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
        [field: SerializeField]
        public int Cost { get; private set; }

        /// <summary>
        /// 効果
        /// </summary>
        [field: SerializeField]
        public EffectData Effect { get; private set; }
    }
}
