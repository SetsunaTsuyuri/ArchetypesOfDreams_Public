using System.Collections.Generic;
using UnityEngine;

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

        protected override bool CanStartDamageTween(DamageResult damage)
        {
            return damage.HP > 0;
        }
    }
}
