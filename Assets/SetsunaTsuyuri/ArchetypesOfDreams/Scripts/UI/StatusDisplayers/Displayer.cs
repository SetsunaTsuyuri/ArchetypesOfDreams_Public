using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 表示の基底クラス
    /// </summary>
    /// <typeparam name="TTarget">対象の型</typeparam>
    /// <typeparam name="TView">表示するものの型</typeparam>
    /// <typeparam name="TValue">値の型</typeparam>
    public abstract class Displayer<TTarget, TView, TValue> : MonoBehaviour
        where TTarget : MonoBehaviour
        where TView : MonoBehaviour
        where TValue : IComparable
    {
        /// <summary>
        /// 表示対象
        /// </summary>
        protected TTarget target = null;

        /// <summary>
        /// 表示するもの
        /// </summary>
        [SerializeField]
        protected TView view = null;

        /// <summary>
        /// 値
        /// </summary>
        protected TValue value = default;

        /// <summary>
        /// 最後に表示された値
        /// </summary>
        protected TValue theLastValue = default;

        protected virtual void LateUpdate()
        {
            if (CanUpdate())
            {
                UpdateValue();

                if (ValueHasChanged())
                {
                    UpdateView();
                    UpdateTheLastValue();
                }
            }
        }

        /// <summary>
        /// 更新処理ができる
        /// </summary>
        /// <returns></returns>
        protected bool CanUpdate()
        {
            return view && target;
        }

        /// <summary>
        /// 値に変化が生じた
        /// </summary>
        /// <returns></returns>
        protected bool ValueHasChanged()
        {
            return !(value is null) &&
                value.CompareTo(theLastValue) != 0;
        }

        /// <summary>
        /// 値を更新する
        /// </summary>
        protected abstract void UpdateValue();

        /// <summary>
        /// 表示を更新する
        /// </summary>
        protected abstract void UpdateView();

        /// <summary>
        /// 最後に表示した値を更新する
        /// </summary>
        protected virtual void UpdateTheLastValue()
        {
            theLastValue = value;
        }
    }
}
