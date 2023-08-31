using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// GPの表示
    /// </summary>
    public class GPText : StatusText<int>
    {
        protected override void UpdateView()
        {
            view.text = value.ToString();
        }

        protected override int GetValue(Combatant combatant)
        {
            return combatant.CurrentGP;
        }
    }
}
