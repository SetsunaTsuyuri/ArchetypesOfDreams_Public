using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Battle
    {
        /// <summary>
        /// 時間が進む
        /// </summary>
        public class TimeAdvancing : StateMachine<Battle>.State
        {
            public override void Enter(Battle context)
            {
                // 戦闘可能なコンテナ配列
                CombatantContainer[] fightables = GetFightables(context);

                // 経過させる時間
                int elapsedTime = fightables.Min(x => x.Combatant.WaitTime);

                // 時間を進める
                foreach (var fightable in fightables)
                {
                    fightable.OnTimeElapsed(elapsedTime);
                }

                context.Actor = null;

                // 行動順を更新する
                context.UpdateOrderOfActions();

                // 行動者を決定する
                context.Actor = context.OrderOfActions.FirstOrDefault();

                // ターン開始
                context.State.Change<TurnStart>();
            }

            /// <summary>
            /// 戦闘可能なコンテナを取得する
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            private CombatantContainer[] GetFightables(Battle context)
            {
                var allies = context.Allies.GetFightables();
                var enemies = context.Enemies.GetFightables();

                CombatantContainer[] fightables = allies
                    .Concat(enemies)
                    .ToArray();

                return fightables;
            }
        }
    }
}
