using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムデータ
    /// </summary>
    public abstract class ItemData : DataWithIdAndName
    {
        /// <summary>
        /// 価格
        /// </summary>
        [field: SerializeField]
        public int Price { get; private set; }
    }
}
