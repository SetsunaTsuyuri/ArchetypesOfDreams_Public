using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ストーリータイプ
    /// </summary>
    public enum StoryType
    {
        Main = 0
    }

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
        readonly List<Combatant> _allies = new();

        /// <summary>
        /// 味方
        /// </summary>
        public static List<Combatant> Allies
        {
            get => Instance._allies;
        }

        /// <summary>
        /// 控えの味方
        /// </summary>
        readonly List<Combatant> _resrveAllies = new();

        /// <summary>
        /// 控えの味方
        /// </summary>
        public static List<Combatant> ReserveAllies
        {
            get => Instance._resrveAllies;
        }

        /// <summary>
        /// 精貨
        /// </summary>
        int _spiritCoins = 0;

        /// <summary>
        /// 精貨
        /// </summary>
        public static int SpiritCoins
        {
            get => Instance._spiritCoins;
            set => Instance._spiritCoins = Math.Clamp(value, 0, GameSettings.Other.MaxSpiritCoins);
        }

        /// <summary>
        /// 歩数
        /// </summary>
        int _steps = 0;

        /// <summary>
        /// 歩数
        /// </summary>
        public static int Steps
        {
            get => Instance._steps;
            set => Instance._steps = value;
        }

        /// <summary>
        /// アイテム所持数ディクショナリー
        /// </summary>
        readonly Dictionary<int, int> _itemsDic = new();

        /// <summary>
        /// アイテム所持数ディクショナリー
        /// </summary>
        public static Dictionary<int, int> ItemsDic => Instance._itemsDic;

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
        /// 物語の進行度
        /// </summary>
        int[] _stroyProgressions = null;

        /// <summary>
        /// 物語の進行度
        /// </summary>
        public static int[] StoryProgressions
        {
            get => Instance._stroyProgressions;
            set => Instance._stroyProgressions = value;
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
            // 精貨
            _spiritCoins = 0;
            
            // 歩数
            _steps = 0;

            // アイテム所持数ディクショナリー
            _itemsDic.Clear();
            int items = MasterData.CountItems();
            for (int i = 0; i < items; i++)
            {
                _itemsDic.Add(i, 0);
            }

            // ダンジョン
            int dungeonNumber = MasterData.CountDungeons();
            _selectableDungeons = new bool[dungeonNumber];
            _clearedDungeons = new bool[dungeonNumber];
            _obtainedTreasures = new bool[30];

            // ストーリー
            int storyNumber = Enum.GetValues(typeof(StoryType)).Length;
            _stroyProgressions = new int[storyNumber];

            _allies.Clear();
            _resrveAllies.Clear();

            // 味方パーティ
            Combatant player = new DreamWalker()
            {
                DataId = 0,
                Level = 1
            };
            player.Initialize();
            Allies.Add(player);

            // アイテム
            ItemsDic[0] = 10;
            ItemsDic[1] = 10;
            ItemsDic[3] = 10;
            ItemsDic[4] = 10;

            // 選択可能なダンジョン
            _selectableDungeons[1] = true;
        }

        /// <summary>
        /// 味方を設定する
        /// </summary>
        /// <param name="allies"></param>
        public static void SetAllies(IEnumerable<Combatant> allies)
        {
            Allies.Clear();
            foreach (var ally in allies)
            {
                Allies.Add(ally);
            }
        }

        /// <summary>
        /// 控えの味方を設定する
        /// </summary>
        /// <param name="reserveAllies"></param>
        public static void SetReserveAllies(IEnumerable<Combatant> reserveAllies)
        {
            ReserveAllies.Clear();
            foreach (var ally in reserveAllies)
            {
                ReserveAllies.Add(ally);
            }
        }
    }
}
