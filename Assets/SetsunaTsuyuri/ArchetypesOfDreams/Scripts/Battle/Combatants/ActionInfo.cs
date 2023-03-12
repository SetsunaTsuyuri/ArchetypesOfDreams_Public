using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者の行動内容
    /// </summary>
    public class ActionInfo
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
        public readonly GameAttribute.Skill SkillAttribute = GameAttribute.Skill.Common;

        /// <summary>
        /// 消費DP
        /// </summary>
        public int ConsumptionDP { get; set; } = 0;

        /// <summary>
        /// 消費アイテムID
        /// </summary>
        public int? ConsumptionItemdId { get; set; } = null;

        /// <summary>
        /// 効果データ
        /// </summary>
        public readonly EffectData Effect = null;

        /// <summary>
        /// スキルの行動内容を作る
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public static ActionInfo CreateSkillAction(int skillId)
        {
            SkillData skillData = MasterData.GetSkillData(skillId);
            ActionInfo skillAction = new(skillData);
            return skillAction;
        }

        /// <summary>
        /// アイテムの行動内容を作る
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static ActionInfo CreateItemAction(int itemId)
        {
            ItemData itemData = MasterData.GetItemData(itemId);
            ActionInfo itemAction = new(itemData);
            return itemAction;
        }

        public ActionInfo(SkillData skill)
        {
            // 名前
            Name = skill.Name;

            // 説明
            Description = skill.Description;

            // 消費DP
            ConsumptionDP = skill.Cost;

            // 効果データ
            Effect = skill.Effect;
        }

        public ActionInfo(ItemData item)
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
        /// 攻撃属性を取得する
        /// </summary>
        /// <returns></returns>
        public GameAttribute.Attack GetAttack()
        {
            GameAttribute.Attack userAttack = SkillAttribute switch
            {
                GameAttribute.Skill.PowerSkill => GameAttribute.Attack.Power,
                GameAttribute.Skill.TechniqueSkill => GameAttribute.Attack.Technique,
                _ => GameAttribute.Attack.Mix,
            };

            GameAttribute.Attack result = Effect.Attack switch
            {
                GameAttribute.Attack.User => userAttack,
                _ => Effect.Attack
            };

            return result;
        }

        /// <summary>
        /// 感情属性を取得する
        /// </summary>
        /// <param name="user">使用者</param>
        /// <returns></returns>
        public GameAttribute.Emotion GetEmotion(Combatant user)
        {
            GameAttribute.Emotion result = Effect.Emotion switch
            {
                GameAttribute.Emotion.User => user.Emotion,
                _ => Effect.Emotion
            };

            return result;
        }
    }
}
