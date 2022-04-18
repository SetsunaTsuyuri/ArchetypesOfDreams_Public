using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 基本ステータスのデータ
    /// </summary>
    [System.Serializable]
    public class BasicStatusData
    {
        /// <summary>
        /// 生命力
        /// </summary>
        [field: SerializeField]
        public int Life { get; private set; }

        /// <summary>
        /// 初期夢想力
        /// </summary>
        [field: SerializeField]
        public int Dream { get; private set; }

        /// <summary>
        /// 近接攻撃力
        /// </summary>
        [field: SerializeField]
        public int MeleeAttack { get; private set; }

        /// <summary>
        /// 遠隔攻撃力
        /// </summary>
        [field: SerializeField]
        public int RangedAttack { get; private set; }

        /// <summary>
        /// 近接守備力
        /// </summary>
        [field: SerializeField]
        public int MeleeDefense { get; private set; }

        /// <summary>
        /// 遠隔守備力
        /// </summary>
        [field: SerializeField]
        public int RangedDefense { get; private set; }

        /// <summary>
        /// 行動速度
        /// </summary>
        [field: SerializeField]
        public int Speed { get; private set; }
    }
}
