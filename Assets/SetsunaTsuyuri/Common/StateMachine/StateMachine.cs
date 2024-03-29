﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// ステートマシン
    /// </summary>
    /// <typeparam name="TContext">制御されるクラス</typeparam>
    public class StateMachine<TContext> where TContext : class
    {
        /// <summary>
        /// ステート
        /// </summary>
        public abstract class State
        {
            /// <summary>
            /// このステートに入ったときの処理
            /// </summary>
            public virtual void Enter(TContext context) { }

            /// <summary>
            /// 更新処理
            /// </summary>
            public virtual void Update(TContext context) { }

            /// <summary>
            /// このステートを出るときの処理
            /// </summary>
            public virtual void Exit(TContext context) { }
        }

        /// <summary>
        /// 制御されるインスタンス
        /// </summary>
        readonly TContext context = null;

        /// <summary>
        /// ステートのリスト
        /// </summary>
        readonly List<State> states = new();

        /// <summary>
        /// 現在のステート
        /// </summary>
        public State Current { get; private set; } = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">このステートマシンによって制御されるインスタンス</param>
        public StateMachine(TContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// ステートを追加する
        /// </summary>
        /// <typeparam name="TState">ステートの型</typeparam>
        public void Add<TState>() where TState : State
        {
            if (states.Any(x => x.GetType() == typeof(TState)))
            {
                Debug.LogWarning(typeof(TState).Name + ": 既に同じ型のステートが追加されています");
                return;
            }

            State state = Activator.CreateInstance<TState>();
            states.Add(state);
        }

        /// <summary>
        /// 指定した型のステートに遷移する
        /// </summary>
        /// <typeparam name="TState">ステートの型</typeparam>
        public void Change<TState>() where TState : State
        {
            State next = states.FirstOrDefault(x => x.GetType() == typeof(TState));
            if (next is null)
            {
                Debug.LogWarning("遷移先のステートが存在しません");
                return;
            }

            Current?.Exit(context);
            Current = next;
            Current?.Enter(context);
        }

        /// <summary>
        /// 更新する
        /// </summary>
        public void Update()
        {
            Current?.Update(context);
        }
    }
}
