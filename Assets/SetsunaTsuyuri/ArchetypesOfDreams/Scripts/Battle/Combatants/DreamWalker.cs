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

        /// <summary>
        /// 夢渡りデータ
        /// </summary>
        /// <returns></returns>
        public DreamWalkerData DreamWalkerData
        {
            get => MasterData.DreamWalkers[DataId];
        }

        public override CombatantData Data
        {
            get => DreamWalkerData;
        }

        public override bool CanBeReleased()
        {
            return false;
        }

        public override bool CanSelectPurification()
        {
            return true;
        }

        public override int GetPurificationSuccessRate(Combatant purifier)
        {
            return 0;
        }

        protected override Combatant CreateClone(string json)
        {
            DreamWalker clone = JsonUtility.FromJson<DreamWalker>(json);
            return clone;
        }
    }
}
