using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ナイトメアのデータ
    /// </summary>
    [System.Serializable]
    public class NightmareData : CombatantData
    {
        /// <summary>
        /// 近接武器属性
        /// </summary>
        [field: SerializeField]
        public Attribute.MeleeWeapon MeleeWeapon { get; private set; }

        /// <summary>
        /// 遠隔武器属性
        /// </summary>
        [field: SerializeField]
        public Attribute.RangedWeapon RangedWeapon { get; private set; }

        /// <summary>
        /// 浄化成功率
        /// </summary>
        [field: SerializeField]
        public int PurificationSuccessRate { get; private set; }

        /// <summary>
        /// 経験値
        /// </summary>
        [field: SerializeField]
        public int Experience { get; private set; }

        /// <summary>
        /// 共感力
        /// </summary>
        [field: SerializeField]
        public int Empathy { get; private set; }
    }
}
