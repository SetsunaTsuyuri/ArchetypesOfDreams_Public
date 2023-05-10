using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムデータ
    /// </summary>
    [System.Serializable]
    public class ItemData : EffectData
    {
        /// <summary>
        /// 価格
        /// </summary>
        public int Price = 0;

        /// <summary>
        /// 使ってもなくならない
        /// </summary>
        public bool IsReusable = false;
    }
}
