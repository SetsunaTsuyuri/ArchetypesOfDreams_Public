using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 説明文の設定
    /// </summary>
    [CreateAssetMenu(menuName = "Settings/Description")]
    public class DescriptionSettings : ScriptableObject
    {
        /// <summary>
        /// 近接武器スキルボタン
        /// </summary>
        [field: SerializeField]
        public string MeleeWeaponSkills { get; private set; } = "近接武器スキルを使用します";

        /// <summary>
        /// 遠隔武器スキルボタン
        /// </summary>
        [field: SerializeField]
        public string RangedWeaponSkills { get; private set; } = "遠隔武器スキルを使用します";

        /// <summary>
        /// 特殊スキルボタン
        /// </summary>
        [field: SerializeField]
        public string SpecialSkills { get; private set; } = "特殊スキルを使用します";

        /// <summary>
        /// 浄化ボタン
        /// </summary>
        [field: SerializeField]
        public string Purification { get; private set; } = "敵の浄化を試みます";

        /// <summary>
        /// 交代ボタン
        /// </summary>
        [field: SerializeField]
        public string Change { get; private set; } = "控えの味方と交代します";

        /// <summary>
        /// 防御ボタン
        /// </summary>
        [field: SerializeField]
        public string Guard { get; private set; } = "身を守ります";

        /// <summary>
        /// アイテムボタン
        /// </summary>
        [field: SerializeField]
        public string Item { get; private set; } = "アイテムを使います";
    }
}
