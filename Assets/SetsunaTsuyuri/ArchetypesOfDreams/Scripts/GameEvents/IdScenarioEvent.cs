using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ID指定でシナリオを再生するゲームイベント
    /// </summary>
    [Serializable]
    public class IdScenarioEvent : IGameEvent
    {
        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; set; } = 1;

        public IdScenarioEvent(string[] columns)
        {
            if (int.TryParse(columns[1], out int id))
            {
                Id = id;
            }
        }

        public UniTask Resolve(CancellationToken token)
        {
            return GameEventsManager.ResolveScenarioEvent(this, token);
        }
    }
}
