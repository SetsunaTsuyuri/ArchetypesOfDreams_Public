using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者UIの管理者
    /// </summary>
    /// <typeparam name="TUI">UIの型</typeparam>
    /// <typeparam name="TContainersManager">戦闘者コンテナ管理者の型</typeparam>
    /// <typeparam name="TContainer">戦闘者コンテナの型</typeparam>
    public class CombatantContainerUIManager<TUI, TContainersManager, TContainer> : GameUI
        where TUI : CombatantContainerUI
        where TContainersManager : CombatantContainersManager<TContainer>
        where TContainer : CombatantContainer 
    {
        /// <summary>
        /// 戦闘者コンテナの管理者
        /// </summary>
        [SerializeField]
        protected TContainersManager containers = null;

        /// <summary>
        /// 戦闘者UI配列
        /// </summary>
        protected TUI[] uiArray = null;

        protected override void Awake()
        {
            base.Awake();

            uiArray = GetComponentsInChildren<TUI>(true);
        }

        protected virtual void Start()
        {
            SetTargets();
        }

        /// <summary>
        /// UIの表示対象を設定する
        /// </summary>
        private void SetTargets()
        {
            CombatantContainer[] targets = containers.GetAllContainers();
            for (int i = 0; i < uiArray.Length; i++)
            {
                uiArray[i].SetTargets(targets[i]);
            }
        }

        /// <summary>
        /// 戦闘者が設定されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnCombatantSet(CombatantContainer container)
        {
            if (uiArray is null)
            {
                return;
            }

            TUI ui = uiArray[container.Id];
            ui.OnCombatantSet(container.ContainsCombatant());
        }

        /// <summary>
        /// 健康状態が設定されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnConditionSet(CombatantContainer container)
        {
            if (uiArray == null)
            {
                return;
            }

            TUI ui = uiArray[container.Id];
            ui.OnConditionSet(container.Combatant.Condition);
        }
    }
}
