using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// タイトルメニューボタンの管理者
    /// </summary>
    public class TitleMenu : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// ニューゲームボタン
        /// </summary>
        [SerializeField]
        GameButton _newGameButton = null;

        /// <summary>
        /// ロードゲームボタン
        /// </summary>
        [SerializeField]
        GameButton _loadGameButton = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="savedataMenu">セーブデータメニュー</param>
        public void SetUp(SelectableGameUIBase savedataMenu)
        {
            base.SetUp();
            
            // ニューゲーム
            _newGameButton.AddPressedListener(() =>
            {
                // 初期化する
                VariableData.Instance.Initialize();

                // 自室へ移動する
                SceneChangeManager.StartChange(SceneType.MyRoom);
            });

            // ロードゲーム
            _loadGameButton.AddPressedListener(() => Stack(typeof(SaveDataMenu)));

            // TODO: オプション オプションメニューを開く

            // TODO: クレジット クレジット画面を開く
        }
    }
}
