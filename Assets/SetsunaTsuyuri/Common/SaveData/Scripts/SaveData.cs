﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SetsunaTsuyuri.ArchetypesOfDreams;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// セーブデータ
    /// </summary>
    [Serializable]
    public class SaveData
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
        /// 精貨
        /// </summary>
        [field: SerializeField]
        public int SpiritCoins { get; set; } = 0;

        /// <summary>
        /// 歩数
        /// </summary>
        [field: SerializeField]
        public int Steps { get; set; } = 0;

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
        /// 選択可能なダンジョンフラグ配列
        /// </summary>
        [field: SerializeField]
        public bool[] SelectableDungeons { get; set; } = { };

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

        /// <summary>
        /// セーブする
        /// </summary>
        /// <param name="id">ID</param>
        public void Save(int id = 0)
        {
            Id = id;
            DateTime = System.DateTime.Now.ToString();

            Copy(VariableData.Allies, Allies);
            Copy(VariableData.ReserveAllies, ReserveAllies);

            SpiritCoins = VariableData.SpiritCoins;
            Steps = VariableData.Steps;

            Items = VariableData.ItemsDic.Values.ToArray();

            StroyProgressions = Copy(VariableData.StoryProgressions);
            SelectableDungeons = Copy(VariableData.SelectableDungeons);
            ClearedDungeons = Copy(VariableData.ClearedDungeons);
            ObtainedTreasures = Copy(VariableData.ObtainedTreasures);
        }

        /// <summary>
        /// ロードする
        /// </summary>
        public void Load()
        {
            Copy(Allies, VariableData.Allies);
            Copy(ReserveAllies, VariableData.ReserveAllies);

            VariableData.SpiritCoins = SpiritCoins;
            VariableData.Steps = Steps;

            VariableData.ItemsDic.Clear();
            for (int i = 0; i < Items.Length; i++)
            {
                VariableData.ItemsDic.Add(i + 1, Items[i]);
            }

            Overwrite(StroyProgressions, VariableData.StoryProgressions);
            Overwrite(SelectableDungeons, VariableData.SelectableDungeons);
            Overwrite(ClearedDungeons, VariableData.ClearedDungeons);
            Overwrite(ObtainedTreasures, VariableData.ObtainedTreasures);
        }

        /// <summary>
        /// コピーする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private T[] Copy<T>(T[] source) where T : struct
        {
            T[] destination = new T[source.Length];
            Array.Copy(source, destination, source.Length);
            return destination;
        }

        /// <summary>
        /// 上書きする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void Overwrite<T>(T[] from, T[] to) where T : struct
        {
            int min = Math.Min(from.Length, to.Length);
            Array.Copy(from, to, min);
        }

        /// <summary>
        /// 戦闘者リストをコピーする
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
