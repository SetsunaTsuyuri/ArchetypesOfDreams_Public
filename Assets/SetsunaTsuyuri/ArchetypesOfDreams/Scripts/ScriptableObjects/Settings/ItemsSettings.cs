using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムの設定
    /// </summary>
    [CreateAssetMenu(fileName = "Items", menuName = "Settings/Items")]
    public class ItemsSettings : ScriptableObject
    {
        /// <summary>
        /// 無限に使用可能なアイテムの個数表示
        /// </summary>
        [field: SerializeField]
        public string Infinity { get; private set; } = "∞";
    }
}
