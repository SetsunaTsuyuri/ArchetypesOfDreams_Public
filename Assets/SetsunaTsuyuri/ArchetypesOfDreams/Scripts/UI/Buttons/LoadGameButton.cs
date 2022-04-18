using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ロードゲームボタン
    /// </summary>
    public class LoadGameButton : GameButton
    {
        /// <summary>
        /// セーブデータメニュー
        /// </summary>
        [SerializeField]
        SaveDataMenu saveDataMenu = null;

        protected override void Awake()
        {
            base.Awake();

            // ボタンにイベントリスナーを登録する
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                saveDataMenu.UpdateButtonsAndSelect(SaveDataCommandType.Load);
            });
        }
    }
}
