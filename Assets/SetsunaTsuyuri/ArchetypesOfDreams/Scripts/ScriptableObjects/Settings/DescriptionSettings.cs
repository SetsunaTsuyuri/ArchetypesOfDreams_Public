using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 説明文の設定
    /// </summary>
    [CreateAssetMenu(fileName = "Description", menuName = "Settings/Description")]
    public class DescriptionSettings : ScriptableObject
    {
        /// <summary>
        /// スキルボタン
        /// </summary>
        [field: SerializeField]
        public string Skills { get; private set; } = "スキルを使用します";

        /// <summary>
        /// アイテムボタン
        /// </summary>
        [field: SerializeField]
        public string Items { get; private set; } = "アイテムを使用します";
    }
}
