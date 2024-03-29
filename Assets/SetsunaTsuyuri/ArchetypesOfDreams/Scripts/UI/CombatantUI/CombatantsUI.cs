﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者コンテナUIの管理UI
    /// </summary>
    /// <typeparam name="TUI">UIの型</typeparam>
    /// <typeparam name="TParty">パーティの型</typeparam>
    /// <typeparam name="TContainer">戦闘者コンテナの型</typeparam>
    public abstract class CombatantsUI<TUI, TParty, TContainer> : GameUI
        where TUI : CombatantUI
        where TParty : CombatantsParty<TContainer>
        where TContainer : CombatantContainer 
    {
        /// <summary>
        /// 戦闘者パーティ
        /// </summary>
        [SerializeField]
        protected TParty _party = null;

        /// <summary>
        /// 戦闘者UI配列
        /// </summary>
        protected TUI[] _uiArray = null;

        protected override void Awake()
        {
            base.Awake();

            _uiArray = GetComponentsInChildren<TUI>(true);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        public virtual void SetUp()
        {
            SetTargets();
        }

        /// <summary>
        /// UIの表示対象を設定する
        /// </summary>
        private void SetTargets()
        {
            CombatantContainer[] targets = _party
                .GetAllContainers()
                .ToArray();

            for (int i = 0; i < _uiArray.Length; i++)
            {
                _uiArray[i].SetTargets(targets[i]);
            }
        }
    }
}
