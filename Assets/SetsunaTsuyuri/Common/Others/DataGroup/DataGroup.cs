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
        where TData : IdData
    {
        /// <summary>
        /// データ
        /// </summary>
        [field: SerializeField]
        public TData[] Data { get; private set; }

        public TData this[int id] => Data.FirstOrDefault(x => x.Id == id);

        /// <summary>
        /// データの数を数える
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Data.Length;
        }
    }
}
