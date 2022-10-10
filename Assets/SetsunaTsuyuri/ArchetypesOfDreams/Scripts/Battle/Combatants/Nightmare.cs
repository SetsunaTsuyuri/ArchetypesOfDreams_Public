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
            get => MasterData.Nightmares[DataId];
        }

        public override CombatantData Data
        {
            get => NightmareData;
        }

        public override int GetPurificationSuccessRate(Combatant purifier)
        {
            int result = 0;

            if (purifier is DreamWalker dreamWalker && !HasBossResistance)
            {
                // 成功率
                float rate = NightmareData.PurificationSuccessRate;

                // レベル差補正
                if (dreamWalker.Level > Level)
                {
                    rate += (dreamWalker.Level - Level) * GameSettings.Purification.LevelDifferenceCorrection;
                }

                // 共感力補正
                rate += dreamWalker.Empathy * GameSettings.Purification.EmpathyCorrection;

                // HP補正
                rate *= 1.0f + (GetHPReductionRate() * GameSettings.Purification.LifeReductionCorrection);

                // クラッシュ補正
                if (IsCrushed())
                {
                    rate *= GameSettings.Purification.CrushCorrection;
                }

                // 設定された範囲内に収める
                int min = GameSettings.Purification.MinSuccessRate;
                int max = GameSettings.Purification.MaxSuccsessRate;
                result = Mathf.Clamp(Mathf.FloorToInt(rate), min, max);
            }

            return result;
        }

        protected override Combatant CreateClone(string json)
        {
            Nightmare clone = JsonUtility.FromJson<Nightmare>(json);
            return clone;
        }
    }
}
