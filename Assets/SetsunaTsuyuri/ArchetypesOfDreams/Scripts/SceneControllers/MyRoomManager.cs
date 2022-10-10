using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 自室の管理者
    /// </summary>
    public class MyRoomManager : MonoBehaviour
    {
        /// <summary>
        /// 自室メニュー
        /// </summary>
        [SerializeField]
        MyRoomMenu menu = null;

        private void Start()
        {
            // セーブデータの戦闘者を味方コンテナへ移す
            AllyContainersManager allies = GetComponentInChildren<AllyContainersManager>();
            allies.TransferCombatantsSaveDataToContainers();

            // 味方全員を全回復する
            allies.InitializeCombatantsStatus();

            // オートセーブを行う
            SaveDataManager.SaveAuto();

            // BGMを再生する
            AudioManager.PlayBgm("自室");

            // メニューをセットアップし、選択する
            menu.SetUp();
            menu.Select();
        }
    }
}
