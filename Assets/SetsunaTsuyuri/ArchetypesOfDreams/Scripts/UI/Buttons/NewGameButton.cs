using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ニューゲームボタン
    /// </summary>
    public class NewGameButton : GameButton
    {
        protected override void Awake()
        {
            base.Awake();

            // ボタンにイベントリスナーを登録する
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                // セーブ初期化
                SaveDataManager.CurrentSaveData.Initialize();

                // 最初のダンジョンへ遷移する
                RuntimeData.DungeonToPlay = MasterData.Dungeons.GetValue(0);
                SceneChangeManager.ChangeScene(ScenesName.Dungeon);
            });
        }
    }
}
