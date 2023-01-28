using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// セーブデータメニュー
    /// </summary>
    public class SaveDataMenu : SelectableGameUI<SaveDataButton>
    {
        /// <summary>
        /// コマンドタイプ
        /// </summary>
        [SerializeField]
        SaveDataCommandType _commandType = SaveDataCommandType.Save;

        /// <summary>
        /// ボタンのプレハブ
        /// </summary>
        [SerializeField]
        SaveDataButton buttonPrefab = null;

        public override void SetUp()
        {
            // オートセーブ+セーブの数だけボタンを作り配列化する
            int numerOfButtons = SaveDataManager.Saves.Length + 1;
            _buttons = new SaveDataButton[numerOfButtons];
            for (int i = 0; i < numerOfButtons; i++)
            {
                SaveDataButton button = Instantiate(buttonPrefab, _layoutGroup.transform);
                _buttons[i] = button;
            }

            base.SetUp();

            Hide();
        }

        public override void BeSelected()
        {
            UpdateButtons();
            base.BeSelected();
        }

        public override void BeCanceled()
        {
            base.BeCanceled();
            Hide();
        }

        /// <summary>
        /// 各ボタンを更新する
        /// </summary>
        public void UpdateButtons()
        {
            // オートセーブ用ボタンのセットアップ
            _buttons[0].SetUp(_commandType, 0, true);

            // セーブ用ボタンのセットアップ
            int saves = SaveDataManager.Saves.Length + 1;
            for (int i = 1; i < saves; i++)
            {
                _buttons[i].SetUp(_commandType, i - 1, false);
            }

            // ナビゲーション更新
            UpdateButtonNavigations();
        }
    }
}
