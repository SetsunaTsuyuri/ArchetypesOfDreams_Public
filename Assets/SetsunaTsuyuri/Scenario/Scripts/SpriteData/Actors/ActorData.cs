using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// 出演者のデータ
    /// </summary>
    [System.Serializable]
    public class ActorData : SpriteData
    {
        /// <summary>
        /// 表情
        /// </summary>
        [field: SerializeField]
        public KeysAndValues<Attribute.Expression, Sprite> Expressions { get; private set; } = null;

        /// <summary>
        /// 拡大率
        /// </summary>
        [field: SerializeField]
        public float Scale { get; private set; } = 1.0f;

        /// <summary>
        /// 表示位置オフセット
        /// </summary>
        [field: SerializeField]
        public Vector2 PositionOffset { get; private set; } = Vector2.zero;
    }
}
