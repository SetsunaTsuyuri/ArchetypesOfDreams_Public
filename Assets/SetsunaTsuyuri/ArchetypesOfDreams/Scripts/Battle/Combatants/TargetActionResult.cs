using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動結果(対象者)
    /// </summary>
    public class TargetActionResult : IInitializable
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
        /// 命中した
        /// </summary>
        public bool Hit { get; set; } = false;

        /// <summary>
        /// クリティカルヒットした
        /// </summary>
        public bool Critical { get; set; } = false;

        /// <summary>
        /// 生命力ダメージ
        /// </summary>
        public int? LifeDamage { get; set; } = null;

        /// <summary>
        /// 夢想力ダメージ
        /// </summary>
        public int? DreamDamage { get; set; } = null;

        /// <summary>
        /// 精神力ダメージ
        /// </summary>
        public int? SoulDamage { get; set; } = null;

        /// <summary>
        /// 生命力回復
        /// </summary>
        public int? LifeRecovery { get; set; } = null;

        /// <summary>
        /// 夢想力回復
        /// </summary>
        public int? DreamRecovery { get; set; } = null;

        /// <summary>
        /// 精神力回復
        /// </summary>
        public int? SoulRecovery { get; set; } = null;

        /// <summary>
        /// 防御している
        /// </summary>
        public bool IsDefending { get; set; } = false;

        /// <summary>
        /// クラッシュした
        /// </summary>
        public bool Crushed { get; set; } = false;

        /// <summary>
        /// 浄化された
        /// </summary>
        public bool Purified { get; set; } = false;

        /// <summary>
        /// 交代の対象にされた
        /// </summary>
        public bool IsChangeTarget { get; set; }

        public void Initialize()
        {
            Affecter = null;

            Effectiveness = Attribute.Effectiveness.Normal;
            PurificationSuccessRate = 0;

            Hit = false;
            Critical = false;

            LifeDamage = null;
            DreamDamage = null;
            SoulDamage = null;

            LifeRecovery = null;
            DreamRecovery = null;
            SoulRecovery = null;

            IsDefending = false;
            Purified = false;
            Crushed = false;
            IsChangeTarget = false;
        }

        /// <summary>
        /// ダメージがある
        /// </summary>
        /// <returns></returns>
        public bool IsDamage()
        {
            return LifeDamage.HasValue || DreamDamage.HasValue || SoulDamage.HasValue;
        }

        /// <summary>
        /// 1以上のダメージがある
        /// </summary>
        /// <returns></returns>
        public bool IsOneOrOverDamage()
        {
            return (LifeDamage.HasValue && LifeDamage > 0) ||
                (DreamDamage.HasValue && DreamDamage > 0) ||
                (SoulDamage.HasValue && SoulDamage > 0);
        }

        /// <summary>
        /// 回復がある
        /// </summary>
        /// <returns></returns>
        public bool IsRecovery()
        {
            return LifeRecovery.HasValue || DreamRecovery.HasValue || SoulRecovery.HasValue;
        }

        /// <summary>
        /// 1以上の回復がある
        /// </summary>
        /// <returns></returns>
        public bool IsOneOrOverRecovery()
        {
            return (LifeRecovery.HasValue && LifeRecovery > 0) ||
                (DreamRecovery.HasValue && DreamRecovery > 0) ||
                (SoulRecovery.HasValue && SoulRecovery > 0);
        }

        /// <summary>
        /// 生命力ダメージを増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddLifeDamage(int value)
        {
            if (LifeDamage.HasValue)
            {
                LifeDamage += value;
            }
            else
            {
                LifeDamage = value;
            }
        }

        /// <summary>
        /// 生命力回復を増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddLifeRecovery(int value)
        {
            if (LifeRecovery.HasValue)
            {
                LifeRecovery += value;
            }
            else
            {
                LifeRecovery = value;
            }
        }

        /// <summary>
        /// 夢想力ダメージを増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddDreamDamage(int value)
        {
            if (DreamDamage.HasValue)
            {
                DreamDamage += value;
            }
            else
            {
                DreamDamage = value;
            }
        }

        /// <summary>
        /// 夢想力回復を増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddDreamRecovery(int value)
        {
            if (DreamRecovery.HasValue)
            {
                DreamRecovery += value;
            }
            else
            {
                DreamRecovery = value;
            }
        }

        /// <summary>
        /// 精神力ダメージを増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddSoulDamage(int value)
        {
            if (SoulDamage.HasValue)
            {
                SoulDamage += value;
            }
            else
            {
                SoulDamage = value;
            }
        }

        /// <summary>
        /// 精神力回復を増やす
        /// </summary>
        /// <param name="value">値</param>
        public void AddSoulRecovery(int value)
        {
            if (SoulRecovery.HasValue)
            {
                SoulRecovery += value;
            }
            else
            {
                SoulRecovery = value;
            }
        }
    }
}
