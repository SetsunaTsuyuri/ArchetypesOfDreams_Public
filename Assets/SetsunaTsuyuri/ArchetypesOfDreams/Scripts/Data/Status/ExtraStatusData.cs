using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 追加ステータスのデータ
    /// </summary>
    [System.Serializable]
    public class ExtraStatusData
    {
        /// <summary>
        /// 命中
        /// </summary>
        [field: SerializeField]
        public int Hit { get; private set; }

        /// <summary>
        /// 回避
        /// </summary>
        [field: SerializeField]
        public int Evasion { get; private set; }

        /// <summary>
        /// クリティカル
        /// </summary>
        [field: SerializeField]
        public int Critical { get; private set; }

        /// <summary>
        /// 近接攻撃無効化率
        /// </summary>
        [field: SerializeField]
        public int MeleeNullification { get; private set; }

        /// <summary>
        /// 遠隔攻撃無効化率
        /// </summary>
        [field: SerializeField]
        public int RangedNullification { get; private set; }
    }
}
