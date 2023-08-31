using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 配列の拡張メソッド
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 1次元配列を2次元配列にする
        /// </summary>
        /// <param name="array"></param>
        /// <param name="row">行 (Y座標)</param>
        /// <param name="column">列 (X座標)</param>
        /// <returns></returns>
        public static T[,] ToArray2D<T>(this T[] array, int row, int column)
        {
            T[,] result = new T[row, column];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    result[i, j] = array[i * column + j];
                }
            }

            return result;
        }
    }
}
