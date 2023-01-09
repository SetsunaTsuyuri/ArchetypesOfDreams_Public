using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 進行度によるマップイベント出現条件
    /// </summary>
    public abstract class ProgressionCondition : IAppearanceCondition
    {
        /// <summary>
        /// 式の種類
        /// </summary>
        public enum FormulaType
        {
            Equal = 0,
            OrMore = 1,
            OrLess = 2
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

        public ProgressionCondition(string[] columns)
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

        public bool Evaluate()
        {
            bool result = false;

            int value = GetValue();
            switch (Formula)
            {
                case FormulaType.Equal:
                    result = value == Parameter;
                    break;

                case FormulaType.OrMore:
                    result = value >= Parameter;
                    break;

                case FormulaType.OrLess:
                    result = value <= Parameter;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 値を取得する
        /// </summary>
        /// <returns></returns>
        public abstract int GetValue();
    }
}

