using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョン選択UIの管理者
    /// </summary>
    public class DungeonSelectionUIManager : MonoBehaviour
    {
        /// <summary>
        /// ダンジョン選択ボタンの管理者
        /// </summary>
        public DungeonButtonsManager DungeonButtons { get; private set; } = null;

        private void Awake()
        {
            DungeonButtons = GetComponentInChildren<DungeonButtonsManager>();
        }

        public void SetUp()
        {
            // ダンジョン選択ボタン
            DescriptionUI description = GetComponentInChildren<DescriptionUI>();
            DungeonButtons.SetUp(description);

            // 味方UI
            AlliesUI allies = GetComponentInChildren<AlliesUI>();
            allies.SetUp();
        }
    }
}
