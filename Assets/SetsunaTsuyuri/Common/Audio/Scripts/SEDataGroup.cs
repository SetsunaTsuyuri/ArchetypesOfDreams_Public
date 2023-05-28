using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// SEID
    /// </summary>
    public enum SEId
    {
        None = 0,
        Damage = 1,
        Healing = 2,
        FocusMove = 3,
        Select = 4,
        Cancel = 5,
        Purification = 6,
        Collapse = 7,
        BattleStart = 8,
        Escape = 9,
        EnemyDamage = 10,
        Slash1 = 101,
    }

    /// <summary>
    /// SEデータグループ
    /// </summary>
    [CreateAssetMenu(fileName = "SE", menuName = "SetsunaTsuyuri/Audio/SE")]
    public class SEDataGroup : AudioDataGroup
    {
        /// <summary>
        /// インデクサー
        /// </summary>
        /// <param name="id">SEID</param>
        /// <returns></returns>
        public AudioData this[SEId id] => this[(int)id];
    }
}
