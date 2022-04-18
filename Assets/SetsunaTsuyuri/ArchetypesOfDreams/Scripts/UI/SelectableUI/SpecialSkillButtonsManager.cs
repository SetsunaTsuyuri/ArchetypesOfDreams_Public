using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 特殊スキルボタンの管理者
    /// </summary>
    public class SpecialSkillButtonsManager : SkillButtonsManager
    {
        protected override Skill[] GetSkills(Combatant combatant)
        {
            Skill[] result = base.GetSkills(combatant)
                .Where(x => x.SkillAttribute == Attribute.Skill.Special)
                .ToArray();

            return result;
        }
    }
}
