using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 行動終了開始時のイベント
        /// </summary>
        [SerializeField]
        GameEvent onActionEndEnter = null;

        /// <summary>
        /// 戦闘者の行動終了
        /// </summary>
        private class ActionEnd : StateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                // イベント呼び出し
                context.onActionEndEnter.Invoke();

                // 行動後の処理
                context.Actor.Combatant?.OnActionEnd(context);

                // 解放可能な敵を解放する
                context.Enemies.ReleaseKnockedOutReleasables();

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
