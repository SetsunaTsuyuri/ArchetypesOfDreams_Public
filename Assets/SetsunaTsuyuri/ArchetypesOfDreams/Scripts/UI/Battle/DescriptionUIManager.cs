using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 説明文の管理者
    /// </summary>
    public class DescriptionUIManager : GameUI
    {
        /// <summary>
        /// 説明文
        /// </summary>
        TextMeshProUGUI text = null;

        protected override void Awake()
        {
            base.Awake();

            text = GetComponentInChildren<TextMeshProUGUI>(true);
        }

        /// <summary>
        /// 文章を設定する
        /// </summary>
        /// <param name="text">文章の文字列</param>
        public void SetText(string text)
        {
            this.text.text = text;
        }
    }
}
