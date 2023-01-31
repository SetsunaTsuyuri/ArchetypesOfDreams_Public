using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動ボタン
    /// </summary>
    public class ActionButton : GameButton, IInitializable
    {
        /// <summary>
        /// 名前の表示テキスト
        /// </summary>
        [field: SerializeField]
        public TextMeshProUGUI Name { get; set; } = null;

        /// <summary>
        /// 数字の表示テキスト
        /// </summary>
        [field: SerializeField]
        public TextMeshProUGUI Number { get; set; } = null;

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; protected set; } = 0;

        /// <summary>
        /// 説明文
        /// </summary>
        protected string Description = string.Empty;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="description">説明文</param>
        public void SetUp(DescriptionUI description)
        {
            AddTrriger(EventTriggerType.Select, _ => description.SetText(Description));
        }

        public override void Initialize()
        {
            base.Initialize();

            Name.text = string.Empty;
            Number.text = string.Empty;
            Description = string.Empty;
            SetInteractable(false);
        }

        /// <summary>
        /// ボタンを更新する
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="canBeUsed">使用できる</param>
        public virtual void UpdateButton(int id, bool canBeUsed)
        {
            SetInteractable(true);
            
            Id = id;
            IsSeald = !canBeUsed;
        }

        /// <summary>
        /// 各テキストを更新する
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="getNumberFunc">数を取得する関数</param>
        protected void UpdateTexts(NameDescriptionData data, System.Func<string> getNumberFunc)
        {
            Description = data.Description;

            // 名前
            if (Name)
            {
                Name.text = data.Name;
            }

            // 数
            if (Number)
            {
                Number.text = getNumberFunc?.Invoke() ?? string.Empty;
            }
        }
    }
}
