using System.Collections.Generic;
using System.Linq;
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
        /// オプションボタン
        /// </summary>
        [SerializeField]
        GameButton _optionsButton = null;

        /// <summary>
        /// クレジットボタン
        /// </summary>
        [SerializeField]
        GameButton _creditsButton = null;

        /// <summary>
        /// 終了ボタン
        /// </summary>
        [SerializeField]
        GameButton _quitButton = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();
            
            // ニューゲーム
            _newGameButton.AddPressedListener(() =>
            {
                // 初期化する
                VariableData.Instance.Initialize();

                // ダンジョンへ移動する
                SceneChangeManager.StartChange(SceneId.Dungeon);
            });

            // ロードゲーム
            _loadGameButton.AddPressedListener(() => Stack(typeof(SaveDataMenu)));

            // ロードできるデータがない場合はロードゲームボタンを封印する
            if (SaveDataManager.SaveDataDictionary.Values.All(x => x is null)
                && SaveDataManager.AutoSaveData is null)
            {
                _loadGameButton.IsSeald = true;
            }

            // オプション
            _optionsButton.AddPressedListener(() => Stack(typeof(OptionsMenu)));

            // クレジット
            _creditsButton.AddPressedListener(() => Stack(typeof(CreditsUI)));

            // ゲーム終了
            _quitButton.AddPressedListener(() => ApplicationUtility.Quit());
        }
    }
}
