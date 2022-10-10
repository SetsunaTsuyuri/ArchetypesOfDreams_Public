using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 戦闘開始
        /// </summary>
        private class BattleStart : StateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                // 戦闘続行不可能な場合
                if (!context.CanContinue())
                {
                    // 戦闘終了
                    context.State.Change<BattleEnd>();
                    return;
                }

                // 戦闘開始時の処理
                context.Allies.OnBattleStart();
                context.Enemies.OnBattleStart();

                // 時間進行
                context.State.Change<TimeAdvancing>();
            }
        }
    }
}
