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
    public class SkillButtonsManager : ActionButtonsManager<SkillButton>
    {
        /// <summary>
        /// ボタンを更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public override void UpdateButtons(BattleManager battle)
        {
            base.UpdateButtons(battle);

            Combatant playerControlled = battle.Actor.Combatant;
            ActionModel[] skills = playerControlled.Skills;

            for (int i = 0; i < skills.Length && i < _buttons.Length; i++)
            {
                ActionModel skill = skills[i];
                SkillButton button = _buttons[i];

                // ボタンのセットアップ
                button.UpdateButton(skill, battle);

                // ボタンを表示する
                button.Show();
            }

            // ナビゲーション更新
            UpdateButtonNavigationsToLoop();
        }
    }
}
