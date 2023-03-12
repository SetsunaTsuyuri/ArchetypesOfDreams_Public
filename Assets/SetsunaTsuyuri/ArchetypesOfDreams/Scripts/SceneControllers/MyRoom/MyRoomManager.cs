﻿using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 自室シーンの管理者
    /// </summary>
    public class MyRoomManager : MonoBehaviour
    {
        /// <summary>
        /// UI
        /// </summary>
        [SerializeField]
        MyRoomUIManager _ui = null;

        private void Start()
        {
            AlliesParty allies = GetComponentInChildren<AlliesParty>();

            allies.SetUp(null);

            // 戦闘者配列を味方コンテナへ移す
            allies.TransferCombatantsViriableDataToContainers();

            // 味方全員を全回復する
            allies.InitializeCombatantsStatus();

            // オートセーブを行う
            SaveDataManager.AutoSave();

            // BGMを再生する
            AudioManager.PlayBgm(BgmId.MyRoom);

            // UIをセットアップする
            _ui.SetUp(allies);

            // 自室メニューを選択する
            _ui.MyRoomMenu.BeSelected();
        }
    }
}