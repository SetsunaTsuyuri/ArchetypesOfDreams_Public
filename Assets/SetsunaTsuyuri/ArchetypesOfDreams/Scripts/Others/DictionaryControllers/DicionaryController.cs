﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ディクショナリーコントローラー
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class DicionaryController<TKey, TValue> : IInitializable
    {
        protected Dictionary<TKey, TValue> Dictionary = new();

        public virtual void Initialize()
        {
            Dictionary.Clear();
        }

        /// <summary>
        /// 取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TValue Get(TKey id)
        {
            TValue result = Dictionary.GetValueOrDefault(id, default);

            if (Dictionary.TryGetValue(id, out TValue value))
            {
                result = value;
            }

            return result;
        }

        /// <summary>
        /// 設定する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Set(TKey id, TValue value)
        {
            if (!Dictionary.ContainsKey(id))
            {
                return;
            }

            Dictionary[id] = value;
        }

        /// <summary>
        /// SerializableKeyValuePair配列にセーブする
        /// </summary>
        /// <returns></returns>
        public SerializableKeyValuePair<TKey, TValue>[] ToSerializableKeyValuePair()
        {
            KeyValuePair<TKey, TValue>[] pairs = Dictionary.ToArray();
            SerializableKeyValuePair<TKey, TValue>[] serializablePairs = new SerializableKeyValuePair<TKey, TValue>[pairs.Length];
            
            for (int i = 0; i < serializablePairs.Length; i++)
            {
                SerializableKeyValuePair<TKey, TValue> serializablePair = new(pairs[i].Key, pairs[i].Value);
                serializablePairs[i] = serializablePair;
            }

            return serializablePairs;
        }

        /// <summary>
        /// SerializableKeyValuePair配列からロードする
        /// </summary>
        /// <param name="pairs"></param>
        public void FromSerializableKeyValuePair(SerializableKeyValuePair<TKey, TValue>[] pairs)
        {
            Initialize();
            foreach (var pair in pairs)
            {
                // キーが登録されている場合のみ値を上書きする
                if (Dictionary.ContainsKey(pair.Key))
                {
                    Dictionary[pair.Key] = pair.Value;
                }
            }
        }
    }
}
