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
                // 初期化する
                RuntimeData.Instance.Initialize();

                // 最初のダンジョンへ遷移する
                RuntimeData.DungeonToPlay = MasterData.Dungeons[0];
                SceneChangeManager.ChangeScene(SceneNames.Dungeon);
            });
        }
    }
}
