using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// TextMeshProUGUIによるステータスの表示
    /// </summary>
    /// <typeparam name="TTarget">対象の型</typeparam>
    public abstract class StatusText<TValue> : StatusDisplayer<TextMeshProUGUI, TValue>
        where TValue : System.IComparable
    {
        protected override void UpdateValue()
        {
            if (target.ContainsCombatant)
            {
                value = GetValue(target.Combatant);
            }
        }

        /// <summary>
        /// 値を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract TValue GetValue(Combatant combatant);
    }
}
