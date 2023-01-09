using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 時間が進む
        /// </summary>
        public class TimeAdvancing : StateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
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

                // 行動順
                CombatantContainer[] orderOfActions = DecideOrderOfActions(fightables);

                // 行動順UIを更新する
                context.BattleUI.OrderOfActions.UpdateDisplay(orderOfActions);

                // 行動者を決定する
                context.Actor = orderOfActions.FirstOrDefault();

                // 行動開始
                context.State.Change<ActionStart>();
            }

            /// <summary>
            /// 戦闘可能なコンテナを取得する
            /// </summary>
            /// <param name="battle">戦闘の管理者</param>
            /// <returns></returns>
            private CombatantContainer[] GetFightables(BattleManager battle)
            {
                CombatantContainer[] allies = battle.Allies.GetFightables();
                CombatantContainer[] enemies = battle.Enemies.GetFightables();

                CombatantContainer[] fightables = allies
                    .Concat(enemies)
                    .ToArray();

                return fightables;
            }

            /// <summary>
            /// 行動順を決定する
            /// </summary>
            /// <returns>戦闘可能なコンテナ配列</returns>
            private CombatantContainer[] DecideOrderOfActions(CombatantContainer[] fightables)
            {
                // 優先順位
                // 待機時間が小さい→味方である→IDが小さい
                CombatantContainer[] order = fightables
                    .OrderBy(x => x.Combatant.WaitTime)
                    .ThenByDescending(x => x is AllyContainer)
                    .ThenBy(x => x.Id)
                    .ToArray();

                return order;
            }
        }
    }
}
