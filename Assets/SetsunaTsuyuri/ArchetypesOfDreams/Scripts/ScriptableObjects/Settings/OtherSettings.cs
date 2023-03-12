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
        /// 精貨の最大数
        /// </summary>
        [field: SerializeField]
        public int MaxSpiritCoins { get; private set; } = 999999999;
    }
}
