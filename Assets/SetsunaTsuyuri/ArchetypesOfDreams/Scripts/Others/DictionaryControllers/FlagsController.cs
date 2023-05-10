using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// フラグID
    /// </summary>
    public enum FlagId
    {
        None = 0,

        /// <summary>
        /// 帰還ボタンが表示される
        /// </summary>
        ReturnButtonIsVisible = 101,
    }

    /// <summary>
    /// フラグの管理クラス
    /// </summary>
    public class FlagsController : DicionaryController<int, bool>, IInitializable
    {
        public override void Initialize()
        {
            base.Initialize();

            Dictionary.Clear();
            Dictionary.Add((int)FlagId.ReturnButtonIsVisible, false);
        }

        /// <summary>
        /// 取得する
        /// </summary>
        /// <param name="flagId"></param>
        /// <returns></returns>
        public bool Get(FlagId flagId)
        {
            return Get((int)flagId);
        }
        /// <summary>
        /// 設定する
        /// </summary>
        /// <param name="flagId"></param>
        /// <param name="value"></param>
        public void Set(FlagId flagId, bool value)
        {
            Set((int)flagId, value);
        }
    }
}
