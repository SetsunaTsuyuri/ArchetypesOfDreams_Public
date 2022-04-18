using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 戦闘者の行動開始
        /// </summary>
        private class ActionStart : FiniteStateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                // 行動中の戦闘者コンテナ
                context.Performer = context.OrderOfActions[context.OrderOfActions.Count - 1];

                // その戦闘者コンテナが戦闘者を格納している場合
                if (context.Performer.ContainsCombatant())
                {
                    // 行動開始時の処理
                    context.Performer.Combatant.OnActionStart();

                    // それが操作可能な場合
                    if (context.Performer.ContainsPlayerControlled())
                    {
                        // 戦闘コマンド設定
                        context.UpdateCommandButtons();

                        // コマンド選択ステートへ移行する
                        context.State.Change<CommandSelection>();
                    }
                    else if (context.Performer.ContainsActionable()) // 行動可能な場合
                    {
                        // AIが使用するスキルを決定する
                        context.SkillToBeUsed = context.Performer.Combatant.DecideTheSkillToBeUsed(context);

                        // スキルが決定された場合
                        if (!(context.SkillToBeUsed is null))
                        {
                            // AIが対象を決定する
                            context.UpdateTargetables();
                            context.UpdateTargetFlags();

                            // 行動実行
                            context.State.Change<ActionExecution>();
                        }
                        else
                        {
                            // 行動済みフラグON
                            context.Performer.Combatant.Acted = true;

                            // 行動終了
                            context.State.Change<ActionEnd>();
                        }
                    }
                    else
                    {
                        // 行動済みフラグON
                        context.Performer.Combatant.Acted = true;

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

        /// <summary>
        /// 各戦闘コマンドボタンを設定し、選択状態にする
        /// </summary>
        private void UpdateCommandButtons()
        {
            BattleUI.UpdateButtons(this);
        }
    }
}
