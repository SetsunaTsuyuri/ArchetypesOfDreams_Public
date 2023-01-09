using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 自室メニューの管理者
    /// </summary>
    public class MyRoomMenu : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// セーブデータメニュー
        /// </summary>
        [SerializeField]
        SaveDataMenu saveDataMenu = null;

        /// <summary>
        /// 説明文のUI
        /// </summary>
        [SerializeField]
        DescriptionUIManager description = null;

        /// <summary>
        /// 冒険ボタン
        /// </summary>
        [SerializeField]
        GameButton adventureButton = null;

        /// <summary>
        /// 冒険ボタンの説明文
        /// </summary>
        [SerializeField]
        string adventureDescription = string.Empty;

        /// <summary>
        /// セーブボタン
        /// </summary>
        [SerializeField]
        GameButton saveButton = null;

        /// <summary>
        /// セーブボタンの説明文
        /// </summary>
        [SerializeField]
        string saveDescription = string.Empty;

        protected override void Awake()
        {
            base.Awake();
            saveDataMenu.Previous = this;
        }

        public override void SetUp()
        {
            base.SetUp();

            // 冒険ボタンのセットアップ
            adventureButton.AddOnClickListener(() => SceneChangeManager.StartChange(SceneNames.DungeonSelection));
            adventureButton.AddTrrigerEntry(EventTriggerType.Select, (_) => description.SetText(adventureDescription));

            // セーブボタンのセットアップ
            saveButton.AddOnClickListener(() => saveDataMenu.UpdateButtonsAndSelect(SaveDataCommandType.Save));
            saveButton.AddTrrigerEntry(EventTriggerType.Select, (_) => description.SetText(saveDescription));
        }
    }
}
