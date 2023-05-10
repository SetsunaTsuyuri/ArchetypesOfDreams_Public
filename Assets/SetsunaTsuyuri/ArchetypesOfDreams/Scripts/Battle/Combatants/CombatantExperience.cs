using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Combatant
    {
        /// <summary>
        /// 経験値が設定されたときの処理
        /// </summary>
        private void OnExperienceSet()
        {
            int previousLevel = Level;

            Level = ExperienceToLevel();

            if (Level != previousLevel)
            {
                RefreshStatus();
            }
        }

        /// <summary>
        /// 経験値をレベルに変換する
        /// </summary>
        /// <returns></returns>
        private int ExperienceToLevel()
        {
            int level = GameSettings.Combatants.MinLevel;
            while (Experience >= ToMinExperience(level + 1))
            {
                level++;
            }

            return level;
        }

        /// <summary>
        /// そのレベルに至るために必要な最低経験値を求める
        /// </summary>
        /// <param name="level">レベル</param>
        /// <returns></returns>
        public int ToMinExperience(int level)
        {
            CombatantsSettings combatants = GameSettings.Combatants;

            // 最小レベル
            int minLevel = combatants.MinLevel;

            // 基本値
            int baseValue = combatants.ExperienceRequiredToLevelUp;

            // 増加倍率
            float rate = combatants.PercentageIncreaceInExperienceRequiredToLevelUp;

            // 合計値
            int sum = 0;

            for (int i = minLevel; i < level; i++)
            {
                // 実際の倍率
                float levelRate = rate * (i - 1) + 1.0f;

                // 経験値を合計値に加える
                int experience = Mathf.FloorToInt(baseValue * levelRate);
                sum += experience;
            }

            return sum;
        }

        /// <summary>
        /// 次のレベルに到達するまでに必要な経験値を計算する
        /// </summary>
        /// <returns></returns>
        public int CalculateNextLevelExperience()
        {
            // 次のレベルまでに必要な最低経験値から現在値を引く
            int next = ToMinExperience(Level + 1);
            next -=  Experience;
            return next;
        }

        /// <summary>
        /// 敵として倒された場合に得られる経験値を計算する
        /// </summary>
        /// <returns></returns>
        public int CalculateRewardExperience()
        {
            int experience = Data.RewardExperience;
            float increace = GameSettings.Combatants.PercentageIncreaceInExperienceRequiredToLevelUp;
            float rate = (Level - 1) * increace + 1.0f;
            int result = Mathf.FloorToInt(experience * rate);
            return result;
        }
    }
}
