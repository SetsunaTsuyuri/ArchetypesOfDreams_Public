using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Battle
    {
        /// <summary>
        /// 戦闘者の行動開始
        /// </summary>
        private class ActionStart : StateMachine<Battle>.State
        {
            public override void Enter(Battle context)
            {
                // 行動者が存在しない場合
                if (!context.Actor)
                {
                    // 戦闘終了
                    context.State.Change<BattleEnd>();
                }

                // 行動者の使用スキルをリセットする
                context.ActorAction = null;

                // 行動開始時の処理
                context.Actor.Combatant.OnActionStart();

                // 行動者が操作可能な場合
                if (context.Actor.ContainsPlayerControlled())
                {
                    // 戦闘コマンド設定
                    context.BattleUI.UpdateUI(context);

                    // コマンド選択
                    context.State.Change<CommandSelection>();
                }
                else if (context.Actor.ContainsActionable()) // 行動可能な場合
                {
                    // AIが使用するスキルを決定する
                    context.ActorAction = context.Actor.Combatant.DecideAction(context);
                    if (context.ActorAction is not null)
                    {
                        // AIが対象を決定する
                        context.UpdateTargetables();
                        context.UpdateTargetFlags();

                        // 行動実行
                        context.State.Change<ActionExecution>();
                    }
                    else
                    {
                        // 行動終了
                        context.State.Change<ActionEnd>();
                    }
                }
                else
                {
                    // 行動終了
                    context.State.Change<ActionEnd>();
                }
            }
        }
    }
}
