using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// スキルボタン
    /// </summary>
    public class SkillButton : ActionButton
    {
        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="canBeUsed">使用できる</param>
        public override void UpdateButton(int id, bool canBeUsed)
        {
            base.UpdateButton(id, canBeUsed);

            SkillData skill = MasterData.GetSkillData(id);
            Description = skill.Description;

            if (Name)
            {
                Name.text = skill.Name;
            }

            if (Number)
            {
                Number.text = skill.Cost.ToString();
            }
        }

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="action">行動</param>
        /// <param name="battle">戦闘の管理者</param>
        public override void UpdateButton(ActionInfo action, Battle battle)
        {
            base.UpdateButton(action, battle);

            // コスト表示
            if (Number)
            {
                Number.text = action.ConsumptionDP.ToString();
            }
        }
    }
}
