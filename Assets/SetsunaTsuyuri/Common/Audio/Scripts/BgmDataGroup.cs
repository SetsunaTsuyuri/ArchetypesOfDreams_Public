using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// BGMID
    /// </summary>
    public enum BgmId
    {
        Title = 1,
        MyRoom = 2,
        NormalBattle = 3,
        BossBattle = 4,
        Dungeon = 5
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
        /// <param name="id">BGMID</param>
        /// <returns></returns>
        public AudioData this[BgmId id] => this[(int)id];
    }
}
