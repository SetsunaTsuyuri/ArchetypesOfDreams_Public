using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 名前と説明のあるデータ
    /// </summary>
    public abstract class NameDescriptionData : Data
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// 説明
        /// </summary>
        public string Description = string.Empty;
    }
}
