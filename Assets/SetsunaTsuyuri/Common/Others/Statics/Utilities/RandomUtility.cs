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

        /// <summary>
        /// ランダムな値を取得する
        /// </summary>
        /// <param name="basicValue">基本値</param>
        /// <param name="min">最小倍率</param>
        /// <param name="max">最大倍率</param>
        /// <returns></returns>
        public static int GetRandomValue(int basicValue, float min, float max)
        {
            float multiplier = Random.Range(min, max);
            int result = Mathf.FloorToInt(basicValue * multiplier);

            return result;
        }
    }
}
