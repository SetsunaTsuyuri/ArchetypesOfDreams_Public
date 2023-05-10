using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

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
        AlliesParty allies = null;

        private void Awake()
        {
            allies = GetComponentInChildren<AlliesParty>();
        }

        private void Start()
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            StartAsync(token).Forget();
        }

        private async UniTask StartAsync(CancellationToken token)
        {
            // 戦闘者を移す
            allies.TransferCombatantsViriableDataToContainers();

            // UI初期化
            ui.Initialize();

            await FadeManager.FadeIn(token);

            ui.DungeonButtons.BeSelected();
        }
    }
}
