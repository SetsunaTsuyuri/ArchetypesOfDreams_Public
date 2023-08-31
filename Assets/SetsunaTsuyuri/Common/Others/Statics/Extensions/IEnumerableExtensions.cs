using System;
using System.Collections.Generic;
using System.Linq;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// IEnumerableの拡張メソッド
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 無作為に並び変える
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> OrderByRandom<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(_ => Guid.NewGuid());
        }

        /// <summary>
        /// 無作為に並び変える
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> ThenByRandom<T>(this IOrderedEnumerable<T> source)
        {
            return source.ThenBy(_ => Guid.NewGuid());
        }

        /// <summary>
        /// 範囲外を指定している
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool OutOfRange<T>(this IEnumerable<T> source, int index)
        {
            return index < 0 || index >= source.Count();
        }

        /// <summary>
        /// 指定した要素または初期値を取得する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetValueOrDefault<T>(this IEnumerable<T> source, int index)
        {
            T result = default;
            if (!source.OutOfRange(index))
            {
                result = source.ElementAt(index);
            }

            return result;
        }

        /// <summary>
        /// 指定した要素を取得する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue<T>(this IEnumerable<T> source, int index, out T value)
        {
            value = default;
            bool result = false;

            if (!source.OutOfRange(index))
            {
                value = source.ElementAt(index);
                result = true;
            }

            return result;
        }

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
