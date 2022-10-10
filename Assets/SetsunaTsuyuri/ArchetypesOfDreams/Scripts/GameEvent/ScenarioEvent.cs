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
        /// シナリオの属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Scenario ScenarioAttribute { get; private set; } = Attribute.Scenario.MainStory;

        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; set; } = 1;

        public UniTask GetUniTask(CancellationToken token)
        {
            return GameEventsManager.ResolveScenarioEvent(this, token);
        }
    }
}
