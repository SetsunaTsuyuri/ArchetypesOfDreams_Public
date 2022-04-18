using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 視覚効果の設定
    /// </summary>
    [CreateAssetMenu(menuName = "Settings/VisualEffects")]
    public class VisualEffectsSettings : ScriptableObject
    {
        /// <summary>
        /// 対象に選択された味方UIの色
        /// </summary>
        [field: SerializeField]
        public Color TargetedAllyUIColor { get; private set; } = Color.yellow;

        /// <summary>
        /// 対象に選択された敵の色
        /// </summary>
        [field: SerializeField]
        public Color TargetedEnemyColor { get; private set; } = Color.gray;

        /// <summary>
        /// 味方の枠色
        /// </summary>
        [field: SerializeField]
        public Color AllyFrameColor { get; private set; } = Color.blue;

        /// <summary>
        /// 敵の枠色
        /// </summary>
        [field: SerializeField]
        public Color EnemyFrameColor { get; private set; } = Color.red;

        /// <summary>
        /// 対象に選択された敵の色変化にかかる時間
        /// </summary>
        [field: SerializeField]
        public float TargetedEnemyColorChangeDuration { get; private set; } = 0.5f;

        /// <summary>
        /// 敵消滅時の色
        /// </summary>
        [field: SerializeField]
        public Color DefeatedEnemyColor { get; private set; } = new Color(1.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        /// 敵のフェード時間
        /// </summary>
        [field: SerializeField]
        public float EnemyFadeDuration { get; private set; } = 0.5f;
    }
}
