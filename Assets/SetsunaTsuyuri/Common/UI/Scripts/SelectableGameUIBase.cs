using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 選択可能なUI
    /// </summary>
    public abstract class SelectableGameUIBase : GameUI
    {
        /// <summary>
        /// 選択する
        /// </summary>
        public virtual void BeSelected()
        {
            Selected?.Invoke(this);
        }

        /// <summary>
        /// キャンセルする
        /// </summary>
        public virtual void BeCanceled()
        {
            Canceled?.Invoke();
        }

        /// <summary>
        /// スタックする
        /// </summary>
        /// <param name="type"></param>
        public void Stack(Type type)
        {
            OnStack?.Invoke(type);
        }

        /// <summary>
        /// 選択を解除し、履歴を消去する
        /// </summary>
        public void BeDeselectedAndClearHistory()
        {
            BeDeselected();
            OnClearingHistory?.Invoke();
        }

        /// <summary>
        /// 選択が解除されたときの処理
        /// </summary>
        public abstract void BeDeselected();

        /// <summary>
        /// 選択されたときのイベント
        /// </summary>
        public event UnityAction<SelectableGameUIBase> Selected;

        /// <summary>
        /// キャンセルされたときのイベント
        /// </summary>
        public event UnityAction Canceled;

        /// <summary>
        /// スタックするときのイベント
        /// </summary>
        public event UnityAction<Type> OnStack;

        /// <summary>
        /// 履歴消去するときのイベント
        /// </summary>
        public event UnityAction OnClearingHistory;
    }
}
