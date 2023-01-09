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
        public void SetUp(BattleManager battle)
        {
            SetUp();

            // イベント追加
            foreach (var button in _buttons)
            {
                button.AddOnClickListener(() => SetInteractable(false));
            }

            // 通常攻撃
            NormalAttack.AddOnClickListener(() =>
            {
                battle.Actor.Combatant.LastSelected = NormalAttack.Button;
            });

            // 浄化
            Purification.AddOnClickListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Purification.Button;
            });

            // 交代
            Change.AddOnClickListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Change.Button;
            });

            // 防御
            Defense.AddOnClickListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Defense.Button;
            });

            // スキル
            Skills.AddOnClickListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Skills.Button;
                battle.BattleUI.Skills.Select();
            });
            Skills.AddTrrigerEntry(EventTriggerType.Select, _ => battle.BattleUI.Description.SetText(GameSettings.Description.Skills));

            // アイテム
            Items.AddOnClickListener(() =>
            {
                battle.Actor.Combatant.LastSelected = Items.Button;
                battle.BattleUI.Items.Select();
            });
            Items.AddTrrigerEntry(EventTriggerType.Select, _ => battle.BattleUI.Description.SetText(GameSettings.Description.Items));

            Hide();
        }

        /// <summary>
        /// 各ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void UpdateButtons(BattleManager battle)
        {
            // 行動者
            Combatant actor = battle.Actor.Combatant;

            // 通常攻撃ボタン
            NormalAttack.UpdateButton(actor.NormalAttack, battle);

            // スキルボタン
            Skills.gameObject.SetActive(actor.HasSkills());
            if (Skills.isActiveAndEnabled)
            {
                Skills.SetInteractable(actor.CanSelectAnySkill(battle));
            }
            else
            {
                Skills.SetInteractable(false);
            }

            // 浄化ボタン
            Purification.gameObject.SetActive(actor is DreamWalker);
            if (Purification.isActiveAndEnabled)
            {
                ActionModel purificationSkill = new(MasterData.GetSkillData(BasicSkillType.Purification));
                Purification.UpdateButton(purificationSkill, battle);
            }
            else
            {
                Purification.SetInteractable(false);
            }

            // 交代ボタン
            Change.gameObject.SetActive(actor is Nightmare);
            if (Change.isActiveAndEnabled)
            {
                ActionModel changeSkill = new(MasterData.GetSkillData(BasicSkillType.Change));
                Change.UpdateButton(changeSkill, battle);
            }
            else
            {
                Change.SetInteractable(false);
            }

            // 防御ボタン
            ActionModel defenseSkill = new(MasterData.GetSkillData(BasicSkillType.Defense));
            Defense.UpdateButton(defenseSkill, battle);

            // アイテムボタン
            Items.SetInteractable(actor.CanSelectAnyItem(battle));

            // ナビゲーション更新
            UpdateButtonNavigationsToLoop();
        }

        /// <summary>
        /// 引数で渡されたものが含まれていればそれを優先して選択する
        /// </summary>
        /// <param name="lastSelected">選択可能なもの</param>
        public void Select(Selectable selectable)
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
