using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 進行度ID
    /// </summary>
    public enum VariableId
    {
        None = 0,

        /// <summary>
        /// 本編1
        /// </summary>
        Story1 = 1,

        /// <summary>
        /// 自室イベント
        /// </summary>
        MyRoom = 101,
    }

    /// <summary>
    /// イベント変数の管理クラス
    /// </summary>
    public class VariablesController : DictionaryController<int, int>, IInitializable
    {
        public override void Initialize()
        {
            base.Initialize();

            Dictionary.Add((int)VariableId.Story1, 0);
            Dictionary.Add((int)VariableId.MyRoom, 0);
        }

        /// <summary>
        /// 取得する
        /// </summary>
        /// <param name="progressId"></param>
        /// <returns></returns>
        public int Get(VariableId progressId)
        {
            return GetValueOrDefault((int)progressId);
        }

        /// <summary>
        /// 設定する
        /// </summary>
        /// <param name="progressId"></param>
        /// <param name="value"></param>
        public void Set(VariableId progressId, int value)
        {
            TrySetValue((int)progressId, value);
        }

        /// <summary>
        /// 加算する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Add(int id, int value)
        {
            if (!Dictionary.ContainsKey(id))
            {
                return;
            }

            Dictionary[id] += value;
        }

        /// <summary>
        /// 減算する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Subtarct(int id, int value)
        {
            if (!Dictionary.ContainsKey(id))
            {
                return;
            }

            Dictionary[id] -= value;
        }
    }
}
