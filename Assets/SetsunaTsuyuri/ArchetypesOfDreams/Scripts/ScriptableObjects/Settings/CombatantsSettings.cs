﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者の設定
    /// </summary>
    [CreateAssetMenu(fileName = "Combatants", menuName = "Settings/Combatants")]
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
        public int MaxLevel { get; private set; } = 99;

        /// <summary>
        /// レベルアップに必要な経験値
        /// </summary>
        [field: SerializeField]
        public int ExperienceRequiredToLevelUp { get; private set; } = 120;

        /// <summary>
        /// 1レベル毎のレベルアップに必要な経験値の増加率
        /// </summary>
        [field: SerializeField]
        public float PercentageIncreaceInExperienceRequiredToLevelUp { get; private set; } = 1.0f;

        /// <summary>
        /// 1レベル毎のステータス増加量
        /// </summary>
        [field: SerializeField]
        public float AmountOfIncreaseInStatusPerLevel { get; private set; } = 0.1f;

        /// <summary>
        /// 最大HP
        /// </summary>
        [field: SerializeField]
        public int MaxHP { get; private set; } = 99999;

        /// <summary>
        /// 最大DP
        /// </summary>
        [field: SerializeField]
        public int MaxDP { get; private set; } = 100;

        /// <summary>
        /// 最大GP
        /// </summary>
        [field: SerializeField]
        public int MaxGP { get; private set; } = 100;

        /// <summary>
        /// 最大待機時間
        /// </summary>
        [field: SerializeField]
        public int MaxWaitTime { get; private set; } = 10000;

        /// <summary>
        /// DPが増える間隔
        /// </summary>
        [field: SerializeField]
        public int DPRegainingInterval { get; private set; } = 20;

        /// <summary>
        /// クリティカルヒットしたときのダメージ倍率
        /// </summary>
        [field: SerializeField]
        public float CriticalDamageScale { get; private set; } = 1.5f;

        /// <summary>
        /// クリティカルヒットしたときのGPダメージ
        /// </summary>
        [field: SerializeField]
        public int CriticalGPDamage { get; private set; } = 1;

        /// <summary>
        /// 最大ダメージ・回復量
        /// </summary>
        [field: SerializeField]
        public int MaxDamageAndHealing { get; private set; } = 99999999;

        /// <summary>
        /// HPが0になったときに付与されるステータス効果
        /// </summary>
        [field: SerializeField]
        public EffectData.StatusEffect EffectHP0 { get; private set; } = new();

        /// <summary>
        /// GPが0になったとき付与されるステータス効果
        /// </summary>
        [field: SerializeField]
        public EffectData.StatusEffect EffectGP0 { get; private set; } = new();

        /// <summary>
        /// 勝利後のHP回復率
        /// </summary>
        [field: SerializeField]
        public float HPHealingRateOnWin { get; private set; } = 0.25f;

        /// <summary>
        /// ダメージゲージ表示待機時間
        /// </summary>
        [field: SerializeField]
        public float DamageGaugeWaitDuration { get; private set; } = 0.25f;

        /// <summary>
        /// ダメージゲージ表示時間
        /// </summary>
        [field: SerializeField]
        public float DamageGaugeDuration { get; private set; } = 0.5f;
    }
}
