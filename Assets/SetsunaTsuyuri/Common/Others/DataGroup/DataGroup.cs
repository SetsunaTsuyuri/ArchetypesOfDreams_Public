using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// データグループ
    /// </summary>
    /// <typeparam name="TData">データの型</typeparam>
    public abstract class DataGroup<TData> : ScriptableObject
        where TData : Data
    {
        /// <summary>
        /// データ
        /// </summary>
        [field: SerializeField]
        public TData[] Data { get; private set; }

        /// <summary>
        /// インデクサー
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public TData this[int id] => Data[id];

        /// <summary>
        /// データの数を数える
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Data.Length;
        }

        protected virtual void OnValidate()
        {
            // IDを設定する
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i].Id = i;
            }
        }
    }
}
