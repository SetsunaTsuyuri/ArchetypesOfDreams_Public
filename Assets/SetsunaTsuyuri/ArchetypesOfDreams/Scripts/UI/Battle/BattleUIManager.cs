using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘UIの管理者
    /// </summary>
    public class BattleUIManager : GameUI
    {
        /// <summary>
        /// 説明文のUI
        /// </summary>
        public DescriptionUIManager Description { get; private set; } = null;

        /// <summary>
        /// 味方のUI
        /// </summary>
        public AllyUIManager AlliesUI { get; private set; } = null;

        /// <summary>
        /// 敵のUI
        /// </summary>
        public EnemyUIManager EnemiesUI { get; private set; } = null;

        /// <summary>
        /// 戦闘コマンドボタンの管理者
        /// </summary>
        public BattleCommandsManager BattleCommands { get; private set; } = null;

        /// <summary>
        /// 近接武器スキルボタンの管理者
        /// </summary>
        public MeleeWeaponSkillButtonsManager MeleeWeaponSkillButtons { get; private set; } = null;

        /// <summary>
        /// 遠隔武器スキルボタンの管理者
        /// </summary>
        public RangedWeaponSkillButtonsManager RangedWeaponSkillButtons { get; private set; } = null;

        /// <summary>
        /// 特殊スキルボタンの管理者
        /// </summary>
        public SpecialSkillButtonsManager SpecialSkillButtons { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            Description = GetComponentInChildren<DescriptionUIManager>(true);

            AlliesUI = GetComponentInChildren<AllyUIManager>(true);
            EnemiesUI = GetComponentInChildren<EnemyUIManager>(true);

            BattleCommands = GetComponentInChildren<BattleCommandsManager>(true);
            MeleeWeaponSkillButtons = GetComponentInChildren<MeleeWeaponSkillButtonsManager>(true);
            RangedWeaponSkillButtons = GetComponentInChildren<RangedWeaponSkillButtonsManager>(true);
            SpecialSkillButtons = GetComponentInChildren<SpecialSkillButtonsManager>(true);
        }

        /// <summary>
        /// 各ボタンのセットアップを行う
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void SetUpButtons(BattleManager battle)
        {
            BattleCommands.SetUpButtons(battle);
            BattleCommands.Hide();

            MeleeWeaponSkillButtons.SetUpButtons(battle);
            MeleeWeaponSkillButtons.Hide();

            RangedWeaponSkillButtons.SetUpButtons(battle);
            RangedWeaponSkillButtons.Hide();

            SpecialSkillButtons.SetUpButtons(battle);
            SpecialSkillButtons.Hide();
        }

        /// <summary>
        /// 各ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void UpdateButtons(BattleManager battle)
        {
            BattleCommands.UpdateButtons(battle);
            MeleeWeaponSkillButtons.UpdateButtons(battle);
            RangedWeaponSkillButtons.UpdateButtons(battle);
            SpecialSkillButtons.UpdateButtons(battle);
        }
    }
}
