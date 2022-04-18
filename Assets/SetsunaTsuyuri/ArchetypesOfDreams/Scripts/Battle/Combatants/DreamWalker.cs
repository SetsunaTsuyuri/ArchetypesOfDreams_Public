using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 夢渡り
    /// </summary>
    [System.Serializable]
    public class DreamWalker : Combatant
    {
        /// <summary>
        /// 共感力
        /// </summary>
        [field: SerializeField]
        public int Empathy { get; set; } = 0;

        public override CombatantData GetData()
        {
            return MasterData.DreamWalkers.GetValue(DataId);
        }

        /// <summary>
        /// 夢渡りのデータにして取得する
        /// </summary>
        /// <returns></returns>
        public DreamWalkerData GetDreamWalkerData()
        {
            return GetData() as DreamWalkerData;
        }
    }
}
