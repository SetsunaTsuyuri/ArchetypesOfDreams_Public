using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 場所移動イベント
    /// </summary>
    [System.Serializable]
    public class TravelEvent : GameEventBase
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

        /// <summary>
        /// 移動先が自室である
        /// </summary>
        /// <returns></returns>
        public bool DestinationIsMyRoom => DungeonId == -1;

        public TravelEvent(string[] columns)
        {
            DungeonId = ToInt(columns, 1, -1);
            Position = ToEnum(columns, 2, PlayerInitialPositionType.Start);
        }

        public override async UniTask Resolve(CancellationToken token)
        {
            await GameEventsManager.ResolveTravelEvent(this, token);
        }
    }
}
