using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムボタンの管理者
    /// </summary>
    public class ItemButtonsManager : ActionButtonsManager<ItemButton>
    {
        /// <summary>
        /// ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public override void UpdateButtons(Battle battle)
        {
            base.UpdateButtons(battle);

            ActionInfo[] items = ItemUtility.GetActionModels();

            for (int i = 0; i < items.Length && i < _buttons.Length; i++)
            {
                ActionInfo item = items[i];
                ItemButton button = _buttons[i];

                // ボタンのセットアップ
                button.UpdateButton(item, battle);

                // ボタンを表示する
                button.Show();
            }

            // ナビゲーション更新
            UpdateButtonNavigations();
        }
    }

}
