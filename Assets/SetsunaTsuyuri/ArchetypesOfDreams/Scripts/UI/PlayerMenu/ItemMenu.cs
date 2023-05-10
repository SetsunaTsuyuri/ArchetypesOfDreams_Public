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
        public override void BeSelected()
        {
            base.BeSelected();

            // 通知
            MessageBrokersManager.ItemMenuSelected.Publish(User);
        }

        protected override void OnActionTargetSelection(TargetSelectionUI targetSelection, int id)
        {
            targetSelection.OnItemTargetSelection(User, id);
        }

        protected override (int, bool)[] GetIdAndCanBeUsed()
        {
            return VariableData.Items.OwnedItemIds
                .Select(x => (x, User.CanUseItem(x)))
                .ToArray();
        }
    }
}
