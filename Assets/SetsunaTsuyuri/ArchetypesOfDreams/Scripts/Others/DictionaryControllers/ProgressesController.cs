using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 進行度ID
    /// </summary>
    public enum ProgressId
    {
        None = 0,

        /// <summary>
        /// 本編
        /// </summary>
        Story = 1,

        /// <summary>
        /// 自室イベント
        /// </summary>
        MyRoom = 2,
    }

    /// <summary>
    /// 進行度の管理クラス
    /// </summary>
    public class ProgressesController : DicionaryController<int, int>, IInitializable
    {
        public override void Initialize()
        {
            base.Initialize();

            Dictionary.Add((int)ProgressId.Story, 0);
            Dictionary.Add((int)ProgressId.MyRoom, 0);
        }

        /// <summary>
        /// 取得する
        /// </summary>
        /// <param name="progressId"></param>
        /// <returns></returns>
        public int Get(ProgressId progressId)
        {
            return Get((int)progressId);
        }

        /// <summary>
        /// 設定する
        /// </summary>
        /// <param name="progressId"></param>
        /// <param name="value"></param>
        public void Set(ProgressId progressId, int value)
        {
            Set((int)progressId, value);
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
