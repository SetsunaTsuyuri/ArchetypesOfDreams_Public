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
        public int EnemyId = 0;

        /// <summary>
        /// レベル
        /// </summary>
        public int Level = 0;

        /// <summary>
        /// ボス耐性を持っている
        /// </summary>
        public bool HasBossResistance = false;

        /// <summary>
        /// リーダーである
        /// </summary>
        public bool IsLeader = false;
    }
}
