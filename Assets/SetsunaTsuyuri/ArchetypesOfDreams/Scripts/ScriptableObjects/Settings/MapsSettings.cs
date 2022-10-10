using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マップの設定
    /// </summary>
    [CreateAssetMenu(fileName = "Maps", menuName = "Settings/Maps")]
    public class MapsSettings : ScriptableObject
    {
        /// <summary>
        /// マップオブジェクトの移動時間
        /// </summary>
        [field: SerializeField]
        public float ObjectsMoveDuration { get; private set; } = 0.25f;

        /// <summary>
        /// マップオブジェクトの開店時間
        /// </summary>
        [field: SerializeField]
        public float ObjectsRotationDuration { get; private set; } = 0.25f;
    }
}
