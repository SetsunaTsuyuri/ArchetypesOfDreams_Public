using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 夢魔
    /// </summary>
    [System.Serializable]
    public class DreamWalker : Combatant
    {
        /// <summary>
        /// 夢魔データ
        /// </summary>
        /// <returns></returns>
        public DreamWalkerData DreamWalkerData
        {
            get => MasterData.GetDreamWalkerData(DataId);
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

        public override int CaluclatePurificationSuccessRate(Combatant purifier)
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
