using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 有効性の設定
    /// </summary>
    [CreateAssetMenu(menuName = "Settings/Effectiveness")]
    public class EffectivenessSettings : ScriptableObject
    {
        /// <summary>
        /// 有効性倍率
        /// </summary>
        [field: SerializeField]
        public KeysAndValues<Attribute.Effectiveness, float> Rates { get; private set; }
    }
}
