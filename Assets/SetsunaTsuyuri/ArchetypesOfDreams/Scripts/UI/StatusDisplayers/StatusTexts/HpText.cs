using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// HPの表示
    /// </summary>
    public class HPText : StatusText<int>
    {
        protected override void UpdateView()
        {
            if (value == 0)
            {
                view.color = Color.red;
            }
            else
            {
                view.color = Color.white;
            }

            view.text = value.ToString();
        }

        protected override int GetValue(Combatant combatant)
        {
            return combatant.CurrentHP;
        }
    }
}
