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
        public int HP = 1000;

        /// <summary>
        /// DP
        /// </summary>
        public int DP = 0;

        /// <summary>
        /// 力
        /// </summary>
        public int Power = 100;

        /// <summary>
        /// 技
        /// </summary>
        public int Technique = 100;

        /// <summary>
        /// 素早さ
        /// </summary>
        public int Speed = 100;

        /// <summary>
        /// 命中
        /// </summary>
        public int Accuracy = 100;

        /// <summary>
        /// 回避
        /// </summary>
        public int Evasion = 0;
    }
}
