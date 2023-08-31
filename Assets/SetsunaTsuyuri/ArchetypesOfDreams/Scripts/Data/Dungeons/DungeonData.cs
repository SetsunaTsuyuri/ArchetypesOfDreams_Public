using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョンのデータ
    /// </summary>
    [System.Serializable]
    public class DungeonData : NameDescriptionData
    {
        /// <summary>
        /// マップ
        /// </summary>
        public string Map = string.Empty;

        /// <summary>
        /// ダンジョン選択画面で選べない
        /// </summary>
        public bool CannotBeSelected = false;

        /// <summary>
        /// BGMID
        /// </summary>
        public int BgmId = 0;

        /// <summary>
        /// 戦闘BGMID
        /// </summary>
        public int BattleBgmId = 0;

        /// <summary>
        /// ランダムエンカウントで出現する敵
        /// </summary>
        public RandomEncounterEnemyData[] Enemies = {};
    }
}
