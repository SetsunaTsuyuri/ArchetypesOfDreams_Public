using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// ターン開始
        /// </summary>
        private class TurnStart : FiniteStateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                // ターン数増加
                context.Turn++;

                // 行動順更新
                context.UpdateOrderOfActions();

                // 戦闘者のランダム速度更新
                context.UpdateRandomSpeedOfCombatants();

                // 行動順更新
                context.UpdateOrderOfActions();

                // 戦闘可能な者が存在するの場合
                if (context.OrderOfActions.Count > 0)
                {
                    // 戦闘者の行動
                    context.State.Change<ActionStart>();
                }
                else
                {
                    // ターン終了
                    context.State.Change<TurnEnd>();
                }
            }
        }
    }
}
