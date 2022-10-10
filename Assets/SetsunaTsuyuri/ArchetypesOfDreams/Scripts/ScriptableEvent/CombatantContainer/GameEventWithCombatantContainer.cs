using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームイベント(戦闘者コンテナ)
    /// </summary>
    [CreateAssetMenu(menuName = "GameEvents/CombatantContainer")]
    public class GameEventWithCombatantContainer : GameEventWithArgument<CombatantContainer> { }
}
