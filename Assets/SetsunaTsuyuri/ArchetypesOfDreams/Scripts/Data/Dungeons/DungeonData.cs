using System.Collections;
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
        /// ダンジョン選択画面で選べない
        /// </summary>
        public bool CannotBeSelected = false;
    }
}
