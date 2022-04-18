using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// IDと名前のあるデータ
    /// </summary>
    public abstract class DataWithIdAndName
    {
        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; private set; } = 0;

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
