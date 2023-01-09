using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// シナリオのゲームコマンド
    /// </summary>
    [Serializable]
    public class ScenarioEvent : IGameEvent
    {
        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; set; } = 1;

        public ScenarioEvent(string[] columns)
        {
            if (int.TryParse(columns[1], out int id))
            {
                Id = id;
            }
        }

        public UniTask GetUniTask(CancellationToken token)
        {
            return GameEventsManager.ResolveScenarioEvent(this, token);
        }
    }
}
