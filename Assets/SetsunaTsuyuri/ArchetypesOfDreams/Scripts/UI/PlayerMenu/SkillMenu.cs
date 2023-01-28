using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// スキルメニュー
    /// </summary>
    public class SkillMenu : ActionMenu<SkillButton>
    {
        // TODO: メニュー画面の場合、左右入力でスキル使用者を切り替える

        protected override void OnActionTargetSelection(TargetSelectionUI targetSelection, int id)
        {
            targetSelection.OnSkillTargetSelection(User, id);
        }

        protected override (int, bool)[] GetIdAndCanBeUsed()
        {
            var ids = User.Combatant.GetAcquisitionSkillIds();
            
            (int, bool)[] result = ids
                .Select(x => (x, User.CanUseSkill(x)))
                .ToArray();
                
            return result;
        }
    }
}
