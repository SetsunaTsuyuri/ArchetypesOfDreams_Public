using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ステータス効果ID
    /// </summary>
    public enum StatusEffectId
    {
        /// <summary>
        /// 戦闘不能
        /// </summary>
        KnockedOut = 1,

        /// <summary>
        /// 崩壊
        /// </summary>
        Crush = 2
    }

    /// <summary>
    /// ステータス効果
    /// </summary>
    [System.Serializable]
    public class StatusEffect
    {
        /// <summary>
        /// データ
        /// </summary>
        [field: SerializeField]
        public StatusEffectData Data { get; private set; } = null;

        /// <summary>
        /// 残りの有効ターン数
        /// </summary>
        [field: SerializeField]
        public int RemainingTurns { get; set; } = 0;

        /// <summary>
        /// 永続する
        /// </summary>
        [field: SerializeField]
        public bool IsPermanent { get; set; } = false;

        /// <summary>
        /// 解除されるべき効果である
        /// </summary>
        public bool MustBeRemoved => !IsPermanent && RemainingTurns == 0;

        public StatusEffect(int id, int turns, bool isPermanent)
        {
            Data = MasterData.GetStatusEffectData(id);
            RemainingTurns = turns;
            IsPermanent = isPermanent;
        }

        public StatusEffect(StatusEffectId id, int turns, bool isPermanent)
        {
            Data = MasterData.GetStatusEffectData(id);
            RemainingTurns = turns;
            IsPermanent = isPermanent;
        }
    }
}
