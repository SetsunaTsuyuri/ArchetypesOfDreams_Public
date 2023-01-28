using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームイベント(戦闘の管理者)
    /// </summary>
    [CreateAssetMenu(menuName = "GameEvents/BattleManager")]
    public class GameEventWithBattleManager : GameEventWithArgument<Battle> { }
}
