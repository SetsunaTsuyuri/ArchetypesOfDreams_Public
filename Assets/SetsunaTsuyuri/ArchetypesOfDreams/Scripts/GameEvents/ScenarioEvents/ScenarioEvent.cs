using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SetsunaTsuyuri.Scenario;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// シナリオイベント
    /// </summary>
    public class ScenarioEvent : GameEventBase
    {
        /// <summary>
        /// 演者ID
        /// </summary>
        [field: SerializeField]
        public int ActorId { get; set; } = 0;

        /// <summary>
        /// 表情ID
        /// </summary>
        [field: SerializeField]
        public int ExpressionId { get; set; } = 0;

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

        public ScenarioEvent(string[] columns)
        {
            ActorId = ToInt(columns, 1);
            ExpressionId = ToInt(columns, 2);
            Name = GetString(columns, 3);
            Message = GetString(columns, 4);
        }

        public override async UniTask Resolve(CancellationToken token)
        {
            ScenarioManager scenario = ScenarioManager.InstanceInActiveScene;
            if (!scenario)
            {
                return;
            }

            await scenario.Play(ActorId, ExpressionId, Name, Message, token);
        }
    }
}
