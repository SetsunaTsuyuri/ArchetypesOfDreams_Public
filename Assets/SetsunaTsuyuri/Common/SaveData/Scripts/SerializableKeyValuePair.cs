using System;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// シリアライズ可能なKeyValuePair
    /// </summary>
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        /// <summary>
        /// キー
        /// </summary>
        [field: SerializeField]
        public TKey Key { get; set; } = default;

        /// <summary>
        /// 値
        /// </summary>
        [field: SerializeField]
        public TValue Value { get; set; } = default;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
