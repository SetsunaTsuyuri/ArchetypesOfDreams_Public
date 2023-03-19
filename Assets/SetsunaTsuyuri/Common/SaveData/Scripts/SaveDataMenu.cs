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
        SaveDataButton _buttonPrefab = null;

        /// <summary>
        /// キャラクターイメージプレハブ
        /// </summary>
        [SerializeField]
        Image _characterImagePrefab = null;

        /// <summary>
        /// キャラクターイメージの最大数
        /// </summary>
        [SerializeField]
        int _maxCharacterImages = 0;

        public override void SetUp()
        {
            // オートセーブ+セーブの数だけボタンを作る
            int numerOfButtons = SaveDataManager.SaveDataDic.Count + 1;
            _buttons = new SaveDataButton[numerOfButtons];

            for (int i = 0; i < numerOfButtons; i++)
            {
                SaveDataButton button = Instantiate(_buttonPrefab, _buttonsRoot.transform);
                button.GenerateCharacterImages(_characterImagePrefab, _maxCharacterImages);

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
            // オートセーブデータボタンのセットアップ
            _buttons[0].UpdateButton(_commandType, 0, true);

            // セーブデータボタンのセットアップ
            int saves = SaveDataManager.SaveDataDic.Count + 1;
            for (int i = 1; i < saves; i++)
            {
                _buttons[i].UpdateButton(_commandType, i, false);
            }

            // ナビゲーション更新
            UpdateButtonNavigations();
        }
    }
}
