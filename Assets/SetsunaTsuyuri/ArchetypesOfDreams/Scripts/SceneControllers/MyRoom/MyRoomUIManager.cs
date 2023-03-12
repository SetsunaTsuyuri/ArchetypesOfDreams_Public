using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 自室シーンUIの管理者
    /// </summary>
    public class MyRoomUIManager : UIManager
    {
        /// <summary>
        /// 自室メニュー
        /// </summary>
        public MyRoomMenu MyRoomMenu { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            MyRoomMenu = GetComponentInChildren<MyRoomMenu>();
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="allies">味方</param>
        public void SetUp(AlliesParty allies)
        {
            // 説明文UI
            DescriptionUI description = GetComponentInChildren<DescriptionUI>();

            // セーブデータメニュー
            SaveDataMenu saveDataMenu = GetComponentInChildren<SaveDataMenu>();
            saveDataMenu.SetUp();

            // 対象選択UI
            TargetSelectionUI targetSelection = GetComponentInChildren<TargetSelectionUI>();
            targetSelection.SetUp(description, allies, null);

            // スキルメニュー
            SkillMenu skillMenu = GetComponentInChildren<SkillMenu>();
            skillMenu.SetUp(description, targetSelection, allies.AlliesUI);

            // アイテムメニュー
            ItemMenu itemMenu = GetComponentInChildren<ItemMenu>();
            itemMenu.SetUp(description, targetSelection);

            // プレイヤーメニュー
            PlayerMenu playerMenu = GetComponentInChildren<PlayerMenu>();
            playerMenu.SetUp(description, skillMenu, itemMenu, targetSelection, allies);

            // オプションメニュー
            OptionsMenu optionsMenu= GetComponentInChildren<OptionsMenu>();
            optionsMenu.SetUp();

            // 自室メニュー
            MyRoomMenu.SetUp();
        }
    }
}
