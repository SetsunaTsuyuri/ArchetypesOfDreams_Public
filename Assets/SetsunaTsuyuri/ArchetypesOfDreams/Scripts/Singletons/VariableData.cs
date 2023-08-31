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
        public static int Energy
        {
            get => Instance._energy;
            set => Instance._energy = Math.Clamp(value, 0, GameSettings.Other.MaxEnergy);
        }
        int _energy = 0;

        /// <summary>
        /// 歩数
        /// </summary>
        public static int Steps
        {
            get => Instance._steps;
            set => Instance._steps = Mathf.Clamp(value, 0, GameSettings.Other.MaxSteps);
        }
        int _steps = 0;

        /// <summary>
        /// アイテム
        /// </summary>
        public static ItemsController Items => Instance._items;
        readonly ItemsController _items = new();

        /// <summary>
        /// ダンジョン
        /// </summary>
        public static DungeonStatusesController Dungeons => Instance._dungeons;
        readonly DungeonStatusesController _dungeons = new();

        /// <summary>
        /// イベント変数
        /// </summary>
        public static VariablesController Variables => Instance._variables;
        readonly VariablesController _variables = new();

        /// <summary>
        /// イベントフラグ
        /// </summary>
        public static FlagsController Flags => Instance.flags;
        readonly FlagsController flags = new();

        /// <summary>
        /// プレイするダンジョンID
        /// </summary>
        int _dungeonId = 1;

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
            DungeonId = 1;

            // プレイヤー初期位置
            PlayerInitialPosition = PlayerInitialPositionType.Start;

            // 精気
            _energy = 0;

            // 歩数
            _steps = 0;

            // アイテム
            Items.Initialize();

            // ダンジョン
            Dungeons.Initialize();

            // 進行度
            Variables.Initialize();

            // フラグ
            Flags.Initialize();

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
