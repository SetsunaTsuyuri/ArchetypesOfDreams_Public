using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Battle
    {
        /// <summary>
        /// 戦闘者のターン開始
        /// </summary>
        private class TurnStart : StateMachine<Battle>.State
        {
            public override void Enter(Battle context)
            {
                // 行動者が存在しない場合
                if (!context.Actor)
                {
                    // 戦闘終了
                    context.State.Change<BattleEnd>();
                }

                CancellationToken token = context.GetCancellationTokenOnDestroy();
                EnterAsync(context, token).Forget();
            }

            private async UniTask EnterAsync(Battle context, CancellationToken token)
            {
                await context.Actor.OnTurnStart(token);
                context.State.Change<ActionExecution>();
            }
        }
    }
}
