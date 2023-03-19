using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ルートUI
    /// </summary>
    public abstract class UIManager : GameUI
    {
        /// <summary>
        /// UI選択履歴
        /// </summary>
        protected UISelectionHistroy History = null;

        protected override void Awake()
        {
            base.Awake();

            SelectableGameUIBase[] selectables = GetComponentsInChildren<SelectableGameUIBase>();
            History = new UISelectionHistroy(selectables);
        }
    }
}
