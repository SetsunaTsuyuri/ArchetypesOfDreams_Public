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
        GameEvent onTurnEndEnter = null;

        /// <summary>
        /// 戦闘者のターン終了
        /// </summary>
        private class TurnEnd : StateMachine<Battle>.State
        {
            public override void Enter(Battle context)
            {
                // イベント呼び出し
                context.onTurnEndEnter.Invoke();

                CancellationToken token = context.GetCancellationTokenOnDestroy();
                EnterAsync(context, token).Forget();
            }

            private async UniTask EnterAsync(Battle context, CancellationToken token)
            {
                await context.Actor.OnTurnEnd(token);

                // 戦闘続行可能の場合
                if (context.CanContinue())
                {
                    // 時間進行
                    context.State.Change<TimeAdvancing>();
                }
                else
                {
                    // 戦闘終了
                    context.State.Change<BattleEnd>();
                }
            }
        }
    }
}
