using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// シーン変更ボタン
    /// </summary>
    public class SceneChangeButton : GameButton
    {
        /// <summary>
        /// シーンの種類
        /// </summary>
        [field: SerializeField]
        public SceneType SceneType { get; set; } = SceneType.Title;

        /// <summary>
        /// 説明文
        /// </summary>
        [SerializeField]
        string _description = string.Empty;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="description"></param>
        public void SetUp(DescriptionUI description)
        {
            AddTrriger(EventTriggerType.Select, _ => description.SetText(_description));
            AddPressedListener(() =>
            {
                SceneChangeManager.StartChange(SceneType);
                Lock();
            });
        }
    }
}
