using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// その他の設定
    /// </summary>
    [CreateAssetMenu(fileName = "Other", menuName = "Settings/Other")]
    public class OtherSettings : ScriptableObject
    {
        /// <summary>
        /// 精気の最大値
        /// </summary>
        [field: SerializeField]
        public int MaxEnergy { get; private set; } = 100;

        /// <summary>
        /// 歩数の最大値
        /// </summary>
        [field: SerializeField]
        public int MaxSteps { get; private set; } = 999999999;
    }
}
