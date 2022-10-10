using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// シナリオデータ
    /// </summary>
    [System.Serializable]
    public class ScenarioData : Data
    {
        /// <summary>
        /// CSVテキスト
        /// </summary>
        [field: SerializeField]
        public TextAsset CSVText { get; private set; }
    }
}
