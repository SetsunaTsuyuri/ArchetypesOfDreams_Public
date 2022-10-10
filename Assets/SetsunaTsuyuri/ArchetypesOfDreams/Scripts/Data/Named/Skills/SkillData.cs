using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 技のデータ
    /// </summary>
    /// <typeparam name="T">行動内容</typeparam>
    [System.Serializable]
    public class SkillData : DataWithIdAndName
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
