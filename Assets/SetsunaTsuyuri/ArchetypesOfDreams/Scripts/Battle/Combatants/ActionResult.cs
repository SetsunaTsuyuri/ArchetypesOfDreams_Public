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
        public int? HPDamage { get; set; } = null;

        /// <summary>
        /// DPダメージ
        /// </summary>
        public int? DPDamage { get; set; } = null;

        /// <summary>
        /// GPダメージ
        /// </summary>
        public int? GPDamage { get; set; } = null;

        /// <summary>
        /// HP回復
        /// </summary>
        public int? HPHealing { get; set; } = null;

        /// <summary>
        /// DP回復
        /// </summary>
        public int? DPHealing { get; set; } = null;

        /// <summary>
        /// GP回復
        /// </summary>
        public int? GPHealing { get; set; } = null;

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

            HPDamage = null;
            DPDamage = null;
            GPDamage = null;

            HPHealing = null;
            DPHealing = null;
            GPHealing = null;

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
            return HPDamage.HasValue || DPDamage.HasValue || GPDamage.HasValue;
        }

        /// <summary>
        /// 1以上のダメージがある
        /// </summary>
        /// <returns></returns>
        public bool IsOneOrOverDamage()
        {
            return HPDamage > 0 || DPDamage > 0 || GPDamage > 0;
        }

        /// <summary>
        /// 回復がある
        /// </summary>
        /// <returns></returns>
        public bool IsHealing()
        {
            return HPHealing.HasValue || DPHealing.HasValue || GPHealing.HasValue;
        }

        /// <summary>
        /// 1以上の回復がある
        /// </summary>
        /// <returns></returns>
        public bool IsOneOrOverHealing()
        {
            return HPHealing > 0 || DPHealing > 0 || GPHealing > 0;
        }

        /// <summary>
        /// HPダメージを増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddHPDamage(int value)
        {
            if (HPDamage.HasValue)
            {
                HPDamage += value;
            }
            else
            {
                HPDamage = value;
            }
        }

        /// <summary>
        /// HP回復を増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddHPHealing(int value)
        {
            if (HPHealing.HasValue)
            {
                HPHealing += value;
            }
            else
            {
                HPHealing = value;
            }
        }

        /// <summary>
        /// DPダメージを増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddDPDamage(int value)
        {
            if (DPDamage.HasValue)
            {
                DPDamage += value;
            }
            else
            {
                DPDamage = value;
            }
        }

        /// <summary>
        /// DP回復を増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddDPHealing(int value)
        {
            if (DPHealing.HasValue)
            {
                DPHealing += value;
            }
            else
            {
                DPHealing = value;
            }
        }

        /// <summary>
        /// GPダメージを増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddGPDamage(int value)
        {
            if (GPDamage.HasValue)
            {
                GPDamage += value;
            }
            else
            {
                GPDamage = value;
            }
        }

        /// <summary>
        /// GP回復を増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddGPHealing(int value)
        {
            if (GPHealing.HasValue)
            {
                GPHealing += value;
            }
            else
            {
                GPHealing = value;
            }
        }
    }
}
