using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 乱数のユーティリティ
    /// </summary>
    public static class RandomUtility
    {
        /// <summary>
        /// 百分率で成否判定する
        /// </summary>
        /// <param name="percent">成功率</param>
        /// <returns></returns>
        public static bool Percent(int percent)
        {
            return Random.Range(0, 100) < percent;
        }
    }
}
