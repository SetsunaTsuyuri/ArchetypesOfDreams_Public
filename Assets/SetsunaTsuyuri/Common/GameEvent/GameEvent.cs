using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// ゲームイベント
    /// </summary>
    [CreateAssetMenu(menuName = "GameEvents/NoArgument")]
    public class GameEvent : ScriptableObject
    {
        /// <summary>
        /// ゲームイベントリスナーのリスト
        /// </summary>
        List<GameEventListener> listeners = new List<GameEventListener>();

        /// <summary>
        /// イベントを呼び出す
        /// </summary>
        public void Invoke()
        {
            foreach (var listener in listeners)
            {
                listener.OnEventInvoked();
            }
        }

        /// <summary>
        /// イベントリスナーを登録する
        /// </summary>
        /// <param name="listener">リスナー</param>
        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// イベントリスナーを登録解除する
        /// </summary>
        /// <param name="listener">リスナー</param>
        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }

    }
}
