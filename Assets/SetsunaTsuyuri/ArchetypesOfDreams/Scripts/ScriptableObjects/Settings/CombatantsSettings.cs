using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者の設定
    /// </summary>
    [CreateAssetMenu(fileName ="Combatants", menuName = "Settings/Combatants")]
    public class CombatantsSettings : ScriptableObject
    {
        /// <summary>
        /// 最小レベル
        /// </summary>
        [field: SerializeField]
        public int MinLevel { get; private set; } = 1;

        /// <summary>
        /// 最大レベル
        /// </summary>
        [field: SerializeField]
        public int MaxLevel { get; private set; } = 999;

        /// <summary>
        /// 1レベル毎のステータス増加量
        /// </summary>
        [field: SerializeField]
        public float AmountOfIncreaseInStatusPerLevel { get; private set; } = 0.05f;

        /// <summary>
        /// 最大生命力
        /// </summary>
        [field: SerializeField]
        public int MaxLife { get; private set; } = 99999;

        /// <summary>
        /// 最大夢想力
        /// </summary>
        [field: SerializeField]
        public int MaxDream { get; private set; } = 100;

        /// <summary>
        /// 最大精神力
        /// </summary>
        [field: SerializeField]
        public int MaxSoul { get; private set; } = 999;

        /// <summary>
        /// 1ターンごとの夢想力増加量
        /// </summary>
        [field: SerializeField]
        public int AmoutOfIncreaseInDreamPerTurn { get; private set; } = 10;

        /// <summary>
        /// 1ターン毎の精神力回復量
        /// </summary>
        [field: SerializeField]
        public int AmoutOfSoulRecoverdPerTurn { get; private set; } = 1;

        /// <summary>
        /// 素早さのランダム補正
        /// </summary>
        [field: SerializeField]
        public float RandomSpeedCorrection { get; private set; } = 0.1f;

        /// <summary>
        /// 防御状態のときの被ダメージ倍率
        /// </summary>
        [field: SerializeField]
        public float DefendingDamageCorrection { get; private set; } = 0.5f;

        /// <summary>
        /// クラッシュ状態のときの与ダメージ・回復倍率
        /// </summary>
        [field: SerializeField]
        public float TakingDamageAndRecoveryCorrectionWhenCrush { get; private set; } = 0.25f;

        /// <summary>
        /// クラッシュ状態のときの被ダメージ倍率
        /// </summary>
        [field: SerializeField]
        public float GivingDamageCorrectionWhenCrush { get; private set; } = 1.5f;

        /// <summary>
        /// クリティカルヒットしたときのダメージ倍率
        /// </summary>
        [field: SerializeField]
        public float CriticalDamageCorrection { get; private set; } = 1.5f;

        /// <summary>
        /// クリティカルヒットしたときの精神力減少値
        /// </summary>
        [field: SerializeField]
        public int CriticalSoulDamage { get; private set; } = 1;

        /// <summary>
        /// 最大ダメージ・回復量
        /// </summary>
        [field: SerializeField]
        public int MaxDamageAndRecovery { get; private set; } = 99999999;

        /// <summary>
        /// クラッシュ状態が継続するターン数
        /// </summary>
        [field: SerializeField]
        public int CrushTurns { get; private set; } = 2;

        /// <summary>
        /// 敵UIの表示位置オフセット
        /// </summary>
        [field: SerializeField]
        public Vector2 EnemyUIPositionOffset { get; private set; } = new Vector2(0.0f, 1.0f);
    }
}
