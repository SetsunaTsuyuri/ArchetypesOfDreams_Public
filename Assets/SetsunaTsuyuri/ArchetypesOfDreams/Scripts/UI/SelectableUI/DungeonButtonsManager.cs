using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョン選択ボタンの管理者
    /// </summary>
    public class DungeonButtonsManager : SelectableGameUI<DungeonButton>
    {
        /// <summary>
        /// ダンジョンボタンをセットアップする
        /// </summary>
        /// <param name="description">説明文の管理者</param>
        public void SetUpDungeonButtons(DescriptionUIManager description)
        {
            SetUpButtons();

            SaveData save = SaveDataManager.CurrentSaveData;
            bool[] openDungeons = save.OpenDungeons;

            for (int i = 0; i < openDungeons.Length; i++)
            {
                // ダンジョンデータ
                DungeonData dungeon = MasterData.Dungeons.GetValue(i);

                // ダンジョンボタン
                DungeonButton button = buttons[i];

                // 選択できないダンジョンは非アクティブかつ選択不能にしてコンティニューする
                if (dungeon.CannotBeSelected)
                {
                    button.SetInteractable(false);
                    button.gameObject.SetActive(false);
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
