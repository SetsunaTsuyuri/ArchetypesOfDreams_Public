using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// ターン終了
        /// </summary>
        private class TurnEnd : FiniteStateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                // ターン終了時の処理
                context.Allies.OnTurnEnd();
                context.Enemies.OnTurnEnd();

                // 戦闘続行可能の場合
                if (context.CanContinue())
                {
                    // 次のターンへ
                    context.State.Change<TurnStart>();
                }
                else
                {
                    // 戦闘終了
                    context.State.Change<BattleEnd>();
                }
            }
        }
    }
}
