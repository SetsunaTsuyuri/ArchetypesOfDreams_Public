using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// スキルボタンの管理者
    /// </summary>
    public class SkillButtonsManager : SelectableGameUI<SkillButton>
    {
        /// <summary>
        /// ボタンをセットアップする
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void SetUpButtons(BattleManager battle)
        {
            SetUpButtons();

            for (int i = 0; i < buttons.Length; i++)
            {
                // スキルボタン
                SkillButton button = buttons[i];

                // イベントリスナー登録
                button.Button.onClick.AddListener(() => Hide());

                // イベントトリガー登録
                button.AddTrriger(EventTriggerType.Cancel,
                    (_) => Hide());
                button.AddTrriger(EventTriggerType.Cancel,
                    (_) => battle.BattleUI.BattleCommands.Select(battle.Performer.Combatant.LastSelected));

                // ボタン非アクティブ化
                SetActiveToAllButtons(false);
            }
        }

        /// <summary>
        /// ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void UpdateButtons(BattleManager battle)
        {
            Combatant playerControlled = battle.Performer.Combatant;
            Skill[] skills = GetSkills(playerControlled);

            // 一旦、全てのボタンを非アクティブにする
            SetActiveToAllButtons(false);

            for (int i = 0; i < skills.Length; i++)
            {
                Skill skill = skills[i];
                SkillButton button = buttons[i];

                // ボタンのセットアップ
                button.SetUp(skill, battle);

                // ボタンアクティブ化
                button.gameObject.SetActive(true);
            }

            // ナビゲーション更新
            UpdateButtonNavigationsToLoop();
        }

        /// <summary>
        /// スキル配列を取得する
        /// </summary>
        /// <param name="combatant">戦闘者</param>
        /// <returns></returns>
        protected virtual Skill[] GetSkills(Combatant combatant)
        {
            return combatant.Skills;
        }
    }
}
