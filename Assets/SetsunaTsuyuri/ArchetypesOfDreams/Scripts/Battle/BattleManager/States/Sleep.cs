using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 休眠状態
        /// </summary>
        private class Sleep : FiniteStateMachine<BattleManager>.State { }
    }
}
