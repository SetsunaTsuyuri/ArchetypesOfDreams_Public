using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 有限ステートマシン(UniTask使用)
    /// </summary>
    /// <typeparam name="TContext">制御されるクラス</typeparam>
    public class UniTaskStateMachine<TContext> where TContext : class
    {
        /// <summary>
        /// ステート
        /// </summary>
        public abstract class State
        {
            /// <summary>
            /// このステートに入る際の非同期処理
            /// </summary>
            public virtual async UniTask EnterAsync(TContext context, CancellationToken token)
            {
                await UniTask.CompletedTask;
            }

            /// <summary>
            /// このステートに入る際の処理
            /// </summary>
            /// <param name="context"></param>
            public virtual void Enter(TContext context) { }

            /// <summary>
            /// 更新処理
            /// </summary>
            /// <param name="context"></param>
            public virtual void Update(TContext context) { }

            /// <summary>
            /// このステートを出る際の処理
            /// </summary>
            /// <param name="context"></param>
            public virtual void Exit(TContext context) { }
        }

        /// <summary>
        /// 制御されるインスタンス
        /// </summary>
        readonly TContext context = null;

        /// <summary>
        /// ステートのリスト
        /// </summary>
        readonly List<State> _states = new();

        /// <summary>
        /// 現在のステート
        /// </summary>
        public State Current { get; private set; } = null;

        /// <summary>
        /// 次のステート
        /// </summary>
        Type _nextState = null;

        /// <summary>
        /// 次のステートに進む条件
        /// </summary>
        UniTask _nextStateTrigger = UniTask.CompletedTask;

        /// <summary>
        /// Updateができる
        /// </summary>
        bool _updateEnabled = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">このステートマシンによって制御されるインスタンス</param>
        public UniTaskStateMachine(TContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// ステートを追加する
        /// </summary>
        /// <typeparam name="TState">ステートの型</typeparam>
        public void Add<TState>() where TState : State
        {
            if (_states.ExistsSameType(typeof(TState)))
            {
                Debug.LogError(typeof(TState).Name + ": 既に同じ型のステートが追加されています");
                return;
            }

            State state = Activator.CreateInstance<TState>();
            _states.Add(state);
        }

        /// <summary>
        /// 指定した型のステートに遷移する
        /// </summary>
        /// <typeparam name="TState">ステートの型</typeparam>
        /// <param name="token"></param>
        private async UniTask Change<TState>(CancellationToken token) where TState : State
        {
            await Change(typeof(TState), token);
        }

        /// <summary>
        /// 指定した型のステートに遷移する
        /// </summary>
        private async UniTask Change(Type type, CancellationToken token)
        {
            State next = _states.GetSameType(type);
            if (next is null)
            {
                Debug.LogError("遷移先のステートが存在しません");
                return;
            }

            // ステートを出る際の処理
            Current?.Exit(context);

            // 次のステートへ遷移する
            Current = next;
            _nextState = null;

            // ステートに入る際の処理
            Current?.Enter(context);

            // ステートに入る際の非同期処理
            if (Current is not null)
            {
                await Current.EnterAsync(context, token);
            }
        }

        /// <summary>
        /// ステートの遷移を開始する
        /// </summary>
        /// <typeparam name="TState">ステートの型</typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public void StartChange<TState>(MonoBehaviour context) where TState : State
        {
            CancellationToken token = context.GetCancellationTokenOnDestroy();
            StartChange<TState>(token).Forget();
        }

        /// <summary>
        /// ステートの変更を開始する
        /// </summary>
        /// <typeparam name="TState">ステートの型</typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask StartChange<TState>(CancellationToken token) where TState : State
        {
            // 更新を無効化する
            _updateEnabled = false;

            await Change<TState>(token);

            // 次のステートが設定されている場合
            while (_nextState is not null)
            {
                // 次のステートに行くまで待機する
                await _nextStateTrigger;

                token.ThrowIfCancellationRequested();

                // 次のステートへ進む
                await Change(_nextState, token);
            }

            // 更新を有効化する
            _updateEnabled = true;
        }

        /// <summary>
        /// 次のステートを設定する
        /// </summary>
        /// <param name="type"></param>
        /// <param name="uniTask"></param>
        public void SetNextState<TState>() where TState : State
        {
            SetNextState<TState>(UniTask.CompletedTask);
        }

        /// <summary>
        /// 次のステートと遷移する条件を設定する
        /// </summary>
        /// <param name="state"></param>
        /// <param name="trigger"></param>
        public void SetNextState<TState>(UniTask trigger) where TState : State
        {
            _nextState = typeof(TState);
            _nextStateTrigger = trigger;
        }

        /// <summary>
        /// 更新する
        /// </summary>
        public void Update()
        {
            if (!CanUpdate())
            {
                return;
            }

            Current?.Update(context);
        }

        /// <summary>
        /// 更新可能である
        /// </summary>
        /// <returns></returns>
        private bool CanUpdate()
        {
            return Current is not null && _updateEnabled;
        }
    }
}
