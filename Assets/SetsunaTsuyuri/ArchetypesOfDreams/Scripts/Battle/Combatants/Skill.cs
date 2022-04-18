using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者のスキル
    /// </summary>
    public class Skill
    {
        /// <summary>
        /// 使用者
        /// </summary>
        public readonly Combatant User;

        /// <summary>
        /// データ
        /// </summary>
        public readonly SkillData Data;

        /// <summary>
        /// スキル属性
        /// </summary>
        public readonly Attribute.Skill SkillAttribute;

        ///// <summary>
        ///// 実際の攻撃属性
        ///// </summary>
        //public readonly Attribute.Attack ActualAttack;

        ///// <summary>
        ///// 実際の感情属性
        ///// </summary>
        //public readonly Attribute.Emotion ActualEmotion;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="user">このスキルを使う戦闘者</param>
        /// <param name="data">スキルデータ</param>
        /// <param name="attack">スキル属性</param>
        public Skill(Combatant user, SkillData data, Attribute.Skill skill = Attribute.Skill.Common)
        {
            // 使用者
            User = user;

            // データ
            Data = data;

            // スキル属性
            SkillAttribute = skill;

            //// 実際の攻撃属性
            //if (Data.Effect.Attack == Attribute.Attack.User)
            //{
            //    ActualAttack = skill switch
            //    {
            //        Attribute.Skill.MeleeWeapon => Attribute.Attack.Melee,
            //        Attribute.Skill.RangedWeapon => Attribute.Attack.Ranged,
            //        _ => Attribute.Attack.Mix,
            //    };
            //}
            //else
            //{
            //    ActualAttack = Data.Effect.Attack;
            //}

            //// 実際の感情属性
            //if (Data.Effect.Emotion == Attribute.Emotion.User)
            //{
            //    ActualEmotion = user.Emotion;
            //}
            //else
            //{
            //    ActualEmotion = Data.Effect.Emotion;
            //}
        }

        /// <summary>
        /// 使用できる
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <returns></returns>
        public bool CanBeUsed(BattleManager battle)
        {
            bool result = false;

            // 使用条件を満たしているならtrue
            if (User.Dream >= Data.Cost &&
                battle.ExistsTargetableOrTargetOfEffectIsNone(Data.Effect))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 実際の名前を取得する
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return Data.Name;
        }

        /// <summary>
        /// 実際の攻撃属性を取得する
        /// </summary>
        /// <returns></returns>
        public Attribute.Attack GetAttack()
        {
            Attribute.Attack result = SkillAttribute switch
            {
                Attribute.Skill.MeleeWeapon => Attribute.Attack.Melee,
                Attribute.Skill.RangedWeapon => Attribute.Attack.Ranged,
                _ => Attribute.Attack.Mix,
            };

            return result;
        }

        /// <summary>
        /// 実際の感情属性を取得する
        /// </summary>
        /// <returns></returns>
        public Attribute.Emotion GetEmotion()
        {
            Attribute.Emotion result = Data.Effect.Emotion switch
            {
                Attribute.Emotion.User => User.Emotion,
                _ => Data.Effect.Emotion
            };

            return result;
        }
    }
}
