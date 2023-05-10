using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 名前と説明のあるデータ
    /// </summary>
    public abstract class NameDescriptionData : NameData
    {
        /// <summary>
        /// 説明
        /// </summary>
        public string Description = string.Empty;
    }
}
