using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 名前のあるデータ
    /// </summary>
    public abstract class NameData : IdData
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name = string.Empty;
    }
}
