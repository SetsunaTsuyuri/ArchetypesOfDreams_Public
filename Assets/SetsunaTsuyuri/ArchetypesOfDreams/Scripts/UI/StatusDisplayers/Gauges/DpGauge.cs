using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// DPゲージ
    /// </summary>
    public class DPGauge : StatusGauge
    {
        protected override (int, int) GetValues(Combatant combatant)
        {
            return (combatant.CurrentDP, combatant.MaxDP);
        }
    }
}
