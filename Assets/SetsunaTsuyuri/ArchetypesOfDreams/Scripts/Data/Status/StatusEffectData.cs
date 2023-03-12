using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動への影響
    /// </summary>
    public enum EffectOnAction
    {
        None = 0,

        /// <summary>
        /// 行動不能
        /// </summary>
        InabiltyToAct = 1
    }

    /// <summary>
    /// 見た目への影響
    /// </summary>
    public enum EffectOnAppearance
    {
        /// <summary>
        /// 影響なし
        /// </summary>
        None = 0
    }

    /// <summary>
    /// 解除タイミング
    /// </summary>
    public enum RemovalTiming
    {
        None = 0,

        /// <summary>
        /// ターン開始時
        /// </summary>
        TurnStart = 1,

        /// <summary>
        /// ターン終了時
        /// </summary>
        TurnEnd = 2
    }
}

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ステータス効果カテゴリ
    /// </summary>
    public enum StatusEffectCategory
    {
        None = 0,

        /// <summary>
        /// 異常
        /// </summary>
        Abnormality = 1,

        /// <summary>
        /// 強化
        /// </summary>
        Buff = 2,

        /// <summary>
        /// 弱体
        /// </summary>
        Debuff = 3,

        /// <summary>
        /// 構え
        /// </summary>
        Stance = 4,
    }

    /// <summary>
    /// 重ね掛けされた場合の挙動
    /// </summary>
    public enum StatusEffectSatck
    {
        None = 0,

        /// <summary>
        /// 延長
        /// </summary>
        Prolong = 1,

        /// <summary>
        /// 上書き
        /// </summary>
        Overwrite = 2,

        /// <summary>
        /// 重複
        /// </summary>
        Stack = 3,

        /// <summary>
        /// 無効
        /// </summary>
        Invaild = 4
    }

    /// <summary>
    /// ステータス効果のデータ
    /// </summary>
    [System.Serializable]
    public class StatusEffectData : NameDescriptionData
    {
        /// <summary>
        /// アイコンの文字
        /// </summary>
        public string IconText = string.Empty;

        /// <summary>
        /// 効果の属性
        /// </summary>
        public StatusEffectCategory EffectAttribute = StatusEffectCategory.None;

        /// <summary>
        /// 行動への影響
        /// </summary>
        public EffectOnAction Action = EffectOnAction.None;

        /// <summary>
        /// 見た目の影響
        /// </summary>
        public EffectOnAppearance Appearance = EffectOnAppearance.None;

        /// <summary>
        /// 解除タイミング
        /// </summary>
        public RemovalTiming RemovalTiming = RemovalTiming.None;

        /// <summary>
        /// 戦闘不能として扱われる
        /// </summary>
        public bool IsTreatedAsKnockedOut = false;

        /// <summary>
        /// 重ね掛けされたときの挙動
        /// </summary>
        public StatusEffectSatck Stack = StatusEffectSatck.Prolong;

        /// <summary>
        /// 戦闘終了時に解除される
        /// </summary>
        public bool IsRemovedOnBattleEnd = true;

        /// <summary>
        /// 行動不能時に解除される
        /// </summary>
        public bool IsRemovedOnInabilityToAct = false;

        /// <summary>
        /// 付与・解除の際ポップアップ表示する
        /// </summary>
        public bool CanPopUp = true;

        /// <summary>
        /// 力依存の攻撃倍率
        /// </summary>
        public float PowerAttackRate = 1.0f;

        /// <summary>
        /// 技依存の攻撃倍率
        /// </summary>
        public float TechniqueAttackRate = 1.0f;

        /// <summary>
        /// 力依存の防御倍率
        /// </summary>
        public float PowerDefenseRate = 1.0f;

        /// <summary>
        /// 技依存の防御倍率
        /// </summary>
        public float TechniqueDefenseRate = 1.0f;

        /// <summary>
        /// HP増減率
        /// </summary>
        public int HPChangeRate = 0;

        /// <summary>
        /// DP増減値
        /// </summary>
        public int DPChangeValue = 0;

        /// <summary>
        /// GP増減値
        /// </summary>
        public int GPChangeValue = 0;

        /// <summary>
        /// 命中
        /// </summary>
        public int Accuracy = 0;

        /// <summary>
        /// 回避
        /// </summary>
        public int Evasion = 0;

        /// <summary>
        /// クリティカル命中
        /// </summary>
        public int CriticalAccuracy = 0;

        /// <summary>
        /// クリティカル回避
        /// </summary>
        public int CriticalEvasion = 0;

        /// <summary>
        /// 遅延時間
        /// </summary>
        public float DelayTime = 0.0f;
    }
}

