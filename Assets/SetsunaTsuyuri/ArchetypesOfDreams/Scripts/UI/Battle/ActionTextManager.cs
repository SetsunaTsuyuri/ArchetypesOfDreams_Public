using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動テキストのUI管理者
    /// </summary>
    public class ActionTextManager : GameUI
    {
        /// <summary>
        /// テキスト
        /// </summary>
        TextMeshProUGUI text = null;

        protected override void Awake()
        {
            base.Awake();
            text = GetComponentInChildren<TextMeshProUGUI>(true);
            
            Hide();
        }

        /// <summary>
        /// スキル名を表示する
        /// </summary>
        /// <param name="name">名前</param>
        public void DisplaySkillName(Skill skill)
        {
            text.text = skill.GetName();
            Show();
        }
    }
}
