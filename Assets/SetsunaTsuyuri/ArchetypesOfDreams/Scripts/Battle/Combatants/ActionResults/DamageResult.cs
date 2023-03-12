using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダメージの行動結果
    /// </summary>
    public class DamageResult : DamageOrHealingResult
    {
        /// <summary>
        /// クリティカル発生
        /// </summary>
        public bool IsCritical { get; set; } = false;

        /// <summary>
        /// クラッシュ発生
        /// </summary>
        public bool IsCrush { get; set; } = false;

        public override void Initialize()
        {
            base.Initialize();

            IsCritical = false;
            IsCrush = false;
        }
    }
}
