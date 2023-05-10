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
            float random = Random.Range(0, 100);
            return random < percent;
        }

        /// <summary>
        /// 百分率で成否判定を行う
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static bool Percent(float percent)
        {
            float random = GetValueExclusive1(100.0f);
            return random < percent;
        }

        /// <summary>
        /// 0以上1未満の値を取得する
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static float GetValueExclusive1(float scale)
        {
            float value = Random.value;
            while (value == 1.0f)
            {
                value = Random.value;
            }

            value *= scale;
            return value;
        }
    }
}
