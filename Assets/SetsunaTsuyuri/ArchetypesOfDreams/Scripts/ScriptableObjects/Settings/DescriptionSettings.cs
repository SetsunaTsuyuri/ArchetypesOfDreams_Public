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
        public string Items { get; private set; } = "アイテムを使用します";
    }
}
