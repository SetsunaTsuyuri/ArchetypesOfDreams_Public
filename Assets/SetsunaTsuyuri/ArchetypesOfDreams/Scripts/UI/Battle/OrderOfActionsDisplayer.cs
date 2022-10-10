using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動順表示UI
    /// </summary>
    public class OrderOfActionsDisplayer : GameUI
    {
        /// <summary>
        /// 行動順表示UI要素配列
        /// </summary>
        OrderOfActionsElement[] _elements = { };

        protected override void Awake()
        {
            base.Awake();

            _elements = GetComponentsInChildren<OrderOfActionsElement>(true);
        }

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="containers">戦闘可能なコンテナ配列</param>
        public void UpdateDisplay(CombatantContainer[] fightable)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (i < fightable.Length)
                {
                    // 対象を設定する
                    _elements[i].Target = fightable[i];
                }
                else
                {
                    // 対象を外す
                    _elements[i].Target = null;
                }
            }
        }
    }
}
