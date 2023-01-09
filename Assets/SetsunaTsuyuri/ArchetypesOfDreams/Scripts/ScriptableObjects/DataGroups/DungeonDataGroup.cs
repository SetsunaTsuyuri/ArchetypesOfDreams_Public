using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョンのデータ集
    /// </summary>
    [CreateAssetMenu(fileName = "Dungeons", menuName = "Data/Dungeons")]
    public class DungeonDataGroup : DataGroup<DungeonData>
    {
        /// <summary>
        /// 選択可能なダンジョンデータ配列を取得する
        /// </summary>
        /// <returns></returns>
        public DungeonData[] GetSelectables()
        {
            return Data
                .Where(x => !x.CannotBeSelected)
                .ToArray();
        }
    }
}
