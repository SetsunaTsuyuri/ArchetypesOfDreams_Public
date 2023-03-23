using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 場所移動イベント
    /// </summary>
    [Serializable]
    public class TravelEvent : IGameEvent
    {
        /// <summary>
        /// 移動先のダンジョンID
        /// </summary>
        [field: SerializeField]
        public int DungeonId { get; set; } = -1;

        /// <summary>
        /// 位置
        /// </summary>
        [field: SerializeField]
        public PlayerInitialPositionType Position { get; set; } = PlayerInitialPositionType.Start;

        public TravelEvent(string[] columns)
        {
            if (int.TryParse(columns[1], out int id))
            {
                DungeonId = id;
            }

            if (columns.Length > 2
                && Enum.TryParse(columns[2], out PlayerInitialPositionType position))
            {
                Position = position;
            }
        }

        public UniTask Resolve(CancellationToken token)
        {
            return GameEventsManager.ResolveTravelEvent(this, token);
        }

        /// <summary>
        /// 移動先が自室である
        /// </summary>
        /// <returns></returns>
        public bool DestinationIsMyRoom()
        {
            return DungeonId == -1;
        }
    }
}
