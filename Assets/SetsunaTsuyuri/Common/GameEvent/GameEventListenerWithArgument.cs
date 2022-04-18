using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 引数付きゲームイベントリスナー
    /// </summary>
    /// <typeparam name="T">引数の型</typeparam>
    public class GameEventListenerWithArgument<T> : MonoBehaviour
    {
        /// <summary>
        /// ゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithArgument<T> gameEvent = null;

        /// <summary>
        /// UnityEvent
        /// </summary>
        [SerializeField]
        UnityEvent<T> unityEvent = new UnityEvent<T>();

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        /// <summary>
        /// イベントが呼び出されたときの処理
        /// </summary>
        /// <param name="arg">引数</param>
        public void OnEventInvoked(T arg)
        {
            unityEvent.Invoke(arg);
        }
    }
}
