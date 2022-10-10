using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者の行動内容
    /// </summary>
    public class ActionModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        public readonly string Name = string.Empty;

        /// <summary>
        /// 説明
        /// </summary>
        public readonly string Description = string.Empty;

        /// <summary>
        /// スキル属性
        /// </summary>
        public readonly Attribute.Skill SkillAttribute = Attribute.Skill.Common;

        /// <summary>
        /// 消費DP
        /// </summary>
        public int ConsumptionDp { get; set; } = 0;

        /// <summary>
        /// 消費アイテムID
        /// </summary>
        public int? ConsumptionItemdId { get; set; } = null;

        /// <summary>
        /// 効果データ
        /// </summary>
        public readonly EffectData Effect = null;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="skill">スキルデータ</param>
        /// <param name="skillAttribute">スキル属性</param>
        public ActionModel(SkillData skill, Attribute.Skill skillAttribute = Attribute.Skill.Common)
        {
            // 名前
            Name = skill.Name;

            // 説明
            Description = skill.Description;

            // 消費DP
            ConsumptionDp = skill.Cost;

            // スキル属性
            SkillAttribute = skillAttribute;

            // 効果データ
            Effect = skill.Effect;
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="item">アイテムデータ</param>
        public ActionModel(ItemData item)
        {
            // 名前
            Name = item.Name;

            // 説明
            Description = item.Description;

            // 効果データ
            Effect = item.Effect;

            // 消費アイテムの場合
            if (!item.IsReusable)
            {
                // 消費アイテムID
                ConsumptionItemdId = item.Id;
            }
        }

        /// <summary>
        /// 実行できる
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <returns></returns>
        public bool CanBeExecuted(BattleManager battle)
        {
            bool result = false;

            // 消費DPが足りていて、
            // なおかつ対象にできる戦闘者がいるもしくは対象を指定しない効果の場合
            if (battle.Actor.Combatant.CurrentDP >= ConsumptionDp
                && battle.ExistsTargetableOrTargetPositionIsNone(Effect))
            {
                // アイテムを消費する場合
                if (ConsumptionItemdId.HasValue)
                {
                    // 対象アイテムを所有しているなら実行できる
                    result = ItemUtility.HasItem(ConsumptionItemdId.Value);
                }
                else
                {
                    // 実行できる
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 実際の攻撃属性を取得する
        /// </summary>
        /// <returns></returns>
        public Attribute.Attack GetAttack()
        {
            Attribute.Attack userAttack = SkillAttribute switch
            {
                Attribute.Skill.PowerSkill => Attribute.Attack.Strength,
                Attribute.Skill.TechniqueSkill => Attribute.Attack.Technique,
                _ => Attribute.Attack.Mix,
            };

            Attribute.Attack result = Effect.Attack switch
            {
                Attribute.Attack.User => userAttack,
                _ => Effect.Attack
            };

            return result;
        }

        /// <summary>
        /// 実際の感情属性を取得する
        /// </summary>
        /// <param name="actor">行動者</param>
        /// <returns></returns>
        public Attribute.Emotion GetEmotion(Combatant actor)
        {
            Attribute.Emotion result = Effect.Emotion switch
            {
                Attribute.Emotion.User => actor.Emotion,
                _ => Effect.Emotion
            };

            return result;
        }
    }
}
