using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 味方のUI
    /// </summary>
    public class AllyUI : CombatantContainerUI
    {
        /// <summary>
        /// Imageの色を変えるもの
        /// </summary>
        public ImageColorChanger ColorChanger { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            ColorChanger = GetComponentInChildren<ImageColorChanger>(true);
        }
    }
}
