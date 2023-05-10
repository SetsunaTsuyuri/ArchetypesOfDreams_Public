using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// プレイヤーの初期位置の種類
    /// </summary>
    public enum PlayerInitialPositionType
    {
        Start = 0,
        Goal = 1
    }

    /// <summary>
    /// ゲーム実行中に変動するデータ
    /// </summary>
    public class VariableData : Singleton<VariableData>, IInitializable
    {
        /// <summary>
        /// 味方
        /// </summary>
        public static List<Combatant> Allies => Instance._allies;
        readonly List<Combatant> _allies = new();

        /// <summary>
        /// 控えの味方
        /// </summary>
        public static List<Combatant> ReserveAllies => Instance._resrveAllies;
        readonly List<Combatant> _resrveAllies = new();

        /// <summary>
        /// 精気
        /// </summary>
        public static int Spirit
        {
            get => Instance._spirit;
            set => Instance._spirit = Math.Clamp(value, 0, GameSettings.Other.MaxSpirit);
        }
        int _spirit = 0;

        /// <summary>
        /// 歩数
        /// </summary>
        public static int Steps
        {
            get => Instance._steps;
            set => Instance._steps = value;
        }
        int _steps = 0;

        /// <summary>
        /// アイテム
        /// </summary>
        public static ItemsController Items => Instance._items;
        readonly ItemsController _items = new();

        /// <summary>
        /// イベント進行度
        /// </summary>
        public static ProgressesController Progresses => Instance._progresses;
        readonly ProgressesController _progresses = new();

        /// <summary>
        /// イベントフラグ
        /// </summary>
        public static FlagsController Flags => Instance.flagsManager;
        readonly FlagsController flagsManager = new();
        
        /// <summary>
        /// 選択可能なダンジョンフラグ
        /// </summary>
        bool[] _selectableDungeons = null;

        /// <summary>
        /// 選択可能なダンジョンフラグ
        /// </summary>
        public static bool[] SelectableDungeons
        {
            get => Instance._selectableDungeons;
            set => Instance._selectableDungeons = value;
        }

        /// <summary>
        /// ダンジョンクリアフラグ
        /// </summary>
        bool[] _clearedDungeons = null;

        /// <summary>
        /// ダンジョンクリアフラグ
        /// </summary>
        public static bool[] ClearedDungeons
        {
            get => Instance._clearedDungeons;
            set => Instance._clearedDungeons = value;
        }

        /// <summary>
        /// トレジャー獲得フラグ
        /// </summary>
        bool[] _obtainedTreasures = null;

        /// <summary>
        /// トレジャー獲得フラグ
        /// </summary>
        public static bool[] ObtainedTreasures
        {
            get => Instance._obtainedTreasures;
            set => Instance._obtainedTreasures = value;
        }

        /// <summary>
        /// プレイするダンジョンID
        /// </summary>
        int _dungeonId = 0;

        /// <summary>
        /// プレイするダンジョンID
        /// </summary>
        public static int DungeonId
        {
            get => Instance._dungeonId;
            set => Instance._dungeonId = value;
        }

        /// <summary>
        /// プレイヤーの初期位置
        /// </summary>
        PlayerInitialPositionType _playerInitialPosition = PlayerInitialPositionType.Start;

        /// <summary>
        /// プレイヤーの初期位置
        /// </summary>
        public static PlayerInitialPositionType PlayerInitialPosition
        {
            get => Instance._playerInitialPosition;
            set => Instance._playerInitialPosition = value;
        }

        public override void Initialize()
        {
            // プレイするダンジョンID
            DungeonId = 0;

            // 精貨
            _spirit = 0;

            // 歩数
            _steps = 0;

            // アイテム
            Items.Initialize();

            // 進行度
            Progresses.Initialize();

            // フラグ
            Flags.Initialize();

            // ダンジョン
            int dungeonNumber = MasterData.CountDungeons();
            _selectableDungeons = new bool[dungeonNumber];
            _clearedDungeons = new bool[dungeonNumber];
            _obtainedTreasures = new bool[30];

            // 味方パーティ
            _allies.Clear();
            _resrveAllies.Clear();
            Combatant player = new DreamWalker()
            {
                DataId = 1,
                Level = 1
            };
            player.Initialize();
            Allies.Add(player);

            // 選択可能なダンジョン
            _selectableDungeons[1] = true;
        }

        /// <summary>
        /// 味方をセーブする
        /// </summary>
        public static void SaveAllies()
        {
            AlliesParty allies = AlliesParty.InstanceInActiveScene;
            if (!allies)
            {
                return;
            }

            // 味方
            Allies.Clear();
            var members = allies.Members
                .Where(x => x.ContainsCombatant)
                .Select(x => x.Combatant);

            foreach (var member in members)
            {
                Allies.Add(member);
            }

            // 控えの味方
            ReserveAllies.Clear();
            var reseveMembers = allies.ReserveMembers
                .Where(x => x.ContainsCombatant)
                .Select(x => x.Combatant);

            foreach (var reserveMember in reseveMembers)
            {
                ReserveAllies.Add(reserveMember);
            }
        }
    }
}
