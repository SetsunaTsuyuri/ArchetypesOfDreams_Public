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
        public void SetUp(DescriptionUIManager description)
        {
            SetUp();

            ShowOrHideAllButtons(false);
            bool[] openDungeons = RuntimeData.OpenDungeons;
            for (int i = 0; i < openDungeons.Length; i++)
            {
                // ダンジョンデータ
                DungeonData dungeon = MasterData.Dungeons[i];

                // ダンジョンボタン
                DungeonButton button = _buttons[i];

                // 選択できないダンジョンは非アクティブかつ選択不能にしてコンティニューする
                if (dungeon.CannotBeSelected)
                {
                    button.SetInteractable(false);
                    button.Hide();
                    continue;
                }

                // それをアクティブにして、さらに開放されているダンジョンを受け持つならデータを与えて有効化する
                button.gameObject.SetActive(true);
                if (openDungeons[i])
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
            UpdateButtonNavigationsToLoop();
        }
    }
}
