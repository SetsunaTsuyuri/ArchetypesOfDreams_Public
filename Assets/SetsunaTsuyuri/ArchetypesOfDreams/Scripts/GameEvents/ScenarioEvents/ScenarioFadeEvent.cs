using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SetsunaTsuyuri.Scenario;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// シナリオフェードイベント
    /// </summary>
    public class ScenarioFadeEvent : GameEventBase
    {
        /// <summary>
        /// フェード時間
        /// </summary>
        static readonly float s_defaultDuration = 0.5f;

        /// <summary>
        /// フェードタイプ
        /// </summary>
        [field: SerializeField]
        public Scenario.Attribute.CommandOfScreen Command { get; set; } = Scenario.Attribute.CommandOfScreen.None;

        /// <summary>
        /// 時間
        /// </summary>
        [field: SerializeField]
        public float Duration { get; set; } = 0.0f;

        public ScenarioFadeEvent(string[] columns)
        {
            Command = ToEnum<Scenario.Attribute.CommandOfScreen>(columns, 1);
            Duration = ToFloat(columns, 2, s_defaultDuration);
        }

        public override async UniTask Resolve(CancellationToken token)
        {
            ScenarioManager scenario = ScenarioManager.InstanceInActiveScene;
            if (!scenario)
            {
                return;
            }

            await scenario.FadeScreen(Command, Duration, token);
        }
    }
}
