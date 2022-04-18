using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 汎用スキルのデータ集
    /// </summary>
    [CreateAssetMenu(menuName = "Data/CommonSkills")]
    public class CommonSkillDataCollection : ScriptableObject
    {
        /// <summary>
        /// 浄化
        /// </summary>
        [field: SerializeField]
        public SkillData Purification { get; private set; }

        /// <summary>
        /// 交代
        /// </summary>
        [field: SerializeField]
        public SkillData Change { get; private set; }

        /// <summary>
        /// 防御
        /// </summary>
        [field: SerializeField]
        public SkillData Defending { get; private set; }
    }
}
