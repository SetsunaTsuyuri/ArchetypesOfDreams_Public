using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 進行度イベント
    /// </summary>
    public abstract class ProgressionEvent : IGameEvent
    {
        /// <summary>
        /// 式の種類
        /// </summary>
        public enum FormulaType
        {
            Assign = 0,
            Add = 1,
            Subtruct = 2
        }

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// 式の種類
        /// </summary>
        [field: SerializeField]
        public FormulaType Formula { get; set; } = 0;

        /// <summary>
        /// パラメーター
        /// </summary>
        [field: SerializeField]
        public int Parameter { get; set; } = 0;

        public async UniTask Execute(CancellationToken token)
        {
            int value = GetValue();

            switch (Formula)
            {
                case FormulaType.Assign:
                    value = Parameter;
                    break;

                case FormulaType.Add:
                    value += Parameter;
                    break;

                case FormulaType.Subtruct:
                    value -= Parameter;
                    break;

                default:
                    break;
            }

            SetValue(value);

            token.ThrowIfCancellationRequested();
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// 値を取得する
        /// </summary>
        /// <returns></returns>
        public abstract int GetValue();

        /// <summary>
        /// 値を設定する
        /// </summary>
        /// <returns></returns>
        public abstract void SetValue(int value);

        public UniTask GetUniTask(CancellationToken token)
        {
            return Execute(token); 
        }
    }
}
