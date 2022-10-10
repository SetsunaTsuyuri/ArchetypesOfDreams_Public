using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 対象に対する行動の結果
    /// </summary>
    public class ActionResult : IInitializable
    {
        /// <summary>
        /// 影響を与えてきた者
        /// </summary>
        public Combatant Affecter { get; set; } = null;

        /// <summary>
        /// 感情属性の有効性
        /// </summary>
        public Attribute.Effectiveness Effectiveness { get; set; } = Attribute.Effectiveness.Normal;

        /// <summary>
        /// 浄化成功率
        /// </summary>
        public int PurificationSuccessRate { get; set; } = 0;

        /// <summary>
        /// 回避された
        /// </summary>
        public bool Miss { get; set; } = false;

        /// <summary>
        /// クリティカルヒットした
        /// </summary>
        public bool Critical { get; set; } = false;

        /// <summary>
        /// HPダメージ
        /// </summary>
        public int? HpDamage { get; set; } = null;

        /// <summary>
        /// DPダメージ
        /// </summary>
        public int? DpDamage { get; set; } = null;

        /// <summary>
        /// SPダメージ
        /// </summary>
        public int? SpDamage { get; set; } = null;

        /// <summary>
        /// HP回復
        /// </summary>
        public int? HpRecovery { get; set; } = null;

        /// <summary>
        /// DP回復
        /// </summary>
        public int? DpRecovery { get; set; } = null;

        /// <summary>
        /// SP回復
        /// </summary>
        public int? SpRecovery { get; set; } = null;

        /// <summary>
        /// 追加ステータス効果リスト
        /// </summary>
        public readonly List<EffectData.StatusEffect> AddedStatusEffects = new();

        /// <summary>
        /// 削除ステータス効果リスト
        /// </summary>
        public readonly List<StatusEffect> RemovedStatusEffects = new();

        /// <summary>
        /// クラッシュした
        /// </summary>
        public bool Crushed { get; set; } = false;

        /// <summary>
        /// 浄化された
        /// </summary>
        public bool? Purified { get; set; } = false;

        /// <summary>
        /// 交代の対象にされた
        /// </summary>
        public bool IsChangeTarget { get; set; }

        public void Initialize()
        {
            Affecter = null;

            Effectiveness = Attribute.Effectiveness.Normal;
            PurificationSuccessRate = 0;

            Miss = false;

            Critical = false;

            HpDamage = null;
            DpDamage = null;
            SpDamage = null;

            HpRecovery = null;
            DpRecovery = null;
            SpRecovery = null;

            Purified = null;

            AddedStatusEffects.Clear();
            RemovedStatusEffects.Clear();

            Crushed = false;
            IsChangeTarget = false;
        }

        /// <summary>
        /// ダメージがある
        /// </summary>
        /// <returns></returns>
        public bool IsDamage()
        {
            return HpDamage.HasValue || DpDamage.HasValue || SpDamage.HasValue;
        }

        /// <summary>
        /// 1以上のダメージがある
        /// </summary>
        /// <returns></returns>
        public bool IsOneOrOverDamage()
        {
            return HpDamage > 0 || DpDamage > 0 || SpDamage > 0;
        }

        /// <summary>
        /// 回復がある
        /// </summary>
        /// <returns></returns>
        public bool IsRecovery()
        {
            return HpRecovery.HasValue || DpRecovery.HasValue || SpRecovery.HasValue;
        }

        /// <summary>
        /// 1以上の回復がある
        /// </summary>
        /// <returns></returns>
        public bool IsOneOrOverRecovery()
        {
            return HpRecovery > 0 || DpRecovery > 0 || SpRecovery > 0;
        }

        /// <summary>
        /// HPダメージを増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddHpDamage(int value)
        {
            if (HpDamage.HasValue)
            {
                HpDamage += value;
            }
            else
            {
                HpDamage = value;
            }
        }

        /// <summary>
        /// HP回復を増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddHpRecovery(int value)
        {
            if (HpRecovery.HasValue)
            {
                HpRecovery += value;
            }
            else
            {
                HpRecovery = value;
            }
        }

        /// <summary>
        /// DPダメージを増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddDpDamage(int value)
        {
            if (DpDamage.HasValue)
            {
                DpDamage += value;
            }
            else
            {
                DpDamage = value;
            }
        }

        /// <summary>
        /// DP回復を増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddDpRecovery(int value)
        {
            if (DpRecovery.HasValue)
            {
                DpRecovery += value;
            }
            else
            {
                DpRecovery = value;
            }
        }

        /// <summary>
        /// SPダメージを増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddSpDamage(int value)
        {
            if (SpDamage.HasValue)
            {
                SpDamage += value;
            }
            else
            {
                SpDamage = value;
            }
        }

        /// <summary>
        /// SP回復を増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddSpRecovery(int value)
        {
            if (SpRecovery.HasValue)
            {
                SpRecovery += value;
            }
            else
            {
                SpRecovery = value;
            }
        }
    }
}
