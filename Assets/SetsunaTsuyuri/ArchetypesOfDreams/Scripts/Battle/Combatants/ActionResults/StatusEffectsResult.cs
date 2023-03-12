using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ステータス効果の結果
    /// </summary>
    public struct StatusEffectsResult
    {
        /// <summary>
        /// 戦闘者コンテナ
        /// </summary>
        public CombatantContainer Container { get; set; }

        /// <summary>
        /// ステータス効果データ
        /// </summary>
        public StatusEffectData[] Effects { get; set; }

        public StatusEffectsResult(CombatantContainer container, StatusEffectData[] effects)
        {
            Container = container;
            Effects = effects;
        }
    }
}
