using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョン選択ボタンの管理者
    /// </summary>
    public class DungeonButtonsManager : SelectableGameUI<DungeonButton>
    {
        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="description">説明文の管理者</param>
        public void SetUp(DescriptionUI description)
        {
            SetUp();

            ShowOrHideAllButtons(false);

            DungeonData[] selectableDungeons = MasterData.GetSelectableDungeons();

            for (int i = 0; i < selectableDungeons.Length; i++)
            {
                // ダンジョンデータ
                DungeonData dungeon = selectableDungeons[i];

                // ダンジョンボタン
                DungeonButton button = _buttons[i];

                // ボタンをアクティブにして、さらに開放されているダンジョンを受け持つならデータを与えて有効化する
                button.gameObject.SetActive(true);
                if (VariableData.SelectableDungeons[dungeon.Id])
                {
                    button.SetUp(dungeon, description);
                    button.SetInteractable(true);
                }
                else
                {
                    button.SetInteractable(false);
                }
            }

            // ナビゲーション更新
            UpdateButtonNavigations();
        }
    }
}
