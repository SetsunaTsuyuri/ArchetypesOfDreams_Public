using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 名前の表示
    /// </summary>
    public class NameText : StatusText<string>
    {
        protected override void UpdateView()
        {
            view.text = value;
        }

        protected override string GetValue(Combatant combatant)
        {
            return combatant.GetData().Name;
        }
    }
}
