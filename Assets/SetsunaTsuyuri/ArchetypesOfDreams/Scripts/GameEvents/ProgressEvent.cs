using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 進行度イベント
    /// </summary>
    [System.Serializable]
    public class ProgressEvent : GameEventBase
    {
        /// <summary>
        /// 式の種類
        /// </summary>
        public enum FormulaType
        {
            None = 0,
            Assign = 1,
            Add = 2,
            Subtruct = 3
        }

        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; set; } = 0;

        /// <summary>
        /// 式の種類
        /// </summary>
        [field: SerializeField]
        public FormulaType Formula { get; set; } = 0;

        /// <summary>
        /// パラメーター
        /// </summary>
        [field: SerializeField]
        public int Parameter { get; set; } = 0;

        public ProgressEvent(string[] columns)
        {
            Id = ToInt(columns, 1, 1);
            Formula = ToEnum<FormulaType>(columns, 2);
            Parameter = ToInt(columns, 3);
        }

        public override async UniTask Resolve(CancellationToken token)
        {
            await UniTask.CompletedTask;
            token.ThrowIfCancellationRequested();

            switch (Formula)
            {
                case FormulaType.Assign:
                    VariableData.Variables.Set(Id, Parameter);
                    break;

                case FormulaType.Add:
                    VariableData.Variables.Add(Id, Parameter);
                    break;

                case FormulaType.Subtruct:
                    VariableData.Variables.Subtarct(Id, Parameter);
                    break;

                default:
                    break;
            }
        }
    }
}
