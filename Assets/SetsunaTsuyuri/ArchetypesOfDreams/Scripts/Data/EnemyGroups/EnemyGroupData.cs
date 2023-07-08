using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵グループのデータ
    /// </summary>
    [System.Serializable]
    public class EnemyGroupData : IdData
    {
        /// <summary>
        /// 敵データ配列
        /// </summary>
        public EnemyData[] Enemies = { };
    }
}
