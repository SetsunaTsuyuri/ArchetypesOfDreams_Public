using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Battle
    {
        /// <summary>
        /// 行動実行
        /// </summary>
        private class ActionExecution : StateMachine<Battle>.State
        {
            public override void Enter(Battle context)
            {
                // 行動者が操作可能な場合
                if (context.Actor.ContainsPlayerControlled())
                {
                    // 戦闘コマンド設定
                    context.BattleUI.UpdateUI(context);

                    // コマンド選択
                    context.State.Change<CommandSelection>();
                    return;
                }

                ActionInfo action = null;
                if (context.Actor.ContainsActionable())
                {
                    // AIが使用するスキルを決定する
                    action = context.Actor.Combatant.DecideAction();
                }
                
                if (action is null)
                {
                    // 行動終了
                    context.State.Change<TurnEnd>();
                    return;
                }

                // AIが対象を決定する
                CombatantContainer[] targetables = context.Actor.GetTargetables(action.Effect).ToArray();
                CombatantContainer[] targets = GetTargets(context.Actor, targetables, action.Effect);

                // 行動する
                context.Actor.Act(action, targets, () => context.OnActionEnd(action));
            }

            /// <summary>
            /// 対象を取得する
            /// </summary>
            /// <param name="actor"></param>
            /// <param name="targetables"></param>
            /// <param name="effect"></param>
            private CombatantContainer[] GetTargets(CombatantContainer actor, CombatantContainer[] targetables, EffectData effect)
            {
                if (!targetables.Any()
                    || effect.TargetSelection != TargetSelectionType.Single)
                {
                    return targetables;
                }
                else
                {
                    int index = actor.Combatant.DecideTargetIndex(targetables);
                    CombatantContainer[] targets = { targetables[index] };

                    return targets;
                }
            }
        }

        /// <summary>
        /// 行動実行後の処理
        /// </summary>
        public void OnActionEnd(ActionInfo action)
        {
            if (action.Effect.CanActAgain)
            {
                UpdateOrderOfActions();
                State.Change<ActionExecution>();
            }
            else
            {
                State.Change<TurnEnd>();
            }
        }
    }
}
