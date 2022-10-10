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
        /// HP
        /// </summary>
        [field: SerializeField]
        public int HP { get; private set; }

        /// <summary>
        /// DP
        /// </summary>
        [field: SerializeField]
        public int DP { get; private set; }

        /// <summary>
        /// 力
        /// </summary>
        [field: SerializeField]
        public int Power { get; private set; }

        /// <summary>
        /// 技
        /// </summary>
        [field: SerializeField]
        public int Technique { get; private set; }

        /// <summary>
        /// 素早さ
        /// </summary>
        [field: SerializeField]
        public int Speed { get; private set; }
    }
}
