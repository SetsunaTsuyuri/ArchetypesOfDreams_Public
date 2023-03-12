using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者コンテナのUI
    /// </summary>
    public class CombatantContainerUI : GameUI
    {
        protected override void Awake()
        {
            base.Awake();
            DeactivateAndHide();
        }

        /// <summary>
        /// 表示対象を設定する
        /// </summary>
        /// <param name="target">表示対象</param>
        public virtual void SetTargets(CombatantContainer target)
        {
            IStatusDisplayer[] displayers = GetComponentsInChildren<IStatusDisplayer>(true);
            foreach (var displayer in displayers)
            {
                displayer.SetTarget(target);
            }
        }

        /// <summary>
        /// 戦闘者が設定されたときの処理
        /// </summary>
        /// <param name="combatantIsNotNull">戦闘者がnullではない</param>
        public void OnCombatantSet(bool combatantIsNotNull)
        {
            if (combatantIsNotNull)
            {
                ActivateAndShow();
            }
            else
            {
                DeactivateAndHide();
            }
        }

        /// <summary>
        /// 戦闘者の健康状態が設定されたときの処理
        /// </summary>
        /// <param name="condition">健康状態</param>
        public virtual void OnConditionSet(GameAttribute.Condition condition) { }
    }
}
