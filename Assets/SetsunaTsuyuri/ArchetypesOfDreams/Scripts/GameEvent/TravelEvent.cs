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
        /// 移動先のマップID
        /// </summary>
        [field: SerializeField]
        public int MapId { get; set; } = -1;

        public UniTask GetUniTask(CancellationToken token)
        {
            return GameEventsManager.ResolveTravelEvent(this, token);
        }

        /// <summary>
        /// 移動先が自室である
        /// </summary>
        /// <returns></returns>
        public bool DestinationIsMyRoom()
        {
            return MapId == -1;
        }
    }
}
