using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// スプライトデータの配列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SpriteDataCollection<T> : ScriptableObject where T : SpriteData
    {
        [field: SerializeField]
        public T[] Data { get; set; }

        /// <summary>
        /// スプライトデータを取得する
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public T GetDataOrDefault(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                return null;
            }

            return Data[index];
        }

        /// <summary>
        /// スプライトデータを取得する
        /// </summary>
        /// <param name="name">データの名前</param>
        /// <returns></returns>
        public T GetDataOrDefault(string name)
        {
            T data = Data.FirstOrDefault(d => d.Name == name);
            return data;
        }
    }
}
