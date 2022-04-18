using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動順表示UIの管理者
    /// </summary>
    public class OrderOfActionsManager : GameUI
    {
        /// <summary>
        /// 行動順表示UI配列
        /// </summary>
        OrderOfAction[] orderOfActions = { };

        protected override void Awake()
        {
            base.Awake();

            orderOfActions = GetComponentsInChildren<OrderOfAction>(true);
            Hide();
        }

        /// <summary>
        /// 表示を更新する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void UpdateDisplay(BattleManager battle)
        {
            CombatantContainer[] containers = battle.OrderOfActions
                .ToArray();

            System.Array.Reverse(containers);

            for (int i = 0; i < orderOfActions.Length; i++)
            {
                if (i < containers.Length)
                {
                    // 対象を設定する
                    orderOfActions[i].Target = containers[i];
                }
                else
                {
                    // 対象を外す
                    orderOfActions[i].Target = null;
                }
            }
        }
    }
}
