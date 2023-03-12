using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 用語の設定
    /// </summary>
    [CreateAssetMenu(fileName = "Terms", menuName = "Settings/Terms")]
    public class TermsSettings : ScriptableObject
    {
        /// <summary>
        /// レベル
        /// </summary>
        [field: SerializeField]
        public string Level { get; private set; } = "Lv.";

        /// <summary>
        /// 感情属性アイコンの名前
        /// </summary>
        [field: SerializeField]
        public KeysAndValues<GameAttribute.Emotion, string> EmotionIconNames { get; private set; }
    }
}
