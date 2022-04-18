using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者
    /// </summary>
    [Serializable]
    public abstract partial class Combatant : IInitializable
    {
        /// <summary>
        /// 戦闘者コンテナ
        /// </summary>
        public CombatantContainer Container { get; set; } = null;

        /// <summary>
        /// データID
        /// </summary>
        [field: SerializeField]
        public int DataId { get; set; } = 0;

        /// <summary>
        /// リーダーである
        /// </summary>
        public bool IsLeader { get; set; } = false;

        /// <summary>
        /// ボス耐性を持っている
        /// </summary>
        public bool HasBossResistance { get; set; } = false;

        /// <summary>
        /// 健康状態
        /// </summary>
        Attribute.Condition condition = Attribute.Condition.Normal;

        /// <summary>
        /// 健康状態
        /// </summary>
        public Attribute.Condition Condition
        {
            get => condition;
            set
            {
                condition = value;
                if (Container)
                {
                    Container.OnConditionSet();
                }
            }
        }

        /// <summary>
        /// 防御している
        /// </summary>
        public bool IsDefending { get; set; } = false;

        /// <summary>
        /// 行動済みである
        /// </summary>
        public bool Acted { get; set; } = false;

        /// <summary>
        /// 最後に選択したコマンド
        /// </summary>
        public Selectable LastSelected { get; set; } = null;

        /// <summary>
        /// 行動の結果
        /// </summary>
        public TargetActionResult Result { get; set; } = new TargetActionResult();

        public void Initialize()
        {
            Result.Initialize();
            Experience = LevelToExperience();
            Acted = false;
            InitializeStatus();
            LastSelected = null;
        }

        /// <summary>
        /// データを取得する
        /// </summary>
        /// <returns></returns>
        public abstract CombatantData GetData();

        /// <summary>
        /// 行動可能である
        /// </summary>
        /// <returns></returns>
        public bool CanAct()
        {
            bool result = false;

            if (!Acted &&
                (Condition == Attribute.Condition.Normal ||
                (Condition == Attribute.Condition.Crush && HasBossResistance)))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// クラッシュしている
        /// </summary>
        /// <returns></returns>
        public bool IsCrushed()
        {
            return Condition == Attribute.Condition.Crush;
        }

        /// <summary>
        /// 戦闘不能である
        /// </summary>
        /// <returns></returns>
        public bool IsKnockedOut()
        {
            return Condition == Attribute.Condition.KnockedOut;
        }

        /// <summary>
        /// 近接武器スキルを有している
        /// </summary>
        /// <returns></returns>
        public bool HasMeleeWeaponSkills()
        {
            return meleeWeaponSkillIdList.Count > 0;
        }

        /// <summary>
        /// 遠隔武器スキルを有している
        /// </summary>
        /// <returns></returns>
        public bool HasRangedWeaponSkills()
        {
            return rangedWeaponSkillIdList.Count > 0;
        }

        /// <summary>
        /// 特殊スキルを有している
        /// </summary>
        /// <returns></returns>
        public bool HasSpecialSkills()
        {
            return specialSkillIdList.Count > 0;
        }

        /// <summary>
        /// 近接武器コマンドを選択できる
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <returns></returns>
        public bool CanSelectAnyMeleeWeaponSkills(BattleManager battle)
        {
            return GetAvailableSkills(battle)
                .Where(x => x.SkillAttribute == Attribute.Skill.MeleeWeapon)
                .Any();
        }

        /// <summary>
        /// 遠隔武器コマンドを選択できる
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <returns></returns>
        public bool CanSelectAnyRangedWeaponSkills(BattleManager battle)
        {
            return GetAvailableSkills(battle)
                .Where(x => x.SkillAttribute == Attribute.Skill.RangedWeapon)
                .Any();
        }

        /// <summary>
        /// 特殊スキルコマンドを選択できる
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <returns></returns>
        public bool CanSelectAnySpecialSkills(BattleManager battle)
        {
            return GetAvailableSkills(battle)
                .Where(x => x.SkillAttribute == Attribute.Skill.Special)
                .Any();
        }

        /// <summary>
        /// 防御コマンドを選択できる
        /// </summary>
        /// <returns></returns>
        public bool CanSelectGuard()
        {
            return true;
        }

        /// <summary>
        /// 浄化コマンドを選択できる
        /// </summary>
        /// <returns></returns>
        public bool CanSelectPurification()
        {
            return this is DreamWalker;
        }

        /// <summary>
        /// コンテナから解放できる
        /// </summary>
        /// <returns></returns>
        public virtual bool CanBeReleased()
        {
            return false;
        }

        /// <summary>
        /// 生命力の割合を取得する
        /// </summary>
        /// <returns></returns>
        public float GetLifeRate()
        {
            return GetStatusRate(Life, MaxLife);
        }

        /// <summary>
        /// 生命力の減少率を取得する
        /// </summary>
        /// <returns></returns>
        public float GetLifeReductionRate()
        {
            return 1.0f - GetLifeRate();
        }

        /// <summary>
        /// 夢想力の割合を取得する
        /// </summary>
        /// <returns></returns>
        public float GetDreamRate()
        {
            return GetStatusRate(Dream, GameSettings.Combatants.MaxDream);
        }

        /// <summary>
        /// 夢想力の減少率を取得する
        /// </summary>
        /// <returns></returns>
        public float GetDreamReductionRate()
        {
            return 1.0f - GetDreamRate();
        }

        /// <summary>
        /// 精神力の割合を取得する
        /// </summary>
        /// <returns></returns>
        public float GetSoulRate()
        {
            return GetStatusRate(Soul, MaxSoul);
        }

        /// <summary>
        /// 精神力の減少率を取得する
        /// </summary>
        /// <returns></returns>
        public float GetSoulReductionRate()
        {
            return 1.0f - GetSoulRate();
        }

        /// <summary>
        /// ステータスの割合を取得する
        /// </summary>
        /// <param name="remaining">現在値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        private float GetStatusRate(int remaining, int max)
        {
            return remaining / max;
        }
    }
}
