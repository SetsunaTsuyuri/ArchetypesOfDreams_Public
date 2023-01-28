using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// ランダム関連のユーティリティ
    /// </summary>
    public static class RandomUtility
    {
        /// <summary>
        /// 百分率で成否判定する
        /// </summary>
        /// <param name="value">成功率</param>
        /// <returns></returns>
        public static bool JudgeByPercentage(int value)
        {
            return value > Random.Range(0, 100);
        }
    }
}
