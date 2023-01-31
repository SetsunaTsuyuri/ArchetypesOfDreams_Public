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
            UpdateTexts(item, () => ItemUtility.GetNumberOfItems(id).ToString());
        }
    }
}
