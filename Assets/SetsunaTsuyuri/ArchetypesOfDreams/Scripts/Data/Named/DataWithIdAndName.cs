using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// IDと名前と説明のあるデータ
    /// </summary>
    public abstract class DataWithIdAndName : Data
    {
        /// <summary>
        /// 名前
        /// </summary>
        [field: SerializeField]
        public string Name { get; private set; }

        /// <summary>
        /// 説明
        /// </summary>
        [field: SerializeField]
        public string Description { get; private set; }
    }
}
