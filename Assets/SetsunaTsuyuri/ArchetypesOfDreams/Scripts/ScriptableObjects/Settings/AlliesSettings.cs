using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 味方の設定
    /// </summary>
    [CreateAssetMenu(fileName = "Allies", menuName = "Settings/Allies")]
    public class AlliesSettings : ScriptableObject
    {
        /// <summary>
        /// ダメージ時の振動
        /// </summary>
        [field: SerializeField]
        public Vector3 DamagePunch { get; private set; } = new Vector3(0.0f, 25.0f, 0.0f);

        /// <summary>
        /// ダメージ時の振動する時間
        /// </summary>
        [field: SerializeField]
        public float DamagePunchDuration { get; private set; } = 0.2f;

        /// <summary>
        /// ダメージ時の振動する回数
        /// </summary>
        [field: SerializeField]
        public int DamagePunchVibrato { get; private set; } = 25;
    }
}
