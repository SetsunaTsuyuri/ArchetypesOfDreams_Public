using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 味方を格納するコンテナ
    /// </summary>
    public class AllyContainer : CombatantContainer
    {
        public override void Escape()
        {
            base.Escape();

            MessageBrokersManager.AlliesEscape.Publish(Empty.Instance);
        }

        public override void OnDamage()
        {
            base.OnDamage();

            AudioManager.PlaySE(SEId.Damage);
        }

        public override bool ContainsPlayerControlled()
        {
            return ContainsActionable;
        }
    }
}
