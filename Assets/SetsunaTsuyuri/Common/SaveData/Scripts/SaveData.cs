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
        public List<Combatant> Allies { get; set; } = new();

        /// <summary>
        /// 控えの味方
        /// </summary>
        [field: SerializeReference]
        public List<Combatant> ReserveAllies { get; set; } = new();

        /// <summary>
        /// アイテム所持数配列
        /// </summary>
        [field: SerializeField]
        public int[] Items { get; set; } = { };

        /// <summary>
        /// 物語の進行度
        /// </summary>
        [field: SerializeField]
        public int[] StroyProgressions { get; set; } = { };

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

        /// <summary>
        /// トレジャー獲得フラグ配列
        /// </summary>
        [field: SerializeField]
        public bool[] ObtainedTreasures { get; set; } = { };

        public SaveData()
        {
            Initialize();
        }

        public void Initialize()
        {
            Items = new int[MasterData.Items.Count()];

            int dungeonNumber = MasterData.Dungeons.Count();
            OpenDungeons = new bool[dungeonNumber];
            ClearedDungeons = new bool[dungeonNumber];
            ObtainedTreasures = new bool[30];
        }

        /// <summary>
        /// セーブする
        /// </summary>
        /// <param name="id">ID</param>
        public void Save(int id = 0)
        {
            Id = id;
            DateTime = System.DateTime.Now.ToString();

            Copy(RuntimeData.Allies, Allies);
            Copy(RuntimeData.ReserveAllies, ReserveAllies);
            Copy(RuntimeData.Items, Items);
            Copy(RuntimeData.StoryProgressions, StroyProgressions);
            Copy(RuntimeData.OpenDungeons, OpenDungeons);
            Copy(RuntimeData.ClearedDungeons, ClearedDungeons);
            Copy(RuntimeData.ObtainedTreasures, ObtainedTreasures);
        }

        /// <summary>
        /// ロードする
        /// </summary>
        public void Load()
        {
            Copy(Allies, RuntimeData.Allies);
            Copy(ReserveAllies, RuntimeData.ReserveAllies);
            Copy(Items, RuntimeData.Items);
            Copy(StroyProgressions, RuntimeData.StoryProgressions);
            Copy(OpenDungeons, RuntimeData.OpenDungeons);
            Copy(ClearedDungeons, RuntimeData.ClearedDungeons);
            Copy(ObtainedTreasures, RuntimeData.ObtainedTreasures);
        }

        /// <summary>
        /// コピーする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void Copy<T>(T[] from, T[] to) where T : struct
        {
            int min = Math.Min(from.Length, to.Length);

            for (int i = 0; i < min; i++)
            {
                to[i] = from[i];
            }
        }

        /// <summary>
        /// コピーする
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void Copy(List<Combatant> from, List<Combatant> to)
        {
            to.Clear();
            foreach (var combatant in from)
            {
                Combatant clone = combatant.Clone();
                to.Add(clone);
            }
        }
    }
}