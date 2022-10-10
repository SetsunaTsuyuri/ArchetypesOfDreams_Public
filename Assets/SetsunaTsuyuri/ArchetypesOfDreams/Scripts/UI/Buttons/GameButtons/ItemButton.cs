using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムボタン
    /// </summary>
    public class ItemButton : ActionButton
    {
        /// <summary>
        /// 個数の表示テキスト
        /// </summary>
        [field: SerializeField]
        public TextMeshProUGUI Remaining { get; set; } = null;

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="action">行動内容</param>
        /// <param name="battle">戦闘の管理者</param>
        public override void UpdateButton(ActionModel action, BattleManager battle)
        {
            base.UpdateButton(action, battle);

            // 個数表示
            if (Remaining)
            {
                string text = GameSettings.Items.Infinity;

                // 消費する場合
                if (action.ConsumptionItemdId.HasValue)
                {
                    // 残りの数
                    int remainning = ItemUtility.GetNumberOfItems(action.ConsumptionItemdId.Value);
                    text = remainning.ToString();
                }

                Remaining.text = text;
            }
        }
    }
}
