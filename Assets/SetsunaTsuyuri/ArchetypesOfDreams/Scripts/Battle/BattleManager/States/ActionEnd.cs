using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 行動終了開始時のイベント
        /// </summary>
        [SerializeField]
        GameEvent onActionEndEnter = null;

        /// <summary>
        /// 戦闘者の行動終了
        /// </summary>
        private class ActionEnd : FiniteStateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                // イベント呼び出し
                context.onActionEndEnter.Invoke();

                // 行動後の処理
                context.Performer.Combatant?.OnActionEnd();

                // 解放可能な死亡者を解放する
                context.Allies.ReleaseDeadReleasables();
                context.Enemies.ReleaseDeadReleasables();

                // 味方に空きがあり、控えに戦闘者がいる場合、それを主要メンバーに加える
                context.Allies.InjectReserveMembersIntoMainMembers();
                
                // 戦闘続行可能の場合
                if (context.CanContinue())
                {
                    // 行動者コンテナが行動できる戦闘者を格納してない場合
                    if (!context.Performer.ContainsActionable())
                    {
                        // 行動順リストの最後尾を削除する(次の行動者の番になる)
                        context.OrderOfActions.RemoveAt(context.OrderOfActions.Count - 1);
                    }

                    // 手番の回ってきていない戦闘者がいる場合
                    if (context.OrderOfActions.Count > 0)
                    {
                        // 行動開始
                        context.State.Change<ActionStart>();
                    }
                    else
                    {
                        // ターン終了
                        context.State.Change<TurnEnd>();
                    }
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
