using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ステータスの表示
    /// </summary>
    /// <typeparam name="TView">表示の型</typeparam>
    /// <typeparam name="TValue">値の型</typeparam>
    public abstract class StatusDisplayer<TView, TValue> : Displayer<CombatantContainer, TView, TValue>, IStatusDisplayer
        where TView : MonoBehaviour
        where TValue : System.IComparable
    {
        public void SetTarget(CombatantContainer target)
        {
            this.target = target;
        }
    }
}
