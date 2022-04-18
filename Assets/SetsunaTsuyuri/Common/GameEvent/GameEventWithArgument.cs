using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 引数付きゲームイベント
    /// </summary>
    /// <typeparam name="T">引数の型</typeparam>
    public abstract class GameEventWithArgument<T> : ScriptableObject
    {
        /// <summary>
        /// ゲームイベントリスナーのリスト
        /// </summary>
        List<GameEventListenerWithArgument<T>> listeners = new List<GameEventListenerWithArgument<T>>();

        /// <summary>
        /// イベントを呼び出す
        /// </summary>
        /// <param name="arg"></param>
        public void Invoke(T arg)
        {
            foreach (var listener in listeners)
            {
                listener.OnEventInvoked(arg);
            }
        }

        /// <summary>
        /// イベントリスナーを登録する
        /// </summary>
        /// <param name="listener">リスナー</param>
        public void RegisterListener(GameEventListenerWithArgument<T> listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// イベントリスナーを登録解除する
        /// </summary>
        /// <param name="listener">リスナー</param>
        public void UnregisterListener(GameEventListenerWithArgument<T> listener)
        {
            listeners.Remove(listener);
        }
    }
}
