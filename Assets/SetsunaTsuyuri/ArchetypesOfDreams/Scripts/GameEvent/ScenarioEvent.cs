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
    public class ScenarioEvent : IGameEvent
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

        public ScenarioEvent(string[] column)
        {
            if (int.TryParse(column[1], out int actorId))
            {
                ActorId = actorId;
            }

            if (int.TryParse(column[2], out int expressionId))
            {
                ExpressionId = expressionId;
            }

            Name = column[3];
            Message = column[4];
        }

        public async UniTask Resolve(CancellationToken token)
        {
            ScenarioManager scenario = ScenarioManager.InstanceInActiveScene;
            if (!scenario)
            {
                return;
            }

            // TODO: 連続して文章を表示する場合、シナリオウィンドウを閉じないようにする
            await scenario.Play(ActorId, ExpressionId, Name, Message, token);
        }
    }
}
