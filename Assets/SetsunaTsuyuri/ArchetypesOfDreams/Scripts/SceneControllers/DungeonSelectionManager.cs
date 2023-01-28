using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョン選択の管理者
    /// </summary>
    public class DungeonSelectionManager : MonoBehaviour
    {
        /// <summary>
        /// ダンジョン選択UIの管理者
        /// </summary>
        [SerializeField]
        DungeonSelectionUIManager ui;

        /// <summary>
        /// 味方コンテナの管理者
        /// </summary>
        AllyContainersManager allies = null;

        private void Awake()
        {
            allies = GetComponentInChildren<AllyContainersManager>();
        }

        private void Start()
        {
            // 戦闘者を移す
            allies.TransferCombatantsViriableDataToContainers();

            // UI初期化
            ui.Initialize();

            // ボタン選択
            ui.DungeonButtons.BeSelected();
        }
    }
}
