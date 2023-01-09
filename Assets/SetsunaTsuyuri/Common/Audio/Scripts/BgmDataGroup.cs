using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// BGMタイプ
    /// </summary>
    public enum BgmType
    {
        Title,
        MyRoom,
        NormalBattle,
        BossBattle,
        Dungeon
    }

    /// <summary>
    /// BGMデータグループ
    /// </summary>
    [CreateAssetMenu(fileName = "BGM", menuName = "SetsunaTsuyuri/Audio/BGM")]
    public class BgmDataGroup : AudioDataGroup
    {
        /// <summary>
        /// インデクサー
        /// </summary>
        /// <param name="type">BGMタイプ</param>
        /// <returns></returns>
        public AudioData this[BgmType type] => this[(int)type];
    }
}
