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
        /// 指定した型と同じ要素を取得する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetSameType<T>(this IEnumerable<T> source, Type type)
        {
            return source.FirstOrDefault(x => x.GetType() == type);
        }

        /// <summary>
        /// 指定した型と同じ要素が存在する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ExistsSameType<T>(this IEnumerable<T> source, Type type)
        {
            return source.Any(x => x.GetType() == type);
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
    }
}
