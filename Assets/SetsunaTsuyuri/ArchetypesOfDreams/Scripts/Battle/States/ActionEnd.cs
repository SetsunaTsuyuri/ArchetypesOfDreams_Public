using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Battle
    {
        /// <summary>
        /// 行動終了開始時のイベント
        /// </summary>
        [SerializeField]
        GameEvent onActionEndEnter = null;

        /// <summary>
        /// 戦闘者の行動終了
        /// </summary>
        private class ActionEnd : StateMachine<Battle>.State
        {
            public override void Enter(Battle context)
            {
                // イベント呼び出し
                context.onActionEndEnter.Invoke();

                // 行動後の処理
                CancellationToken token = context.GetCancellationTokenOnDestroy();
                context.ActionEndAsync(token).Forget();
            }
        }

        /// <summary>
        /// 戦闘者の行動終了時の処理
        /// </summary>
        /// <param name="token"></param>
        private async UniTask ActionEndAsync(CancellationToken token)
        {
            if (Actor != null)
            {
                await Actor.Combatant.OnActionEnd(this, token);
            }

            // 解放可能な敵を解放する
            Enemies.ReleaseKnockedOutReleasables();

            // 戦闘続行可能の場合
            if (CanContinue())
            {
                // 時間進行
                State.Change<TimeAdvancing>();
            }
            else
            {
                // 戦闘終了
                State.Change<BattleEnd>();
            }
        }

        /// <summary>
        /// 行動終了ステートへ移行する
        /// </summary>
        public void ToActionEnd()
        {
            State.Change<ActionEnd>();
        }
    }
}
