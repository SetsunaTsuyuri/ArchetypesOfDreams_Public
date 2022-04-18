using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 疑似乱数関連の便利な関数集
    /// </summary>
    public static class RandomUtility
    {
        /// <summary>
        /// 百分率で成否判定する
        /// </summary>
        /// <param name="value">値</param>
        /// <returns></returns>
        public static bool JudgeByPercentage(int value)
        {
            return value > Random.Range(0, 100);
        }
    }
}
