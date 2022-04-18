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
        /// 戦闘終了
        /// </summary>
        private class BattleEnd : FiniteStateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
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
                Debug.Log("味方の勝ち");

                // 味方経験値増加
                AddAlliesExperience();
            }
            else
            {
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

            // 戦闘終了フラグON
            IsOver = true;
        }

        /// <summary>
        /// 味方の経験値を増やす
        /// </summary>
        private void AddAlliesExperience()
        {
            Allies.AddExperience(Experience);
        }
    }
}
