using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マップイベントオブジェクトの出現要因
    /// </summary>
    public interface IAppearanceTrigger
    {
        /// <summary>
        /// 条件を満たしているかどうか評価する
        /// </summary>
        /// <returns></returns>
        bool Evaluate();
    }
}
