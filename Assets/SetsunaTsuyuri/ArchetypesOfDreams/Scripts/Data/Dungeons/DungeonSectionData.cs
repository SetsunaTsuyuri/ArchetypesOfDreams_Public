using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョンセクションのデータ
    /// </summary>
    [System.Serializable]
    public class DungeonSectionData
    {
        /// <summary>
        /// ランダムに出現する敵グループIDの最小値
        /// </summary>
        [field: SerializeField]
        public int MinRandomGroupId { get; private set; } = 0;

        /// <summary>
        /// ランダムに出現する敵グループIDの最大値
        /// </summary>
        [field: SerializeField]
        public int MaxRandomGroupId { get; private set; } = 0;

        /// <summary>
        /// ゲームコマンド配列
        /// </summary>
        [field: SerializeField]
        public GameCommand[] GameCommands { get; private set; } = { };
    }
}
