using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// Transformの拡張メソッド集
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// ローカル座標Xを変更する
        /// </summary>
        /// <param name="transform">トランスフォーム</param>
        /// <param name="x">X座標</param>
        public static void ChangeLocalPositionX(this Transform transform, float x)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.x = x;
            transform.localPosition = newPosition;
        }
    }
}
