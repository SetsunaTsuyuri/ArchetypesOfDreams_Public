using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームイベント(戦闘者)
    /// </summary>
    [CreateAssetMenu(menuName = "GameEvents/Combatant")]
    public class GameEventWithCombatant : GameEventWithArgument<Combatant> { }
}
