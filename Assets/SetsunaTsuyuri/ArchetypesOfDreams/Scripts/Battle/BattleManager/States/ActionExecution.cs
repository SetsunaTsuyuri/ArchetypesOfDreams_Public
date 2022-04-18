using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 行動の対象
        /// </summary>
        public CombatantContainer[] Targets { get; set; }

        /// <summary>
        /// 行動実行
        /// </summary>
        private class ActionExecution : FiniteStateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                // 行動の対象を抽出する
                context.Targets = context.Targetables
                    .Where(c => c.IsTargeted)
                    .ToArray();

                // 行動させる
                context.ExecuteCombatantActionAsync().Forget();
            }

            public override void Exit(BattleManager context)
            {
                // 対象フラグをリセットする
                context.ResetTargetFlags();
            }
        }

        /// <summary>
        /// 非同期的に戦闘者を行動させる
        /// </summary>
        /// <returns></returns>
        private async UniTask ExecuteCombatantActionAsync()
        {
            // スキルを実行させる
            await Performer.Combatant.UseSkill(this, this.GetCancellationTokenOnDestroy());

            // 行動終了へ
            State.Change<ActionEnd>();
        }

        /// <summary>
        /// 対象フラグをリセットする
        /// </summary>
        public void ResetTargetFlags()
        {
            foreach (var target in Targets)
            {
                target.IsTargeted = false;
            }
        }
    }
}
