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
            AllyContainersManager allies = GetComponentInChildren<AllyContainersManager>();

            // 戦闘者配列を味方コンテナへ移す
            allies.TransferCombatantsRuntimeDataToContainers();

            // 味方全員を全回復する
            allies.InitializeCombatantsStatus();

            // オートセーブを行う
            SaveDataManager.SaveAuto();

            // BGMを再生する
            AudioManager.PlayBgm(BgmType.MyRoom);

            // メニューをセットアップする
            menu.SetUp();

            // メニューを選択する
            menu.Select();
        }
    }
}
