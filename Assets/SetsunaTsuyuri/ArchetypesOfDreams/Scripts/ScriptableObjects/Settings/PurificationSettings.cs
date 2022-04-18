using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 浄化の設定
    /// </summary>
    [CreateAssetMenu(fileName = "Purification", menuName = "Settings/Purification")]
    public class PurificationSettings : ScriptableObject
    {
        /// <summary>
        /// 最低成功率
        /// </summary>
        [field: SerializeField]
        public int MinSuccessRate { get; private set; } = 1;

        /// <summary>
        /// 最高成功率
        /// </summary>
        [field: SerializeField]
        public int MaxSuccsessRate { get; private set; } = 100;

        /// <summary>
        /// レベル差による成功率補正
        /// </summary>
        [field: SerializeField]
        public int LevelDifferenceCorrection { get; private set; } = 10;

        /// <summary>
        /// 共感力による成功率補正
        /// </summary>
        [field: SerializeField]
        public float EmpathyCorrection { get; private set; } = 1.0f;

        /// <summary>
        /// 生命力減少による成功率補正
        /// </summary>
        [field: SerializeField]
        public float LifeReductionCorrection { get; private set; } = 2.0f;

        /// <summary>
        /// クラッシュ状態のときの成功率補正
        /// </summary>
        [field: SerializeField]
        public float CrushCorrection { get; private set; } = 3.0f;
    }
}
