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
            // 消費DP
            ConsumptionDP = skill.Cost;

            // 効果データ
            Effect = skill;
        }

        public ActionInfo(ItemData item)
        {
            // 効果データ
            Effect = item;

            // 消費アイテムID
            ConsumptionItemdId = !item.IsReusable ? item.Id : null;
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
