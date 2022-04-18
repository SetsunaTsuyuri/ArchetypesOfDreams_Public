using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// ゲームイベント(string)
    /// </summary>
    [CreateAssetMenu(fileName = "GameEventString", menuName = "GameEvents/String")]
    public class GameEventWithString : GameEventWithArgument<string> { }
}
