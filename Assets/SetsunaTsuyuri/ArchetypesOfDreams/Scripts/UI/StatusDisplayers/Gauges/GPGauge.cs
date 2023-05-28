using System.Collections.Generic;
using UnityEngine;

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

        protected override bool CanStartDamageTween(DamageResult damage)
        {
            return damage.GP > 0;
        }
    }
}
