using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// スプライトデータグループ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SpriteDataGroup<T> : ScriptableObject where T : SpriteData
    {
        [field: SerializeField]
        public T[] Data { get; private set; }

        /// <summary>
        /// スプライトデータを取得する
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public T GetDataOrDefault(int id)
        {
            return Data.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// スプライトデータを取得する
        /// </summary>
        /// <param name="name">データの名前</param>
        /// <returns></returns>
        public T GetDataOrDefault(string name)
        {
            return Data.FirstOrDefault(d => d.Name == name);
        }
    }
}
