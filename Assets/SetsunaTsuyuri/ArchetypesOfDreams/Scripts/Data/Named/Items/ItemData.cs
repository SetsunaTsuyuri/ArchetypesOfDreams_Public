using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムデータ
    /// </summary>
    [System.Serializable]
    public class ItemData : DataWithIdAndName
    {
        /// <summary>
        /// 価格
        /// </summary>
        [field: SerializeField]
        public int Price { get; private set; }

        /// <summary>
        /// 使ってもなくならない
        /// </summary>
        [field: SerializeField]
        public bool IsReusable { get; private set; }

        /// <summary>
        /// 効果データ
        /// </summary>
        [field: SerializeField]
        public EffectData Effect { get; private set; }
    }
}
