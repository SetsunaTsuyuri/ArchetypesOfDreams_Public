using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ステータス効果
    /// </summary>
    public class StatusEffect
    {
        /// <summary>
        /// データ
        /// </summary>
        public StatusEffectData Data { get; private set; } = null;

        /// <summary>
        /// 残りの有効ターン数
        /// </summary>
        public int RemainingTurns { get; set; } = 0;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="turns">有効ターン数</param>
        public StatusEffect(int id, int turns)
        {
            Data = MasterData.GetStatusEffectData(id);
            RemainingTurns = turns;
        }
    }
}
