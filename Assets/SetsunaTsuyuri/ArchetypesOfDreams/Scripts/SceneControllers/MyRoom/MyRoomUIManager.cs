using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 自室シーンUIの管理者
    /// </summary>
    public class MyRoomUIManager : UIManager
    {
        /// <summary>
        /// ヒロインのImage
        /// </summary>
        [SerializeField]
        Image _heroineImage = null;

        /// <summary>
        /// 自室メニュー
        /// </summary>
        public MyRoomMenu MyRoomMenu { get; private set; } = null;

        /// <summary>
        /// 味方UI
        /// </summary>
        AlliesUI _allies = null;

        /// <summary>
        /// 説明UI
        /// </summary>
        DescriptionUI _description = null;

        protected override void Awake()
        {
            base.Awake();

            MyRoomMenu = GetComponentInChildren<MyRoomMenu>();
            _allies = GetComponentInChildren<AlliesUI>();
            _description = GetComponentInChildren<DescriptionUI>();
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="allies">味方</param>
        public void SetUp(AlliesParty allies)
        {
            // セーブデータメニュー
            SaveDataMenu saveDataMenu = GetComponentInChildren<SaveDataMenu>();
            saveDataMenu.SetUp();

            // 対象選択UI
            TargetSelectionUI targetSelection = GetComponentInChildren<TargetSelectionUI>();
            targetSelection.SetUp(_description, allies, null);

            // スキルメニュー
            SkillMenu skillMenu = GetComponentInChildren<SkillMenu>();
            skillMenu.SetUp(_description, targetSelection, allies.AlliesUI);

            // アイテムメニュー
            ItemMenu itemMenu = GetComponentInChildren<ItemMenu>();
            itemMenu.SetUp(_description, targetSelection);

            // プレイヤーメニュー
            PlayerMenu playerMenu = GetComponentInChildren<PlayerMenu>();
            playerMenu.SetUp(_description, skillMenu, itemMenu, targetSelection, allies);

            // オプションメニュー
            OptionsMenu optionsMenu= GetComponentInChildren<OptionsMenu>();
            optionsMenu.SetUp();

            // 自室メニュー
            MyRoomMenu.SetUp();
        }

        /// <summary>
        /// 非表示になっているUIを表示する
        /// </summary>
        public void SetUIsEnabled(bool value)
        {
            _description.SetEnabled(value);
            _allies.SetEnabled(value);
            _heroineImage.enabled = value;
        }
    }
}
