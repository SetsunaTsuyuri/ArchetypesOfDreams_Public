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
        /// セットアップする
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void SetUp(Battle battle)
        {
            SetUp();

            // イベント追加
            foreach (var button in _buttons)
            {
                button.AddPressedListener(() => SetInteractable(false));
            }

            // 通常攻撃
            NormalAttack.AddPressedListener(() =>
            {
                battle.Actor.Combatant.LastSelected = NormalAttack.Button;
            });

            // 浄化
            Purification.AddPressedListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Purification.Button;
            });

            // 交代
            Change.AddPressedListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Change.Button;
            });

            // 防御
            Defense.AddPressedListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Defense.Button;
            });

            // スキル
            Skills.AddPressedListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Skills.Button;
                battle.BattleUI.Skills.BeSelected();
            });
            Skills.AddTrriger(EventTriggerType.Select, _ => battle.BattleUI.Description.SetText(GameSettings.Description.Skills));

            // アイテム
            Items.AddPressedListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Items.Button;
                battle.BattleUI.Items.BeSelected();
            });
            Items.AddTrriger(EventTriggerType.Select, _ => battle.BattleUI.Description.SetText(GameSettings.Description.Items));

            Hide();
        }

        /// <summary>
        /// 各ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void UpdateButtons(Battle battle)
        {
            // 行動者
            CombatantContainer actor = battle.Actor;
            Combatant combatant = battle.Actor.Combatant;

            // 通常攻撃ボタン
            {
                int id = (int)BasicSkillType.Attack;
                bool canBeUsed = actor.CanUseSkill(id);
                //NormalAttack.UpdateButton(id, canBeUsed);
                NormalAttack.UpdateButton(ActionInfo.CreateSkillAction(id), battle);
            }

            // スキルボタン
            Skills.SetInteractable(actor.CanUseAnySkill());

            // 浄化ボタン
            Purification.gameObject.SetActive(combatant is DreamWalker);
            if (Purification.isActiveAndEnabled)
            {
                int id = (int)BasicSkillType.Purification;
                bool canBeUsed = actor.CanUseSkill(id);
                Purification.UpdateButton(ActionInfo.CreateSkillAction(id), battle);
                //Purification.UpdateButton(id, canBeUsed);
            }
            else
            {
                Purification.SetInteractable(false);
            }

            // 交代ボタン
            Change.gameObject.SetActive(combatant is Nightmare);
            if (Change.isActiveAndEnabled)
            {
                int id = (int)BasicSkillType.Change;
                bool canBeUsed = actor.CanUseSkill(id);
                Change.UpdateButton(ActionInfo.CreateSkillAction(id), battle);
                //Change.UpdateButton(id, canBeUsed);
            }
            else
            {
                Change.SetInteractable(false);
            }

            // 防御ボタン
            {
                int id = (int)BasicSkillType.Defense;
                bool canBeUsed = actor.CanUseSkill(id);
                Defense.UpdateButton(ActionInfo.CreateSkillAction(id), battle);
                //Defense.UpdateButton(id, canBeUsed);
            }

            // アイテムボタン
            Items.SetInteractable(actor.CanUseAnyItem());

            // ナビゲーション更新
            UpdateButtonNavigations();
        }

        /// <summary>
        /// 引数で渡されたものが含まれていればそれを優先して選択する
        /// </summary>
        /// <param name="lastSelected">選択可能なもの</param>
        public void BeSelected(Selectable selectable)
        {
            Show();

            if (selectable &&
                selectable.isActiveAndEnabled &&
                selectable.interactable)
            {
                selectable.Select();
            }
            else
            {
                foreach (var button in _buttons)
                {
                    if (button.Button.interactable)
                    {
                        button.Button.Select();
                        break;
                    }
                }
            }
        }
    }
}
