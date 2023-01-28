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
        /// ボタンが選択されたときに実行するイベントトリガーエントリー
        /// </summary>
        readonly EventTrigger.Entry _entry = new()
        {
            eventID = EventTriggerType.Select
        };

        /// <summary>
        /// ボタンが選択されたときに実行する関数
        /// </summary>
        UnityAction<BaseEventData> _onSelected = (_) => { };

        /// <summary>
        /// ボタンがクリックされたときに実行する関数
        /// </summary>
        UnityAction _onClicked = () => { };

        protected override void Awake()
        {
            base.Awake();

            Button.onClick.AddListener(_onClicked);
            EventTrigger.triggers.Add(_entry);
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
        /// セットアップする
        /// </summary>
        /// <param name="description">説明文</param>
        public void SetUp(DescriptionUI description)
        {
            AddTrriger(EventTriggerType.Select, _ => description.SetText(Description));
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="owner">ボタンの管理者</param>
        public void SetUp(SelectableGameUIBase owner)
        {
            // イベントリスナー
            AddPressedListener(() => owner.Hide());

            Hide();
        }

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="action">行動内容</param>
        /// <param name="battle">戦闘の管理者</param>
        public virtual void UpdateButton(ActionInfo action, Battle battle)
        {
            // 名前表示
            if (Name)
            {
                Name.text = action.Name;
            }

            // ボタン
            RemovePressedListener(_onClicked);
            _onClicked = () => battle.OnSkillSelected(action);
            AddPressedListener(_onClicked);

            // イベントトリガーエントリー
            _entry.callback.RemoveListener(_onSelected);
            _onSelected = (_) => battle.BattleUI.Description.SetText(action.Description);
            _entry.callback.AddListener(_onSelected);

            SetInteractable(action.CanBeExecuted(battle));
        }
    }
}
