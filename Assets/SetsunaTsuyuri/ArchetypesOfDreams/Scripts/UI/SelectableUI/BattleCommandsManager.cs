using System.Collections;
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
        /// 逃走ボタン
        /// </summary>
        [field: SerializeField]
        public SkillButton Escape { get; private set; } = null;

        /// <summary>
        /// 使用者
        /// </summary>
        public CombatantContainer User { get; set; } = null;

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

            // 逃走
            SetUpBasicSkillButton(Escape, BasicSkillId.Escape, battle);

            // スキル
            Skills.AddPressedListener(() =>
            {
                skillMenu.User = battle.ActiveContainer;
                Stack(typeof(SkillMenu));
            });
            Skills.AddTrriger(EventTriggerType.Select, _ => description.SetText(GameSettings.Description.Skills));

            // アイテム
            Items.AddPressedListener(() =>
            {
                itemMenu.User = battle.ActiveContainer;
                Stack(typeof(ItemMenu));
            });
            Items.AddTrriger(EventTriggerType.Select, _ => description.SetText(GameSettings.Description.Items));

            SetEnabled(false);
        }

        public override void BeSelected()
        {
            base.BeSelected();

            // 通知
            MessageBrokersManager.BattleCommandsSelected.Publish(User);
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
                CombatantContainer user = battle.ActiveContainer;
                int id = (int)basicSkillId;

                battle.BattleUI.TargetSelection.OnSkillTargetSelection(user, id);
                Stack(typeof(TargetSelectionUI));
            });
        }

        /// <summary>
        /// 各ボタンを更新する
        /// </summary>
        /// <param name="battle"></param>
        public void UpdateButtons(Battle battle)
        {
            CombatantContainer activeContainer = battle.ActiveContainer;

            // 通常攻撃
            {
                int id = (int)BasicSkillId.Attack;
                bool canBeUsed = activeContainer.CanUseSkill(id);
                NormalAttack.UpdateButton(id, canBeUsed);
            }

            // スキル
            Skills.gameObject.SetActive(activeContainer.HasAnySkill);
            Skills.SetInteractable(Skills.isActiveAndEnabled);

            // 浄化
            Purification.gameObject.SetActive(activeContainer.Combatant is DreamWalker);
            if (Purification.isActiveAndEnabled)
            {
                int id = (int)BasicSkillId.Purification;
                bool canBeUsed = activeContainer.CanUseSkill(id);
                Purification.UpdateButton(id, canBeUsed);
            }
            else
            {
                Purification.SetInteractable(false);
            }

            // 交代
            Change.gameObject.SetActive(activeContainer.Combatant is Nightmare);
            if (Change.isActiveAndEnabled)
            {
                int id = (int)BasicSkillId.Change;
                bool canBeUsed = activeContainer.CanUseSkill(id);
                Change.UpdateButton(id, canBeUsed);
            }
            else
            {
                Change.SetInteractable(false);
            }

            // 防御
            {
                int id = (int)BasicSkillId.Defense;
                bool canBeUsed = activeContainer.CanUseSkill(id);
                Defense.UpdateButton(id, canBeUsed);
            }

            // アイテム
            Items.SetInteractable(activeContainer.CanUseAnyItem());

            // 逃走
            {
                int id = (int)BasicSkillId.Escape;
                bool canBeUsed = activeContainer.CanUseSkill(id) && battle.AlliesCanEscape;
                Escape.UpdateButton(id, canBeUsed);
            }

            // ナビゲーション更新
            UpdateButtonNavigations();
        }
    }
}
