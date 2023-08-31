using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ランダムエンカウント敵データ
    /// </summary>
    [System.Serializable]
    public class RandomEncounterEnemyData
    {
        /// <summary>
        /// エリアID
        /// </summary>
        public int AreaId = 0;

        /// <summary>
        /// 敵ID
        /// </summary>
        public int EnemyId = 0;

        /// <summary>
        /// レベル
        /// </summary>
        public int Level = 0;

        /// <summary>
        /// レシオ
        /// </summary>
        public float Ratio = 0.0f;
    }
}
