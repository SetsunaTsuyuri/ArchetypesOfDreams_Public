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
        /// ブレイク発生
        /// </summary>
        public bool IsBreak { get; set; } = false;

        public override void Initialize()
        {
            base.Initialize();

            IsCritical = false;
            IsBreak = false;
        }
    }
}
