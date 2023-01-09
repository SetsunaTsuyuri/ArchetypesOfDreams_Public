using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// GPゲージ
    /// </summary>
    public class GPGauge : StatusGauge
    {
        protected override (int, int) GetValues(Combatant combatant)
        {
            return (combatant.CurrentGP, combatant.MaxGP);
        }
    }
}
