using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// DPの表示
    /// </summary>
    public class DPText : StatusText<int>
    {
        protected override void UpdateView()
        {
            view.text = value.ToString();
        }

        protected override int GetValue(Combatant combatant)
        {
            return combatant.CurrentDP;
        }
    }
}
