using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 遠隔武器スキルボタンの管理者
    /// </summary>
    public class RangedWeaponSkillButtonsManager : SkillButtonsManager
    {
        protected override Skill[] GetSkills(Combatant combatant)
        {
            Skill[] result = base.GetSkills(combatant)
                .Where(x => x.SkillAttribute == Attribute.Skill.RangedWeapon)
                .ToArray();

            return result;
        }
    }
}
