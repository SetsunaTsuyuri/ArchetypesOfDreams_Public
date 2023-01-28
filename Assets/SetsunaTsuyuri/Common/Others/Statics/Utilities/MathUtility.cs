using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 数学のユーティリティ
    /// </summary>
    public static class MathUtility
    {
        /// <summary>
        /// n%にする
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static int Percent(int value, float percent)
        {
            int result = Mathf.FloorToInt(value * percent / 100);
            return result;
        }

        /// <summary>
        /// n%増やす
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static void IncreaseBy(ref int value, float percent)
        {
            value = Mathf.FloorToInt(value * (100 + percent) / 100);
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
