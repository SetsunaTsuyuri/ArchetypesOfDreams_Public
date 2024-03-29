﻿using System.Collections.Generic;
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
        Change = 4,

        /// <summary>
        /// 逃走
        /// </summary>
        Escape = 5,

        /// <summary>
        /// 何もしない
        /// </summary>
        NoAction = 6,
    }

    /// <summary>
    /// 技のデータ
    /// </summary>
    /// <typeparam name="T">行動内容</typeparam>
    [System.Serializable]
    public class SkillData : EffectData
    {
        /// <summary>
        /// 消費DP
        /// </summary>
        public int Cost = 0;
    }
}
