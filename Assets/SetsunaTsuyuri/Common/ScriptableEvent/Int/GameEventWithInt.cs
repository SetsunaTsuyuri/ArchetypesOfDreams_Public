using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// ゲームイベント(int)
    /// </summary>
    [CreateAssetMenu(fileName = "GameEventWithInt", menuName = "GameEvents/Int")]
    public class GameEventWithInt : GameEventWithArgument<int> { }
}
