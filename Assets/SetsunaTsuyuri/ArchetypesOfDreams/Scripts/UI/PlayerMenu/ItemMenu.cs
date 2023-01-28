using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムメニュー
    /// </summary>
    public class ItemMenu : ActionMenu<ItemButton>
    {
        protected override void OnActionTargetSelection(TargetSelectionUI targetSelection, int id)
        {
            targetSelection.OnItemTargetSelection(User, id);
        }

        protected override (int, bool)[] GetIdAndCanBeUsed()
        {
            return ItemUtility.GetOwnedItemIds()
                .Select(x => (x, User.CanUseItem(x)))
                .ToArray();
        }
    }
}
