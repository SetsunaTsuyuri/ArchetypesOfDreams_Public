using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 効果アイテムのデータ
    /// </summary>
    [System.Serializable]
    public class EffectItemData : ItemData
    {
        /// <summary>
        /// 使ってもなくならない
        /// </summary>
        [field: SerializeField]
        public bool IsReusable { get; private set; }

        /// <summary>
        /// 行動内容
        /// </summary>
        [field : SerializeField]
        public EffectData Effect { get; private set; }
    }
}
