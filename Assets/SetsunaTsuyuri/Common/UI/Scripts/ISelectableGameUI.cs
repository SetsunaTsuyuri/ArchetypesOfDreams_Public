using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 選択できるUI
    /// </summary>
    public interface ISelectableGameUI
    {
        /// <summary>
        /// 選択する
        /// </summary>
        /// <param name="selectLastSelected">最後に選択されていたものを選択する</param>
        void Select(bool selectLastSelected = true);

        /// <summary>
        /// 隠す
        /// </summary>
        void Hide();
    }
}
