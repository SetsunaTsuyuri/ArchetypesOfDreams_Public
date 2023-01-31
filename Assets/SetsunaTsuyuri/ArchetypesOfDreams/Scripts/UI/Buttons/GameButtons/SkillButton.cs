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
            UpdateTexts(skill, () => skill.Cost.ToString());
        }
    }
}
