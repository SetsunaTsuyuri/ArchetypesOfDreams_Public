using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲージによるInt型のステータス表示
    /// </summary>
    public abstract class StatusGauge : StatusDisplayer<Image, int>
    {
        /// <summary>
        /// 最大値
        /// </summary>
        protected int maxValue;

        /// <summary>
        /// 最後に表示された最大値
        /// </summary>
        protected int theLastMaxValue;

        protected override void UpdateView()
        {
            float amount = (float)value / maxValue;
            view.fillAmount = amount;
        }

        protected override void UpdateValue()
        {
            if (target.ContainsCombatant())
            {
                (int, int) result = GetValues(target.Combatant);
                value = result.Item1;
                maxValue = result.Item2;
            }
        }

        protected override void UpdateTheLastValue()
        {
            base.UpdateTheLastValue();

            theLastMaxValue = maxValue;
        }

        /// <summary>
        /// 値を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract (int, int) GetValues(Combatant combatant);
    }
}