using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// HPゲージ
    /// </summary>
    public class HPGauge : StatusGauge
    {
        protected override (int, int) GetValues(Combatant combatant)
        {
            return (combatant.CurrentHP, combatant.MaxHP);
        }
    }
}
