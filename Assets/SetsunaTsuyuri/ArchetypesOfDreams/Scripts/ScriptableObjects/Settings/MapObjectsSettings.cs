using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マップオブジェクトの設定
    /// </summary>
    [CreateAssetMenu(fileName = "MapObjects", menuName = "Settings/MapObjects")]
    public class MapObjectsSettings : ScriptableObject
    {
        /// <summary>
        /// マップオブジェクトの移動時間
        /// </summary>
        [field: SerializeField]
        public float MoveDuration { get; private set; } = 0.3f;

        /// <summary>
        /// マップオブジェクトの移動イージング
        /// </summary>
        [field: SerializeField]
        public Ease MoveEase { get; private set; } = Ease.Linear;

        /// <summary>
        /// マップオブジェクトの回転時間
        /// </summary>
        [field: SerializeField]
        public float RotationDuration { get; private set; } = 0.3f;

        /// <summary>
        /// マップオブジェクトの回転イージング
        /// </summary>
        [field: SerializeField]
        public Ease RotationEase { get; private set; } = Ease.OutQuad;
    }
}
