using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Battle
    {
        /// <summary>
        /// 戦闘終了
        /// </summary>
        private class BattleEnd : StateMachine<Battle>.State
        {
            public override void Enter(Battle context)
            {
                context.OnBattleEndAsync(context.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        /// <summary>
        /// 戦闘終了(非同期)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask OnBattleEndAsync(CancellationToken token)
        {
            // 戦闘終了時の処理
            Allies.OnBattleEnd();

            if (Allies.CanFight())
            {
                Allies.OnWin(RewardExperience);

                _result = BattleResultType.Win;
                Debug.Log("味方の勝ち");
            }
            else
            {
                _result = BattleResultType.Lose;
                Debug.Log("敵の勝ち");
            }

            // ★ 待機
            await UniTask.Delay(300);

            // フェードアウト
            await FadeManager.FadeOut(token);

            // UIを隠す
            BattleUI.Hide();

            // ★ 待機
            await UniTask.Delay(500);

            // 休眠状態へ
            State.Change<Sleep>();

            // カメラ視点を元に戻す
            _cameraController.ToPrevious();

            // 戦闘終了フラグON
            IsOver = true;
        }
    }
}
