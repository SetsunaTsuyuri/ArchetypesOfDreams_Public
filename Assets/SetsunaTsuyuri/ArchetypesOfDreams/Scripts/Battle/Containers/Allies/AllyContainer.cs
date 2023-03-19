using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 味方を格納するコンテナ
    /// </summary>
    public class AllyContainer : CombatantContainer
    {
        public override bool ContainsPlayerControlled()
        {
            return ContainsActionable;
        }
    }
}
