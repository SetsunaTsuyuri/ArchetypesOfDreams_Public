using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵のデータ
    /// </summary>
    [System.Serializable]
    public class EnemyData
    {
        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; private set; }

        /// <summary>
        /// レベル補正
        /// </summary>
        [field: SerializeField]
        public int LevelCorrection { get; private set; }

        /// <summary>
        /// リーダーである
        /// </summary>
        [field: SerializeField]
        public bool IsLeader { get; private set; }

        /// <summary>
        /// ボスとしての耐性を持っている
        /// </summary>
        [field: SerializeField]
        public bool HasBossResistance { get; private set; }
    }
}
