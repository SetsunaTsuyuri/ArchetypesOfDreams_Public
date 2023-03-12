﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘コマンドボタンの管理者
    /// </summary>
    public class BattleCommandsManager : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// 通常攻撃ボタン
        /// </summary>
        [field: SerializeField]
        public SkillButton NormalAttack { get; private set; } = null;

        /// <summary>
        /// スキルボタン
        /// </summary>
        [field: SerializeField]
        public GameButton Skills { get; private set; } = null;

        /// <summary>
        /// アイテムボタン
        /// </summary>
        [field: SerializeField]
        public GameButton Items { get; private set; } = null;

        /// <summary>
        /// 防御ボタン
        /// </summary>
        [field: SerializeField]
        public SkillButton Defense { get; private set; } = null;

        /// <summary>
        /// 浄化ボタン
        /// </summary>
        [field: SerializeField]
        public SkillButton Purification { get; private set; } = null;

        /// <summary>
        /// 交代ボタン
        /// </summary>
        [field: SerializeField]
        public SkillButton Change { get; private set; } = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="battle">戦闘</param>
        public void SetUp(Battle battle)
        {
            SetUp();

            // UI
            DescriptionUI description = battle.BattleUI.Description;
            SkillMenu skillMenu = battle.BattleUI.SkillMenu;
            ItemMenu itemMenu = battle.BattleUI.ItemMenu; ;

            // イベント追加
            foreach (var button in _buttons)
            {
                button.AddPressedListener(() => SetInteractable(false));
            }

            // 通常攻撃
            SetUpBasicSkillButton(NormalAttack, BasicSkillId.Attack, battle);

            // 防御
            SetUpBasicSkillButton(Defense, BasicSkillId.Defense, battle);

            // 浄化
            SetUpBasicSkillButton(Purification, BasicSkillId.Purification, battle);

            // 交代
            SetUpBasicSkillButton(Change, BasicSkillId.Change, battle);

            // スキル
            Skills.AddPressedListener(() =>
            {
                skillMenu.User = battle.Actor;
                Stack(typeof(SkillMenu));
            });
            Skills.AddTrriger(EventTriggerType.Select, _ => description.SetText(GameSettings.Description.Skills));

            // アイテム
            Items.AddPressedListener(() =>
            {
                itemMenu.User = battle.Actor;
                Stack(typeof(ItemMenu));
            });
            Items.AddTrriger(EventTriggerType.Select, _ => description.SetText(GameSettings.Description.Items));

            Hide();
        }

        /// <summary>
        /// 基本スキルボタンをセットアップする
        /// </summary>
        /// <param name="button"></param>
        /// <param name="basicSkillId"></param>
        /// <param name="battle"></param>
        private void SetUpBasicSkillButton(SkillButton button, BasicSkillId basicSkillId, Battle battle)
        {
            button.SetUp(battle.BattleUI.Description);
            button.AddPressedListener(() =>
            {
                CombatantContainer user = battle.Actor;
                int id = (int)basicSkillId;

                battle.BattleUI.TargetSelection.OnSkillTargetSelection(user, id);
                Stack(typeof(TargetSelectionUI));
                Hide();
            });
        }

        /// <summary>
        /// 各ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘</param>
        public void UpdateButtons(Battle battle)
        {
            // 行動者
            CombatantContainer actor = battle.Actor;
            Combatant combatant = battle.Actor.Combatant;

            // 通常攻撃ボタン
            {
                int id = (int)BasicSkillId.Attack;
                bool canBeUsed = actor.CanUseSkill(id);
                NormalAttack.UpdateButton(id, canBeUsed);
            }

            // スキルボタン
            Skills.SetInteractable(actor.CanUseAnySkill());

            // 浄化ボタン
            Purification.gameObject.SetActive(combatant is DreamWalker);
            if (Purification.isActiveAndEnabled)
            {
                int id = (int)BasicSkillId.Purification;
                bool canBeUsed = actor.CanUseSkill(id);
                Purification.UpdateButton(id, canBeUsed);
            }
            else
            {
                Purification.SetInteractable(false);
            }

            // 交代ボタン
            Change.gameObject.SetActive(combatant is Nightmare);
            if (Change.isActiveAndEnabled)
            {
                int id = (int)BasicSkillId.Change;
                bool canBeUsed = actor.CanUseSkill(id);
                Change.UpdateButton(id, canBeUsed);
            }
            else
            {
                Change.SetInteractable(false);
            }

            // 防御ボタン
            {
                int id = (int)BasicSkillId.Defense;
                bool canBeUsed = actor.CanUseSkill(id);
                Defense.UpdateButton(id, canBeUsed);
            }

            // アイテムボタン
            Items.SetInteractable(actor.CanUseAnyItem());

            // ナビゲーション更新
            UpdateButtonNavigations();
        }
    }
}
