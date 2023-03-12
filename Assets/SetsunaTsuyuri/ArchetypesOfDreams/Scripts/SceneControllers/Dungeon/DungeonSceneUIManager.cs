using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョンシーンのUI管理者
    /// </summary>
    public class DungeonSceneUIManager : UIManager
    {
        /// <summary>
        /// メインUI
        /// </summary>
        [field: SerializeField]
        public GameUI Main { get; private set; } = null;

        /// <summary>
        /// プレイヤーメニュー
        /// </summary>
        public PlayerMenu PlayerMenu { get; private set; } = null;

        /// <summary>
        /// 説明文
        /// </summary>
        public DescriptionUI Description { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlayerMenu = GetComponentInChildren<PlayerMenu>();
            Description = GetComponentInChildren<DescriptionUI>();
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="player">プレイヤー</param>
        /// <param name="allies">味方</param>
        /// <param name="battle">戦闘</param>
        public void SetUp(Player player, AlliesParty allies, Battle battle)
        {
            // 対象選択UI
            TargetSelectionUI targetSelection = GetComponentInChildren<TargetSelectionUI>();
            targetSelection.SetUp(Description, allies, battle);

            PopUpTextsManager popUpTexts = GetComponentInChildren<PopUpTextsManager>();
            popUpTexts.SetUp(targetSelection);

            // スキルメニュー
            SkillMenu skillMenu = GetComponentInChildren<SkillMenu>();
            skillMenu.SetUp(Description, targetSelection, allies.AlliesUI);

            // アイテムメニュー
            ItemMenu itemMenu = GetComponentInChildren<ItemMenu>();
            itemMenu.SetUp(Description, targetSelection);

            // プレイヤーメニュー
            PlayerMenu.SetUp(Description, skillMenu, itemMenu, targetSelection, allies);
            PlayerMenu.Canceled += () =>
            {
                Main.FadeIn();
                player.EnableInput();
            };

            // オプションメニュー
            OptionsMenu optionsMenu = GetComponentInChildren<OptionsMenu>();
            optionsMenu.SetUp();
        }
    }
}
