using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// SEタイプ
    /// </summary>
    public enum SEType
    {
        Damage,
        Healing,
        CursolMove,
        Select,
        Cancel,
        Purification,
        Collapse,
        BattleStart
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
        /// <param name="type">SEタイプ</param>
        /// <returns></returns>
        public AudioData this[SEType type] => this[(int)type];
    }
}
