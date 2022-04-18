using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// IEnumerableUtility関連の便利な関数集
    /// </summary>
    public static class IEnumerableUtility
    {
        /// <summary>
        /// 条件を満たす要素が何番目にあるかを返す
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="collection">コレクション</param>
        /// <param name="conditon">条件式</param>
        /// <param name="startIndex">検索開始地点</param>
        /// <returns>見つからなければ-1を返す</returns>
        public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> conditon, int startIndex = 0)
        {
            int result = -1;

            T[] array = collection.ToArray();
            for (int i = startIndex; i < array.Length; i++)
            {
                T value = array[i];
                if (conditon(value))
                {
                    result = i;
                    break;
                }
            }

            return result;
        }
    }
}
