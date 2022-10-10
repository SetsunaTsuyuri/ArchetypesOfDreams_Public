using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 進行度によるマップイベント出現要因
    /// </summary>
    public abstract class ProgressionTrigger : IAppearanceTrigger
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

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formula"></param>
        /// <param name="parameter"></param>
        public ProgressionTrigger(int? id, FormulaType? formula, int? parameter)
        {
            // ID
            if (id.HasValue)
            {
                Id = id.Value;
            }

            // 式
            if (formula.HasValue)
            {
                Formula = formula.Value;
            }

            // パラメーター
            if (parameter.HasValue)
            {
                Parameter = parameter.Value;
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

