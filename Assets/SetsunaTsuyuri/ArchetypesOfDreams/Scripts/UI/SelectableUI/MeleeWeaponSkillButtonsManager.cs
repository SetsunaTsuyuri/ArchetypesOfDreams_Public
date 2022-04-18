using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 近接武器スキルボタンの管理者
    /// </summary>
    public class MeleeWeaponSkillButtonsManager : SkillButtonsManager
    {
        protected override Skill[] GetSkills(Combatant combatant)
        {
            Skill[] result = base.GetSkills(combatant)
                .Where(x => x.SkillAttribute == Attribute.Skill.MeleeWeapon)
                .ToArray();

            return result;
        }
    }
}
