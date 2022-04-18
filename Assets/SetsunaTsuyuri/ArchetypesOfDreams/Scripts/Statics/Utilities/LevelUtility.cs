using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// レベル関連の便利なクラス
    /// </summary>
    public static class LevelUtility
    {
        /// <summary>
        /// 経験値をレベルに変換する
        /// </summary>
        /// <param name="experience">経験値</param>
        /// <returns></returns>
        public static int ToLevel(int experience)
        {
            // レベル
            int level = GameSettings.Combatants.MinLevel;
            //int requestedExperience =;

            return 0;
        }

        /// <summary>
        /// レベルを経験値に変換する
        /// </summary>
        /// <param name="level">レベル</param>
        /// <returns></returns>
        public static int ToExperience(int level)
        {
            return 0;
        }
    }
}
