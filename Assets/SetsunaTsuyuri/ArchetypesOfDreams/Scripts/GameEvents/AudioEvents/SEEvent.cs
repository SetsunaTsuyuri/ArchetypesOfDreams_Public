using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// SEイベント
    /// </summary>
    public class SEEvent : GameEventBase
    {
        /// <summary>
        /// デフォルト音量
        /// </summary>
        private static readonly float s_defaultVolume = 1.0f;

        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; set; } = 0;

        /// <summary>
        /// 音量
        /// </summary>
        [field: SerializeField]
        public float Volume { get; set; } = 0.0f;

        public SEEvent(string[] columns)
        {
            Id = ToInt(columns, 1);
            Volume = ToFloat(columns, 2, s_defaultVolume);
        }

        public override async UniTask Resolve(CancellationToken token)
        {
            AudioManager.PlaySE(Id);
            await UniTask.CompletedTask;
        }
    }
}
