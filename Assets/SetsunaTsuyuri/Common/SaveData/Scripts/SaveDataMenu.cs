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
        /// ボタンのプレハブ
        /// </summary>
        [SerializeField]
        SaveDataButton buttonPrefab = null;

        protected override void Awake()
        {
            base.Awake();

            // オートセーブ+セーブの数だけボタンを作り配列化する
            int numerOfButtons = SaveDataManager.Saves.Length + 1;
            buttons = new SaveDataButton[numerOfButtons];
            for (int i = 0; i < numerOfButtons; i++)
            {
                SaveDataButton button = Instantiate(buttonPrefab, layoutGroup.transform);
                buttons[i] = button;
            }
        }

        private void Start()
        {
            // ボタンをセットアップする
            SetUpButtons();

            // 隠す
            Hide();
        }

        /// <summary>
        /// 各ボタンを更新し、いずれかのボタンを選択状態にする
        /// </summary>
        /// <param name="command">セーブデータコマンド</param>
        public void UpdateButtonsAndSelect(SaveDataCommandType command)
        {
            // オートセーブ用ボタンのセットアップ
            buttons[0].SetUp(command, 0, true);

            // セーブ用ボタンのセットアップ
            int saves = SaveDataManager.Saves.Length + 1;
            for (int i = 1; i < saves; i++)
            {
                buttons[i].SetUp(command, i - 1);
            }

            // ナビゲーション更新
            UpdateButtonNavigationsToLoop();

            // 選択する
            Select();
        }
    }
}
