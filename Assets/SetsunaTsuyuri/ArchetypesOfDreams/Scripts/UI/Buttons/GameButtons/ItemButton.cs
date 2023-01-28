using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムボタン
    /// </summary>
    public class ItemButton : ActionButton
    {
        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="canBeUsed">使用できる</param>
        public override void UpdateButton(int id, bool canBeUsed)
        {
            base.UpdateButton(id, canBeUsed);

            ItemData item = MasterData.GetItemData(id);
            Description = item.Description;

            if (Name)
            {
                Name.text = item.Name;
            }

            if (Number)
            {
                Number.text = VariableData.ItemsDic[Id].ToString();
            }
        }

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="action">行動内容</param>
        /// <param name="battle">戦闘の管理者</param>
        public override void UpdateButton(ActionInfo action, Battle battle)
        {
            base.UpdateButton(action, battle);

            // 個数表示
            if (Number)
            {
                string text = GameSettings.Items.Infinity;

                // 消費する場合
                if (action.ConsumptionItemdId.HasValue)
                {
                    // 残りの数
                    int remainning = ItemUtility.GetNumberOfItems(action.ConsumptionItemdId.Value);
                    text = remainning.ToString();
                }

                Number.text = text;
            }
        }
    }
}
