using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 計算のユーティリティ
    /// </summary>
    public static class CalculationUtility
    {
        /// <summary>
        /// n%にする
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static int Percent(int value, int percent)
        {
            int result = Mathf.FloorToInt(value * (float)percent / 100);
            return result;
        }

        /// <summary>
        /// かける
        /// </summary>
        /// <param name="value"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static void Multiply(ref int value, float multiplier)
        {
            value = Mathf.FloorToInt(value * multiplier);
        }
    }
}
