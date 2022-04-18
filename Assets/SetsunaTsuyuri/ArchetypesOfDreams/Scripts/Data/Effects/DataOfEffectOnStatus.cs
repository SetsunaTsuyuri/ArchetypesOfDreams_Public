using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ステータスへの影響データ
    /// </summary>
    [System.Serializable]
    public class DataOfEffectOnStatus
    {
        /// <summary>
        /// 影響を受けるステータス
        /// </summary>
        [field: SerializeField]
        public Effect.Affected Affected { get; private set; }

        /// <summary>
        /// 増減の種類
        /// </summary>
        [field: SerializeField]
        public Effect.IncreaseOrDecrease IncreaseOrDecrease { get; private set; }

        /// <summary>
        /// 計算式の種類
        /// </summary>
        [field: SerializeField]
        public Effect.Formula Formula { get; private set; }

        /// <summary>
        /// 値
        /// </summary>
        [field: SerializeField]
        public float Parameter { get; private set; }

        /// <summary>
        /// 固定値
        /// </summary>
        [field: SerializeField]
        public int Fixed { get; private set; }

        /// <summary>
        /// 乱数値
        /// </summary>
        [field: SerializeField]
        public int Random { get; private set; }

        /// <summary>
        /// ゼロにしない
        /// </summary>
        [field: SerializeField]
        public bool DontToZero { get; private set; }

        /// <summary>
        /// ダメージを与える効果である
        /// </summary>
        /// <returns></returns>
        public bool IsDamage()
        {
            return IncreaseOrDecrease == Effect.IncreaseOrDecrease.Damage;
        }

        /// <summary>
        /// 回復させる効果である
        /// </summary>
        /// <returns></returns>
        public bool IsRecovery()
        {
            return IncreaseOrDecrease == Effect.IncreaseOrDecrease.Recovery;
        }
    }
}
