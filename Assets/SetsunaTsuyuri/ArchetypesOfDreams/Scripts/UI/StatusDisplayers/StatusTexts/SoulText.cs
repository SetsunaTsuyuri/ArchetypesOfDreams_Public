using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 精神力の表示
    /// </summary>
    public class SoulText : StatusText<int>
    {
        protected override void UpdateView()
        {
            view.text = value.ToString();
        }

        protected override int GetValue(Combatant combatant)
        {
            return combatant.Soul;
        }
    }
}
