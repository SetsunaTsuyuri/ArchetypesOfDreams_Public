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
        /// 対象選択
        /// </summary>
        [field: SerializeField]
        public TargetSelectionUI TargetSelection { get; private set; } = null;

        /// <summary>
        /// スキルメニュー
        /// </summary>
        [field: SerializeField]
        public SkillMenu SkillMenu { get; private set; } = null;

        /// <summary>
        /// アイテムメニュー
        /// </summary>
        [field: SerializeField]
        public ItemMenu ItemMenu { get; private set; } = null;

        /// <summary>
        /// 敵のUI
        /// </summary>
        public EnemiesUI EnemiesUI { get; private set; } = null;

        /// <summary>
        /// 戦闘コマンドボタンの管理者
        /// </summary>
        public BattleCommandsManager BattleCommands { get; private set; } = null;

        /// <summary>
        /// 行動順UIの管理者
        /// </summary>
        public OrderOfActionsDisplayer OrderOfActions { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            EnemiesUI = GetComponentInChildren<EnemiesUI>(true);

            BattleCommands = GetComponentInChildren<BattleCommandsManager>(true);

            OrderOfActions = GetComponentInChildren<OrderOfActionsDisplayer>(true);
        }

        /// <summary>
        /// セットアップを行う
        /// </summary>
        /// <param name="battle">戦闘</param>
        public void SetUp(Battle battle)
        {
            BattleCommands.SetUp(battle);
        }

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="battle">戦闘</param>
        public void UpdateUI(CombatantContainer activeContainer)
        {
            BattleCommands.User = activeContainer;
            SkillMenu.User = activeContainer;
            ItemMenu.User = activeContainer;

            BattleCommands.UpdateButtons(activeContainer);
        }
    }
}
