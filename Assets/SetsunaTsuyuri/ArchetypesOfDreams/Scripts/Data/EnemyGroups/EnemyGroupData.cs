using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵グループのデータ
    /// </summary>
    [System.Serializable]
    public class EnemyGroupData
    {
        /// <summary>
        /// 敵の構成
        /// </summary>
        [field: SerializeField]
        public EnemyData[] Enemies { get; private set; }
    }
}
