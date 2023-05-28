using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 待機処理の設定
    /// </summary>
    [CreateAssetMenu(fileName = "WaitTime", menuName = "Settings/WaitTime")]
    public class WaitTimeSettings : ScriptableObject
    {
        /// <summary>
        /// ミス
        /// </summary>
        [field: SerializeField]
        public float Miss { get; set; } = 0.3f;

        /// <summary>
        /// ダメージ
        /// </summary>
        [field: SerializeField]
        public float Damage { get; set; } = 0.3f;

        /// <summary>
        /// 回復
        /// </summary>
        [field: SerializeField]
        public float Healing { get; set; } = 0.3f;

        /// <summary>
        /// ステータス効果付与
        /// </summary>
        [field: SerializeField]
        public float StatusEffectAdded { get; set; } = 0.2f;

        /// <summary>
        /// 行動実行直後
        /// </summary>
        [field: SerializeField]
        public float ActionExecuted { get; private set; } = 0.1f;

        /// <summary>
        /// 戦闘終了
        /// </summary>
        [field: SerializeField]
        public float BattleEnd { get; private set; } = 0.3f;
    }
}
