using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// シナリオイベント
    /// </summary>
    public class ScenarioEvent : IGameEvent
    {
        /// <summary>
        /// 名前
        /// </summary>
        [field: SerializeField]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 文章
        /// </summary>
        [field: SerializeField]
        public string Message { get; set; } = string.Empty;

        public UniTask GetUniTask(CancellationToken token)
        {
            return UniTask.CompletedTask;
        }
    }
}
