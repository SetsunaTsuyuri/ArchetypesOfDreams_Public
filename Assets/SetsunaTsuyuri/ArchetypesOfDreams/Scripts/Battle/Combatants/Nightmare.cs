using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ナイトメア
    /// </summary>
    [System.Serializable]
    public class Nightmare : Combatant
    {
        public override bool CanBeReleased()
        {
            return true;
        }

        public override bool CanSelectPurification()
        {
            return false;
        }

        /// <summary>
        /// ナイトメアデータ
        /// </summary>
        /// <returns></returns>
        private NightmareData NightmareData
        {
            get => MasterData.GetNightmareData(DataId);
        }

        public override CombatantData Data
        {
            get => NightmareData;
        }

        public override int CaluclatePurificationSuccessRate(Combatant purifier)
        {
            // ボス耐性がある場合は常に0%
            if (HasBossResistance)
            {
                return 0;
            }

            // 基本成功率
            int result = NightmareData.PurificationSuccessRate;

            // レベル差補正
            result += (purifier.Level - Level) * GameSettings.Purification.LevelDifferenceCorrection;

            // HP減少補正
            result += Mathf.FloorToInt(HPDecreaseRate * GameSettings.Purification.HPDecreaseCorrection);

            // 最低値と最大値の範囲内に収める
            int min = GameSettings.Purification.MinSuccessRate;
            int max = GameSettings.Purification.MaxSuccsessRate;
            result = Mathf.Clamp(result, min, max);

            return result;
        }

        protected override Combatant CreateClone(string json)
        {
            Nightmare clone = JsonUtility.FromJson<Nightmare>(json);
            return clone;
        }
    }
}
