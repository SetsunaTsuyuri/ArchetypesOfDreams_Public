using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動ボタンの管理者
    /// </summary>
    /// <typeparam name="TActionButton">行動ボタン</typeparam>
    public abstract class ActionButtonsManager<TActionButton> : SelectableGameUI<TActionButton>
        where TActionButton : ActionButton
    {
        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void SetUp(BattleManager battle)
        {
            Previous = battle.BattleUI.BattleCommands;

            SetUp();

            foreach (var button in _buttons)
            {
                button.SetUp(this);
            }

            Hide();
        }

        /// <summary>
        /// ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public virtual void UpdateButtons(BattleManager battle)
        {
            // 一旦、全てのボタンを隠す
            ShowOrHideAllButtons(false);
        }
    }
}
