using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// スキル習得データ
    /// </summary>
    [System.Serializable]
    public class SkillAcquisitionData
    {
        /// <summary>
        /// スキルID
        /// </summary>
        public int SkillId = 0;

        /// <summary>
        /// 習得レベル
        /// </summary>
        public int AcquisitionLevel = 0;
    }
}
