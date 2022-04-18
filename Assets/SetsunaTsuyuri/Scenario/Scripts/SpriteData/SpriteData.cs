using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// スプライトデータ
    /// </summary>
    public abstract class SpriteData
    {
        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; set; } = 0;

        /// <summary>
        /// 名前
        /// </summary>
        [field: SerializeField]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 表示される名前
        /// </summary>
        [field: SerializeField]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 基本スプライト
        /// </summary>
        [field: SerializeField]
        public Sprite BasicSprite { get; set; } = null;
    }
}
