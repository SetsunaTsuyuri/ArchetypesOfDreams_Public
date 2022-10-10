using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// エフェクトの再生位置
    /// </summary>
    public enum EffectPlayPositionType
    {
        Targrt = 0,
        Self = 1,
        Screen = 2
    }

    /// <summary>
    /// 効果のデータ
    /// </summary>
    [Serializable]
    public class EffectData
    {
        /// <summary>
        /// ダメージ
        /// </summary>
        [Serializable]
        public class Damage
        {
            /// <summary>
            /// 影響を受けるステータス
            /// </summary>
            [field: SerializeField]
            public Affected Affected { get; private set; } = Affected.Hp;

            /// <summary>
            /// ダメージの種類
            /// </summary>
            [field: SerializeField]
            public DamageType DamageType { get; private set; } = DamageType.Damage;

            /// <summary>
            /// 計算式の種類
            /// </summary>
            [field: SerializeField]
            public Formula Formula { get; private set; } = Formula.Status;

            /// <summary>
            /// 値
            /// </summary>
            [field: SerializeField]
            public float Parameter { get; private set; } = 1.0f;

            /// <summary>
            /// 固定値
            /// </summary>
            [field: SerializeField]
            public int Fixed { get; private set; } = 0;

            /// <summary>
            /// 乱数値
            /// </summary>
            [field: SerializeField]
            public int Random { get; private set; } = 0;

            /// <summary>
            /// ゼロにしない
            /// </summary>
            [field: SerializeField]
            public bool DontToZero { get; private set; } = false;

            /// <summary>
            /// ダメージを与える効果である
            /// </summary>
            /// <returns></returns>
            public bool IsDamage()
            {
                return DamageType == DamageType.Damage;
            }

            /// <summary>
            /// 回復させる効果である
            /// </summary>
            /// <returns></returns>
            public bool IsRecovery()
            {
                return DamageType == DamageType.Recovery;
            }
        }

        /// <summary>
        /// ステータス効果
        /// </summary>
        [Serializable]
        public class StatusEffect
        {
            /// <summary>
            /// ID
            /// </summary>
            [field: SerializeField]
            public int Id { get; private set; } = 0;

            /// <summary>
            /// 有効ターン数
            /// </summary>
            [field: SerializeField]
            public int Turns { get; private set; } = 1;

            /// <summary>
            /// 付与確率
            /// </summary>
            [field: SerializeField]
            public int Rate { get; private set; } = 100;

            /// <summary>
            /// 抵抗を無視する
            /// </summary>
            [field: SerializeField]
            public bool IgnoreRegistance { get; private set; } = false;
        }

        /// <summary>
        /// エフェクトとSE
        /// </summary>
        [Serializable]
        public class EffectAndSE
        {
            /// <summary>
            /// 再生するタイミング
            /// </summary>
            public float Timing = 0.0f;

            /// <summary>
            /// エフェクト再生位置
            /// </summary>
            public EffectPlayPositionType EffectPlayPosition = EffectPlayPositionType.Targrt;

            /// <summary>
            /// エフェクトタイプ
            /// </summary>
            public EffectType Effect = default;

            /// <summary>
            /// SEタイプ
            /// </summary>
            public SEType SE = default;
        }

        /// <summary>
        /// どの立場の者の中から
        /// </summary>
        [field: SerializeField]
        public TargetPosition TargetPosition { get; private set; } = TargetPosition.Enemies;

        /// <summary>
        /// どの状態の者を
        /// </summary>
        [field: SerializeField]
        public TargetCondition TargetCondition { get; private set; } = TargetCondition.Living;

        /// <summary>
        /// どのように選ぶか
        /// </summary>
        [field: SerializeField]
        public TargetSelectionType TargetSelection { get; private set; } = TargetSelectionType.Single;

        /// <summary>
        /// ダメージと回復
        /// </summary>
        [field: SerializeField]
        public Damage[] Damages { get; private set; } = { };

        /// <summary>
        /// ステータス効果
        /// </summary>
        [field: SerializeField]
        public StatusEffect[] StatusEffects { get; private set; } = { };

        /// <summary>
        /// エフェクト待機時間
        /// </summary>
        public float EffectWaitDuration = 0.0f;

        /// <summary>
        /// エフェクトとSE
        /// </summary>
        public EffectAndSE[] EffectsAndSEs = { };

        /// <summary>
        /// 実行回数
        /// </summary>
        [field: SerializeField]
        public int Executions { get; private set; } = 1;

        /// <summary>
        /// 動作時間
        /// </summary>
        [field: SerializeField]
        public float ActionTime { get; private set; } = 1.0f;

        /// <summary>
        /// 浄化スキルである
        /// </summary>
        [field: SerializeField]
        public bool IsPurification { get; private set; } = false;

        /// <summary>
        /// 交代スキルである
        /// </summary>
        [field: SerializeField]
        public bool IsChange { get; private set; } = false;

        /// <summary>
        /// 攻撃的なスキルである
        /// </summary>
        [field: SerializeField]
        public bool IsOffensive { get; private set; } = false;

        /// <summary>
        /// 攻撃の属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Attack Attack { get; private set; } = Attribute.Attack.None;

        /// <summary>
        /// 感情の属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Emotion Emotion { get; private set; } = Attribute.Emotion.None;

        /// <summary>
        /// 弱点を突いた場合に与える追加精神ダメージ
        /// </summary>
        [field: SerializeField]
        public int SoulDamage { get; private set; } = 1;

        /// <summary>
        /// 命中率
        /// </summary>
        [field: SerializeField]
        public int Hit { get; private set; } = 100;

        /// <summary>
        /// クリティカル率
        /// </summary>
        [field: SerializeField]
        public int Critical { get; private set; } = 0;

        /// <summary>
        /// ステータスに影響を与える
        /// </summary>
        /// <returns></returns>
        public bool AffectStatus()
        {
            return Damages.Length > 0;
        }

        /// <summary>
        /// ステータス効果を付与する
        /// </summary>
        /// <returns></returns>
        public bool AddsStatusEffects()
        {
            return StatusEffects.Length > 0;
        }
    }
}
