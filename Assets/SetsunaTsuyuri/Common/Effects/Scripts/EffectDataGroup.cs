using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// エフェクトデータID
    /// </summary>
    public enum EffectDataId
    {
        None = 0,

        /// <summary>
        /// 浄化
        /// </summary>
        Purification = 1
    }

    /// <summary>
    /// エフェクトデータグループ
    /// </summary>
    [CreateAssetMenu(fileName = "Effects", menuName = "SetsunaTsuyuri/Effects")]
    public class EffectDataGroup : DataGroup<EffectObjectData> { }
}
