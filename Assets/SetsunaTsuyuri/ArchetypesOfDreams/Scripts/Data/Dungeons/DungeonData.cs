using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョンのデータ
    /// </summary>
    [System.Serializable]
    public class DungeonData : DataWithIdAndName
    {
        /// <summary>
        /// 敵の基本レベル
        /// </summary>
        [field: SerializeField]
        public int BasicEnemyLevel { get; private set; } = 1;

        /// <summary>
        /// ダンジョン選択画面で選べない
        /// </summary>
        [field: SerializeField]
        public bool CannotBeSelected { get; private set; } = false;

        /// <summary>
        /// クリア後、自身のID+1のIDを持つダンジョンを開放する
        /// </summary>
        [field: SerializeField]
        public bool OpenNextDungeon { get; private set; } = true;

        /// <summary>
        /// クリア後、開放するダンジョンのID配列
        /// </summary>
        [field: SerializeField]
        public int[] OpenDungeonsId { get; private set; } = { };

        /// <summary>
        /// ダンジョンセクションデータ配列
        /// </summary>
        [field: SerializeField]
        public DungeonSectionData[] DungeonSections { get; private set; } = { };
    }
}
