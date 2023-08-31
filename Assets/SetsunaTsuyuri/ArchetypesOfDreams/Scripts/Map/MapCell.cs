using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マップセルの種類
    /// </summary>
    public enum MapCellType
    {
        None = 0,
        Floor = 1,
        Water = 2,

        Bridge = 11,

        Wall = 21
    }

    /// <summary>
    /// マップセル
    /// </summary>
    public class MapCell
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector2Int Position { get; set; } = Vector2Int.zero;

        /// <summary>
        /// セルタイプ配列
        /// </summary>
        public MapCellType[] Types { get; set; } = { };

        /// <summary>
        /// 空のセルである
        /// </summary>
        /// <returns></returns>
        public bool IsNone => Types.All(x => x == MapCellType.None);

        /// <summary>
        /// 進入可能である
        /// </summary>
        /// <param name="mapObject">進入するマップオブジェクト</param>
        /// <returns></returns>
        public bool IsAccessible(MapObject mapObject)
        {
            return Types
                .Intersect(mapObject.AccessibleCells)
                .Any();
        }
    }
}
