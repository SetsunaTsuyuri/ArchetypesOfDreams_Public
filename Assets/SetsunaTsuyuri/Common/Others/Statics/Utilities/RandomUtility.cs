using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// 重み付け確率抽選を行う
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T Weighted<T>(T[] array, System.Func<T, float> func)
        {
            float[] weights = array
                .Select(x => func(x))
                .ToArray();

            int index = Weighted(weights);
            return array[index];
        }

        /// <summary>
        /// 重み付け確率抽選を行う
        /// </summary>
        /// <param name="weights"></param>
        /// <returns></returns>
        public static int Weighted(float[] weights)
        {
            float[] sums = CreateSums(weights);
            
            int result = LotteryIndex(sums);
            
            return result;
        }

        /// <summary>
        /// 重み付け確率抽選を行う
        /// </summary>
        /// <param name="weights"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int[] Weighted(float[] weights, int number)
        {
            float[] sums = CreateSums(weights);

            int[] reuslts = new int[number];
            for (int i = 0; i < number; i++)
            {
                reuslts[i] = LotteryIndex(sums);
            }

            return reuslts;

        }

        /// <summary>
        /// 重みの総和配列を作る
        /// </summary>
        /// <param name="weights"></param>
        /// <returns></returns>
        private static float[] CreateSums(float[] weights)
        {
            // 重みの総和
            float sum = 0.0f;

            // 各要素までの重みの総和配列
            float[] sums = new float[weights.Length];

            for (int i = 0; i < sums.Length; i++)
            {
                sum += weights[i];
                sums[i] = sum;
            }

            return sums;
        }

        /// <summary>
        /// 重みの総和配列から抽選する
        /// </summary>
        /// <param name="sums"></param>
        /// <returns></returns>
        private static int LotteryIndex(float[] sums)
        {
            float randomValue = Random.Range(0, sums[^1]);

            // 二分探索
            int left = -1;
            int right = sums.Length;
            while (right - left > 1)
            {
                int center = (left + right) / 2;

                if (sums[center] >= randomValue)
                {
                    right = center;
                }
                else
                {
                    left = center;
                }
            }

            return right;
        }
    }
}
