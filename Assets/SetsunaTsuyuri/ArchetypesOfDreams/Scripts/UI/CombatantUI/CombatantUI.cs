using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者のUI
    /// </summary>
    public abstract class CombatantUI : GameUI
    {
        /// <summary>
        /// 対象コンテナ
        /// </summary>
        protected CombatantContainer Target = null;

        protected override void Awake()
        {
            base.Awake();
            DeactivateAndHide();

            // コンテナ設定
            MessageBrokersManager.CombatantSet
                .Receive<CombatantContainer>()
                .Where(x => x == Target)
                .TakeUntilDestroy(gameObject)
                .Subscribe(OnCombatantSet);
        }

        /// <summary>
        /// 表示対象を設定する
        /// </summary>
        /// <param name="target">表示対象</param>
        public virtual void SetTargets(CombatantContainer target)
        {
            Target = target;

            IStatusDisplayer[] displayers = GetComponentsInChildren<IStatusDisplayer>(true);
            foreach (var displayer in displayers)
            {
                displayer.SetTarget(target);
            }
        }

        /// <summary>
        /// 戦闘者が設定されたときの処理
        /// </summary>
        /// <param name="target">戦闘者がnullではない</param>
        public void OnCombatantSet(CombatantContainer target)
        {
            if (target.Combatant is not null)
            {
                ActivateAndShow();
            }
            else
            {
                DeactivateAndHide();
            }
        }
    }
}
