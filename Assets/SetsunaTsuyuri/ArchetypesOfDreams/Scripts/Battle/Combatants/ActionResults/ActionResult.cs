using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動結果
    /// </summary>
    public abstract class ActionResult : IInitializable
    {
        public abstract void Initialize();

        /// <summary>
        /// 有効である
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid { get; }
    }
}
