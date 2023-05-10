﻿using System;
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
    public class EffectData : NameDescriptionData
    {
        /// <summary>
        /// ダメージ
        /// </summary>
        [Serializable]
        public class DamageEffect
        {
            /// <summary>
            /// 影響を与えるステータス
            /// </summary>
            public AffectedStatusType AffectedStatusType = AffectedStatusType.HP;

            /// <summary>
            /// ダメージの種類
            /// </summary>
            public DamageType DamageType = DamageType.Damage;

            /// <summary>
            /// 使用者の力
            /// </summary>
            public float Power = 0.0f;

            /// <summary>
            /// 使用者の技
            /// </summary>
            public float Technique = 0.0f;

            /// <summary>
            /// 対象の現在値
            /// </summary>
            public float Current = 0.0f;

            /// <summary>
            /// 対象の最大値
            /// </summary>
            public float Max = 0.0f;

            /// <summary>
            /// 固定値
            /// </summary>
            public int Fixed = 0;

            /// <summary>
            /// 乱数値
            /// </summary>
            public int Random = 0;

            /// <summary>
            /// ゼロにしない
            /// </summary>
            public bool DontKill = false;
        }

        /// <summary>
        /// ステータス効果
        /// </summary>
        [Serializable]
        public class StatusEffect
        {
            /// <summary>
            /// ステータス効果ID
            /// </summary>
            public int StatusEffectId = 0;

            /// <summary>
            /// 解除する
            /// </summary>
            public bool IsRemoval = false;

            /// <summary>
            /// 解除するカテゴリ
            /// </summary>
            public StatusEffectCategory RemovalCategory = StatusEffectCategory.None;

            /// <summary>
            /// 付与・解除確率
            /// </summary>
            public int Probability = 100;

            /// <summary>
            /// 有効ターン数
            /// </summary>
            public int Turns = 1;

            /// <summary>
            /// 永続する
            /// </summary>
            public bool IsPermanent = false;

            /// <summary>
            /// 抵抗を無視する
            /// </summary>
            public bool IgnoresRegistance = false;
        }

        /// <summary>
        /// どの立場の者の中から
        /// </summary>
        public TargetPosition TargetPosition = TargetPosition.None;

        /// <summary>
        /// どの状態の者を
        /// </summary>
        public TargetCondition TargetCondition = TargetCondition.None;

        /// <summary>
        /// どのように選ぶか
        /// </summary>
        public TargetSelectionType TargetSelection = TargetSelectionType.None;

        /// <summary>
        /// ダメージ効果
        /// </summary>
        public DamageEffect[] DamageEffects = { };

        /// <summary>
        /// ステータス効果
        /// </summary>
        public StatusEffect[] StatusEffects = { };

        /// <summary>
        /// アニメーションID
        /// </summary>
        public int AnimationId = 0;

        /// <summary>
        /// 追加実行回数
        /// </summary>
        public int ExtraExecutions = 1;

        /// <summary>
        /// 動作時間
        /// </summary>
        public float ActionTime = 1.0f;

        /// <summary>
        /// 浄化スキルである
        /// </summary>
        public bool IsPurification = false;

        /// <summary>
        /// 交代スキルである
        /// </summary>
        public bool IsChange = false;

        /// <summary>
        /// 効果適用後、再び行動できる
        /// </summary>
        public bool CanActAgain = false;

        /// <summary>
        /// 攻撃的な効果である
        /// </summary>
        public bool IsOffensive = false;

        /// <summary>
        /// 感情の属性
        /// </summary>
        public GameAttribute.Emotion Emotion = GameAttribute.Emotion.None;

        /// <summary>
        /// 命中率
        /// </summary>
        public int Hit = 100;

        /// <summary>
        /// クリティカル率
        /// </summary>
        public int Critical = 0;

        /// <summary>
        /// ダメージ効果がある
        /// </summary>
        /// <returns></returns>
        public bool HasDamageEffects => DamageEffects.Length > 0;
    }
}