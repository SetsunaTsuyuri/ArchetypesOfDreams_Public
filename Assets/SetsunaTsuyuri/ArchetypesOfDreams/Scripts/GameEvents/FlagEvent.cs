using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// フラグイベント
    /// </summary>
    [System.Serializable]
    public class FlagEvent : GameEventBase
    {
        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; set; } = 0;

        /// <summary>
        /// 値
        /// </summary>
        [field: SerializeField]
        public bool Value { get; set; } = false;

        public FlagEvent(string[] columns) 
        {
            Id = ToInt(columns, 1);
            Value = ToBool(columns, 2);
        }

        public override async UniTask Resolve(CancellationToken token)
        {
            await UniTask.CompletedTask;
            token.ThrowIfCancellationRequested();

            VariableData.Flags.TrySetValue(Id, Value);
        }
    }
}
