using System;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲーム変数によるマップイベント出現条件
    /// </summary>
    public class VariableCondition : IAppearanceCondition
    {
        /// <summary>
        /// 式の種類
        /// </summary>
        public enum FormulaType
        {
            None = 0,
            Equal = 1,
            OrMore = 2,
            OrLess = 3
        }

        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; private set; } = 0;

        /// <summary>
        /// 式
        /// </summary>
        [field: SerializeField]
        public FormulaType Formula { get; private set; } = FormulaType.Equal;

        /// <summary>
        /// パラメーター
        /// </summary>
        [field: SerializeField]
        public int Parameter { get; private set; } = 0;

        public VariableCondition(string[] columns)
        {
            if (int.TryParse(columns[1], out int id))
            {
                Id = id;
            }

            if (Enum.TryParse(columns[2], out FormulaType formula))
            {
                Formula = formula;
            }

            if (int.TryParse(columns[3], out int parameter))
            {
                Parameter = parameter;
            }
        }

        /// <summary>
        /// 評価する
        /// </summary>
        /// <returns></returns>
        public bool Evaluate()
        {
            int value = VariableData.Variables.GetValueOrDefault(Id);
            bool result = Formula switch
            {
                FormulaType.Equal => value == Parameter,
                FormulaType.OrMore => value >= Parameter,
                FormulaType.OrLess => value <= Parameter,
                _ => false
            };

            return result;
        }
    }
}

