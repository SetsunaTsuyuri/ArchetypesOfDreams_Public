using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 待機処理のユーティリティクラス
    /// </summary>
    public static class TimeUtility
    {
        /// <summary>
        /// 待機する
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask Wait(float seconds, CancellationToken token)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            await UniTask.Delay(timeSpan, cancellationToken: token);
        }
    }
}
