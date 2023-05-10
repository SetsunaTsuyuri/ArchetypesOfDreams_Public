using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// カメラの設定
    /// </summary>
    [CreateAssetMenu(fileName = "Cameras", menuName = "Settings/Cameras")]
    public class CamerasSettings : ScriptableObject
    {
        /// <summary>
        /// 振動の強さ
        /// </summary>
        [field: SerializeField]
        public float ShakeStrength { get; private set; } = 0.3f;

        /// <summary>
        /// 振動する回数
        /// </summary>
        [field: SerializeField]
        public int ShakeVibrato { get; private set; } = 30;
    }
}
