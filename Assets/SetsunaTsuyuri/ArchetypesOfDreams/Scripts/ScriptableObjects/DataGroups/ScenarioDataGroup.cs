using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// シナリオデータ集
    /// </summary>
    [CreateAssetMenu(fileName = "Scenarios", menuName = "Data/Scenarios")]
    public class ScenarioDataGroup : DataGroup<ScenarioData>
    {
        /// <summary>
        /// 汎用シナリオ集
        /// </summary>
        [field: SerializeField]
        public KeysAndValues<GameAttribute.Scenario, TextAsset> CommonScenarios { get; private set; }
    }
}
