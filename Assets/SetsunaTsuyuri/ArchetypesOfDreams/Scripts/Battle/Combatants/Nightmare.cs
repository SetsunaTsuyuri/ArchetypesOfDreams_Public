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

        public override CombatantData GetData()
        {
            return MasterData.Nightmares.GetValue(DataId);
        }

        /// <summary>
        /// ナイトメアのデータとして取得する
        /// </summary>
        /// <returns></returns>
        public NightmareData GetNightmareData()
        {
            return GetData() as NightmareData;
        }

        /// <summary>
        /// 浄化成功率を取得する
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns></returns>
        public virtual int GetPurificationSuccessRate(DreamWalker dreamWalker)
        {
            // ボス耐性持ちには通じない
            if (HasBossResistance)
            {
                return 0;
            }

            // 成功率
            float rate = GetNightmareData().PurificationSuccessRate;

            // レベル差補正
            if (dreamWalker.Level > Level)
            {
                rate += (dreamWalker.Level - Level) * GameSettings.Purification.LevelDifferenceCorrection;
            }

            // 共感力補正
            rate += dreamWalker.Empathy * GameSettings.Purification.EmpathyCorrection;

            // 生命力補正
            rate *= 1.0f + (GetLifeReductionRate() * GameSettings.Purification.LifeReductionCorrection);

            // クラッシュ補正
            if (IsCrushed())
            {
                rate *= GameSettings.Purification.CrushCorrection;
            }

            // 0-100%の範囲内に収める
            int result = Mathf.Clamp(Mathf.FloorToInt(rate), 0, 100);
            return result;
        }
    }
}
