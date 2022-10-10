using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 戦闘開始前の準備
        /// </summary>
        private class Preparation : StateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                context.OnPreparetionAsync(context.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        /// <summary>
        /// 戦闘開始前の準備(非同期)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask OnPreparetionAsync(CancellationToken token)
        {
            // フェードアウト
            await FadeManager.FadeOut(token);

            // 戦闘UIを表示する
            BattleUI.Show();

            // 位置を調整する
            Enemies.AdjsutEnemiesPosition();

            // フェードイン
            await FadeManager.FadeIn(token);

            // 戦闘開始ステートへ移行する
            State.Change<BattleStart>();
        }
    }
}
