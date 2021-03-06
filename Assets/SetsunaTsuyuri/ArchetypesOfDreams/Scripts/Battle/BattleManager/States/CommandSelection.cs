using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// コマンド選択開始時のイベントリスナー
        /// </summary>
        [SerializeField]
        GameEventWithBattleManager onPlayerControlledCombatantCommandSelection = null;

        /// <summary>
        /// コマンド選択終了時のイベントリスナー
        /// </summary>
        [SerializeField]
        GameEvent onCommandSelectionExit = null;

        /// <summary>
        /// コマンド選択
        /// </summary>
        private class CommandSelection : FiniteStateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                // コマンド選択
                context.SelectCommandButtons();

                // 戦闘コマンド表示
                context.BattleUI.BattleCommands.Show();

                // イベント実行
                context.onPlayerControlledCombatantCommandSelection.Invoke(context);
            }

            public override void Exit(BattleManager context)
            {
                // 戦闘コマンド非表示
                context.BattleUI.BattleCommands.Hide();

                // イベント実行
                context.onCommandSelectionExit.Invoke();
            }
        }

        /// <summary>
        /// いずれかのコマンドボタンを選択する
        /// </summary>
        private void SelectCommandButtons()
        {
            BattleUI.BattleCommands.Select(Performer.Combatant.LastSelected);
        }
    }
}
