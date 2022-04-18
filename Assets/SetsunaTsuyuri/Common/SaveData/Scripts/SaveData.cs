using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SetsunaTsuyuri.ArchetypesOfDreams;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// セーブデータ
    /// </summary>
    [Serializable]
    public class SaveData : IInitializable
    {
        /// <summary>
        /// セーブデータID
        /// </summary>
        [field: SerializeField]
        public int Id { get; protected set; } = 0;

        /// <summary>
        /// セーブした日付と時刻
        /// </summary>
        [field: SerializeField]
        public string DateTime { get; protected set; } = System.DateTime.MinValue.ToString();

        /// <summary>
        /// 味方
        /// </summary>
        [field: SerializeReference]
        public Combatant[] Allies { get; set; } = { };

        /// <summary>
        /// 控えの味方
        /// </summary>
        [field: SerializeReference]
        public Combatant[] ReserveAllies { get; set; } = { };

        /// <summary>
        /// ダンジョン開放フラグ配列
        /// </summary>
        [field: SerializeField]
        public bool[] OpenDungeons { get; set; } = { };

        /// <summary>
        /// ダンジョンクリアフラグ配列
        /// </summary>
        [field: SerializeField]
        public bool[] ClearedDungeons { get; set; } = { };

        public SaveData()
        {
            Initialize();
        }

        public void Initialize()
        {
            int numberOfDungeons = MasterData.Dungeons.Data.Length;
            OpenDungeons = new bool[numberOfDungeons];
            ClearedDungeons = new bool[numberOfDungeons];

            Combatant player = new DreamWalker()
            {
                DataId = 0,
                Level = 1
            };
            player.Initialize();
            Allies = new Combatant[1];
            Allies[0] = player;
        }

        /// <summary>
        /// セーブする
        /// </summary>
        /// <param name="id">ID</param>
        public virtual void Save(int id = 0)
        {
            Id = id;
            DateTime = System.DateTime.Now.ToString();
        }
    }
}
