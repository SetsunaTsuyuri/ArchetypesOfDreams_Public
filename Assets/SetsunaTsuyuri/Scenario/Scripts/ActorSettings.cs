using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 演者の設定
    /// </summary>
    [CreateAssetMenu(menuName = "SetsunaTsuyuri/Scenario/Settings/Actor")]
    public class ActorSettings : ScriptableObject
    {
        /// <summary>
        /// 暗いときの色
        /// </summary>
        [field: SerializeField]
        public Color DarkColor { get; private set; } = Color.gray;

        /// <summary>
        /// フェード時間
        /// </summary>
        [field: SerializeField]
        public float FadeDuration { get; private set; } = 0.3f;
    }
}
