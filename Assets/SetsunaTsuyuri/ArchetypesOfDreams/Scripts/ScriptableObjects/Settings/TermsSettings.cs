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
        /// 戦闘不能の対象
        /// </summary>
        [field: SerializeField]
        public string TargetKnockedOut { get; private set; } = "戦闘不能の";

        /// <summary>
        /// 敵
        /// </summary>
        [field: SerializeField]
        public string TargetEnemies { get; private set; } = "敵";

        /// <summary>
        /// 味方
        /// </summary>
        [field: SerializeField]
        public string TargetAllies { get; private set; } = "味方";

        /// <summary>
        /// 敵味方両方
        /// </summary>
        [field: SerializeField]
        public string TargetBoth { get; private set; } = "敵味方";

        /// <summary>
        /// 自分自身
        /// </summary>
        [field: SerializeField]
        public string TargetOneself { get; private set; } = "自分自身";

        /// <summary>
        /// 自分以外の味方
        /// </summary>
        [field: SerializeField]
        public string TargetAlliesOtherThanOneself { get; private set; } = "自分以外の味方";

        /// <summary>
        /// 控えの味方
        /// </summary>
        [field: SerializeField]
        public string TargetReserves { get; private set; } = "控えの味方";

        /// <summary>
        /// 単体
        /// </summary>
        [field: SerializeField]
        public string TargetSingle { get; private set; } = "単体";

        /// <summary>
        /// 全体
        /// </summary>
        [field: SerializeField]
        public string TargetAll { get; private set; } = "全体";

        /// <summary>
        /// ランダム
        /// </summary>
        [field: SerializeField]
        public string TargetRandom { get; private set; } = "の中からランダム";

        /// <summary>
        /// 感情属性アイコンの名前
        /// </summary>
        [field: SerializeField]
        public KeysAndValues<GameAttribute.Emotion, string> EmotionIconNames { get; private set; }
    }
}
