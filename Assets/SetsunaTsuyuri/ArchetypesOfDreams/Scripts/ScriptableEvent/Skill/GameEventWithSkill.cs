using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームイベント(スキル)
    /// </summary>
    [CreateAssetMenu(menuName = "GameEvents/Skill")]
    public class GameEventWithSkill : GameEventWithArgument<ActionModel> { }
}
