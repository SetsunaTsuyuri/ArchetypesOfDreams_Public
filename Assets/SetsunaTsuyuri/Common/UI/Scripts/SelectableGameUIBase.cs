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
        /// キャンセルした際の遷移先UI
        /// NOTE: 削除予定
        /// </summary>
        public SelectableGameUIBase Previous { get; set; } = null;

        /// <summary>
        /// 自身を選択する
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

            if (Previous)
            {
                Previous.BeSelected();
            }
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
    }
}
