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
        /// 近接武器スキルボタン
        /// </summary>
        [field: SerializeField]
        public GameButton MeleeWeaponSkills { get; private set; } = null;

        /// <summary>
        /// 遠隔武器スキルボタン
        /// </summary>
        [field: SerializeField]
        public GameButton RangedWeaponSkills { get; private set; } = null;

        /// <summary>
        /// 特殊スキルボタン
        /// </summary>
        [field: SerializeField]
        public GameButton SpecialSkills { get; private set; } = null;

        /// <summary>
        /// 防御ボタン
        /// </summary>
        [field: SerializeField]
        public SkillButton Defending { get; private set; } = null;

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

        ///// <summary>
        ///// アイテムボタン
        ///// </summary>
        //[SerializeField]
        //GameButton item = null;

        /// <summary>
        /// ボタンをセットアップする
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void SetUpButtons(BattleManager battle)
        {
            // イベント追加
            foreach (var button in buttons)
            {
                button.AddOnClickListener(() => SetInteractable(false));
            }

            // 近接武器スキル
            MeleeWeaponSkills.Button.onClick.AddListener(() =>
            {
                battle.Performer.Combatant.LastSelected = MeleeWeaponSkills.Button;
                battle.BattleUI.MeleeWeaponSkillButtons.Select();
            });
            MeleeWeaponSkills.AddTrriger(EventTriggerType.Select,(_) => battle.BattleUI.Description.SetText(GameSettings.Description.MeleeWeaponSkills));

            // 遠隔武器スキル
            RangedWeaponSkills.Button.onClick.AddListener(() =>
            {
                battle.Performer.Combatant.LastSelected = RangedWeaponSkills.Button;
                battle.BattleUI.RangedWeaponSkillButtons.Select();
            });
            RangedWeaponSkills.AddTrriger(EventTriggerType.Select, (_) => battle.BattleUI.Description.SetText(GameSettings.Description.RangedWeaponSkills));

            // 特殊スキル
            SpecialSkills.Button.onClick.AddListener(() =>
            {
                battle.Performer.Combatant.LastSelected = SpecialSkills.Button;
                battle.BattleUI.SpecialSkillButtons.Select();
            });
            SpecialSkills.AddTrriger(EventTriggerType.Select, (_) => battle.BattleUI.Description.SetText(GameSettings.Description.SpecialSkills));

            // 浄化
            Purification.Button.onClick.AddListener(() =>
            {
                battle.Performer.Combatant.LastSelected = Purification.Button;
            });

            // 交代
            Change.Button.onClick.AddListener(() =>
            {
                battle.Performer.Combatant.LastSelected = Change.Button;
            });

            // 防御
            Defending.Button.onClick.AddListener(() =>
            {
                battle.Performer.Combatant.LastSelected = Defending.Button;
            });
        }

        /// <summary>
        /// 各ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void UpdateButtons(BattleManager battle)
        {
            // 行動する戦闘者
            Combatant combatant = battle.Performer.Combatant;

            // 近接武器スキルボタン
            MeleeWeaponSkills.gameObject.SetActive(combatant.HasMeleeWeaponSkills());
            if (MeleeWeaponSkills.gameObject.activeSelf)
            {
                MeleeWeaponSkills.SetInteractable(combatant.CanSelectAnyMeleeWeaponSkills(battle));
            }
            else
            {
                MeleeWeaponSkills.SetInteractable(false);
            }

            // 遠隔武器スキルボタン
            RangedWeaponSkills.gameObject.SetActive(combatant.HasRangedWeaponSkills());
            if (RangedWeaponSkills.gameObject.activeSelf)
            {
                RangedWeaponSkills.SetInteractable(combatant.CanSelectAnyRangedWeaponSkills(battle));
            }
            else
            {
                RangedWeaponSkills.SetInteractable(false);
            }

            // 特殊スキルボタン
            SpecialSkills.gameObject.SetActive(combatant.HasSpecialSkills());
            if (SpecialSkills.gameObject.activeSelf)
            {
                SpecialSkills.SetInteractable(combatant.CanSelectAnySpecialSkills(battle));
            }
            else
            {
                SpecialSkills.SetInteractable(false);
            }

            // 浄化ボタン
            Purification.gameObject.SetActive(combatant is DreamWalker);
            if (Purification.gameObject.activeSelf)
            {
                Skill purificationSkill = new Skill(combatant, MasterData.CommonSkills.Purification);
                Purification.SetUp(purificationSkill, battle);
            }
            else
            {
                Purification.SetInteractable(false);
            }

            // 交代ボタン
            Change.gameObject.SetActive(combatant is Nightmare);
            if (Change.gameObject.activeSelf)
            {
                Skill changeSkill = new Skill(combatant, MasterData.CommonSkills.Change);
                Change.SetUp(changeSkill, battle);
            }
            else
            {
                Change.SetInteractable(false);
            }

            // 防御ボタン
            Skill defendingSkill = new Skill(combatant, MasterData.CommonSkills.Defending);
            Defending.SetUp(defendingSkill, battle);

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
                foreach (var button in buttons)
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
