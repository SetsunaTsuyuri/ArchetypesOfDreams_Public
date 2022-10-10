using System;
using System.Collections;
using System.Collections.Generic;
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
    /// ゲーム実行中、常に保持するデータ
    /// </summary>
    public class RuntimeData : Singleton<RuntimeData>, IInitializable
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
        /// アイテム所持数
        /// </summary>
        int[] _items = null;

        /// <summary>
        /// アイテム所持数
        /// </summary>
        public static int[] Items
        {
            get => Instance._items;
            set => Instance._items = value;
        }

        /// <summary>
        /// ダンジョン開放フラグ
        /// </summary>
        bool[] _openDungeons = null;

        /// <summary>
        /// ダンジョン開放フラグ
        /// </summary>
        public static bool[] OpenDungeons
        {
            get => Instance._openDungeons;
            set => Instance._openDungeons = value;
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
        /// プレイするダンジョンのデータ
        /// </summary>
        DungeonData _dungeonToPlay = null;

        /// <summary>
        /// プレイするダンジョンのデータ
        /// </summary>
        public static DungeonData DungeonToPlay
        {
            get => Instance._dungeonToPlay;
            set => Instance._dungeonToPlay = value;
        }

        public override void Initialize()
        {
            _items = new int[MasterData.Items.Count()];

            int dungeonNumber = MasterData.Dungeons.Count();
            _openDungeons = new bool[dungeonNumber];
            _clearedDungeons = new bool[dungeonNumber];
            _obtainedTreasures = new bool[30];

            int storyNumber = Enum.GetValues(typeof(StoryType)).Length;
            _stroyProgressions = new int[storyNumber];

            _dungeonToPlay = MasterData.Dungeons[0];

            _allies.Clear();
            _resrveAllies.Clear();

            Combatant player = new DreamWalker()
            {
                DataId = 0,
                Level = 1
            };
            player.Initialize();
            Allies.Add(player);

            Items[0] = 3;
            Items[1] = 2;
            Items[2] = 1;
            Items[3] = 1;
            Items[4] = 2;
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
