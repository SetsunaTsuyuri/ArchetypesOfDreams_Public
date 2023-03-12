using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 付与ステータス効果の結果
    /// </summary>
    public struct AddedStatusEffectResult
    {
        /// <summary>
        /// 戦闘者コンテナ
        /// </summary>
        public CombatantContainer Container { get; set; }

        /// <summary>
        /// ステータス効果データ
        /// </summary>
        public StatusEffectData Effect { get; set; }

        public AddedStatusEffectResult(CombatantContainer container, StatusEffectData effect)
        {
            Container = container;
            Effect = effect;
        }
    }
}
