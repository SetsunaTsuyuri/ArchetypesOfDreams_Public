using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// スキルボタン
    /// </summary>
    public class SkillButton : ActionButton
    {
        /// <summary>
        /// コストの表示テキスト
        /// </summary>
        [field: SerializeField]
        public TextMeshProUGUI Cost { get; set; } = null;

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="action">行動</param>
        /// <param name="battle">戦闘の管理者</param>
        public override void UpdateButton(ActionModel action, BattleManager battle)
        {
            base.UpdateButton(action, battle);

            // コスト表示
            if (Cost)
            {
                Cost.text = action.ConsumptionDp.ToString();
            }
        }
    }
}
