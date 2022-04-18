using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// ゲームイベントリスナー
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {
        /// <summary>
        /// ゲームイベント
        /// </summary>
        [SerializeField]
        GameEvent gameEvent = null;

        /// <summary>
        /// UnityEvent
        /// </summary>
        [SerializeField]
        UnityEvent unityEvent = null;

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
        public void OnEventInvoked()
        {
            unityEvent.Invoke();
        }
    }
}
