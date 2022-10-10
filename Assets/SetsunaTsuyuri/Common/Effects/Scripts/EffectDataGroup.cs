using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// エフェクトタイプ
    /// </summary>
    public enum EffectType
    {
        Purification = 0
    }

    /// <summary>
    /// エフェクトデータグループ
    /// </summary>
    [CreateAssetMenu(fileName = "Effects", menuName = "SetsunaTsuyuri/Effects")]
    public class EffectDataGroup : DataGroup<EffectData> { }
}
