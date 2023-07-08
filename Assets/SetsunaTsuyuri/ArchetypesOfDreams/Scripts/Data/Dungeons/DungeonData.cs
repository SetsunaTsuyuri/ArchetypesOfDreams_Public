using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョンのデータ
    /// </summary>
    [System.Serializable]
    public class DungeonData : NameDescriptionData
    {
        /// <summary>
        /// マップJson
        /// </summary>
        public TextAsset MapJson = null;

        /// <summary>
        /// マップ
        /// </summary>
        public string Map = string.Empty;

        /// <summary>
        /// ダンジョン選択画面で選べない
        /// </summary>
        public bool CannotBeSelected = false;
    }
}
