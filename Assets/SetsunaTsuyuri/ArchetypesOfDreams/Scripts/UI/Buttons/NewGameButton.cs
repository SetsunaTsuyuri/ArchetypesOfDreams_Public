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
                VariableData.Instance.Initialize();

                // 自室へ移動する
                SceneChangeManager.StartChange(SceneId.MyRoom);

                //// 最初のダンジョンへ遷移する
                //VariableData.DungeonId = 0;
                //SceneChangeManager.ChangeScene(SceneNames.Dungeon);
            });
        }
    }
}
