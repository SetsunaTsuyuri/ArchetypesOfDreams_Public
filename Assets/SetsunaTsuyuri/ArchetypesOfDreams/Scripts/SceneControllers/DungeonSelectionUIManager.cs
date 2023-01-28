using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョン選択UIの管理者
    /// </summary>
    public class DungeonSelectionUIManager : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// 説明文UIの管理者
        /// </summary>
        DescriptionUI description = null;

        /// <summary>
        /// ダンジョン選択ボタンの管理者
        /// </summary>
        public DungeonButtonsManager DungeonButtons { get; private set; } = null;

        private void Awake()
        {
            description = GetComponentInChildren<DescriptionUI>(true);
            DungeonButtons = GetComponentInChildren<DungeonButtonsManager>(true);
        }

        public void Initialize()
        {
            DungeonButtons.SetUp(description);
        }
    }
}
