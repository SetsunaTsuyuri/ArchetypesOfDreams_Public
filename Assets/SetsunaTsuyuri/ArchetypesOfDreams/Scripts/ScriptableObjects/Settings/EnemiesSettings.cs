using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵の設定
    /// </summary>
    [CreateAssetMenu(fileName = "Enemies", menuName = "Settings/Enemies")]
    public class EnemiesSettings : ScriptableObject
    {
        /// <summary>
        /// UIの表示位置オフセット
        /// </summary>
        [field: SerializeField]
        public Vector2 UIPositionOffset { get; private set; } = new Vector2(0.0f, 0.1f);

        /// <summary>
        /// 敵ダメージ時の振動する時間
        /// </summary>
        [field: SerializeField]
        public float DamageShakeDuration { get; private set; } = 0.2f;

        /// <summary>
        /// 敵ダメージ時の振動する強さ
        /// </summary>
        [field: SerializeField]
        public float DamageShakeStrength { get; private set; } = 0.1f;

        /// <summary>
        /// 敵ダメージ時の振動する回数
        /// </summary>
        [field: SerializeField]
        public int DamageShakeVibrato { get; private set; } = 25;
    }
}
