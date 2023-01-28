using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘UIの管理者
    /// </summary>
    public class BattleUI : GameUI
    {
        /// <summary>
        /// 説明文のUI
        /// </summary>
        [field: SerializeField]
        public DescriptionUI Description { get; private set; } = null;

        /// <summary>
        /// 味方のUI
        /// </summary>
        [field: SerializeField]
        public AlliesUI AlliesUI { get; private set; } = null;

        /// <summary>
        /// 敵のUI
        /// </summary>
        public EnemiesUI EnemiesUI { get; private set; } = null;

        /// <summary>
        /// 戦闘コマンドボタンの管理者
        /// </summary>
        public BattleCommandsManager BattleCommands { get; private set; } = null;

        /// <summary>
        /// スキルボタンの管理者
        /// </summary>
        public SkillButtonsManager Skills { get; private set; } = null;

        /// <summary>
        /// アイテムボタンの管理者
        /// </summary>
        public ItemButtonsManager Items { get; private set; } = null;

        /// <summary>
        /// 行動順UIの管理者
        /// </summary>
        public OrderOfActionsDisplayer OrderOfActions { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            EnemiesUI = GetComponentInChildren<EnemiesUI>(true);

            BattleCommands = GetComponentInChildren<BattleCommandsManager>(true);
            Skills = GetComponentInChildren<SkillButtonsManager>(true);
            Items = GetComponentInChildren<ItemButtonsManager>(true);

            OrderOfActions = GetComponentInChildren<OrderOfActionsDisplayer>(true);
        }

        /// <summary>
        /// 各ボタンのセットアップを行う
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void SetUpButtons(Battle battle)
        {
            BattleCommands.SetUp(battle);
            Skills.SetUp(battle);
            Items.SetUp(battle);
        }

        /// <summary>
        /// 各ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void UpdateButtons(Battle battle)
        {
            BattleCommands.UpdateButtons(battle);
            Skills.UpdateButtons(battle);
            Items.UpdateButtons(battle);
        }
    }
}
