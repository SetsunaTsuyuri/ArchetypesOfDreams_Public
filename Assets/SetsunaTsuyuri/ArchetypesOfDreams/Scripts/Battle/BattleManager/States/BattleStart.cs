using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 戦闘開始
        /// </summary>
        private class BattleStart : FiniteStateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                // 戦闘続行不可能な場合
                if (!context.CanContinue())
                {

                    Debug.Log(context.Allies.CanFight());
                    // 戦闘終了
                    context.State.Change<BattleEnd>();
                    return;
                }

                // ターン開始
                context.State.Change<TurnStart>();
            }
        }
    }
}
