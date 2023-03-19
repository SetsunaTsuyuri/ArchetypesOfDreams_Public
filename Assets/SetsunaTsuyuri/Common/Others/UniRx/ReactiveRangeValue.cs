using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 最小値と最大値を設定できるstructのリアクティブプロパティ
    /// </summary>
    public class ReactiveRangeValue<T> where T : struct, System.IComparable<T>
    {
        public ReactiveRangeValue(T initialValue, T min, T max)
        {
            Min = min;
            Max = max;
            _property = new(Clamp(initialValue));
        }

        /// <summary>
        /// 最小値
        /// </summary>
        public readonly T Min = default;

        /// <summary>
        /// 最大値
        /// </summary>
        public readonly T Max = default;

        /// <summary>
        /// ReactiveProperty
        /// </summary>
        readonly ReactiveProperty<T> _property = null;

        /// <summary>
        /// IObservable
        /// </summary>
        public System.IObservable<T> Observable => _property;

        /// <summary>
        /// 値
        /// </summary>
        public T Value
        {
            get => _property.Value;
            set
            {
                T newValue = Clamp(value);
                _property.Value = newValue;
            }
        }

        /// <summary>
        /// クランプする
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private T Clamp(T value)
        {
            if (value.CompareTo(Min) < 0)
            {
                value = Min;
            }
            else if (value.CompareTo(Max) > 0)
            {
                value = Max;
            }

            return value;
        }

        /// <summary>
        /// 値を設定し、強制的に通知する
        /// </summary>
        /// <param name="value"></param>
        public void SetValueAndForceNortify(T value)
        {
            T newValue = Clamp(value);
            _property.SetValueAndForceNotify(newValue);
        }
    }
}
