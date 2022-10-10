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
    public class ActionButton : GameButton
    {
        /// <summary>
        /// 名前の表示テキスト
        /// </summary>
        [field: SerializeField]
        public TextMeshProUGUI Name { get; set; } = null;

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

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="owner">ボタンの管理者</param>
        public void SetUp(ISelectableGameUI owner)
        {
            // イベントリスナー登録
            Button.onClick.AddListener(() => owner.Hide());

            Hide();
        }

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="action">行動内容</param>
        /// <param name="battle">戦闘の管理者</param>
        public virtual void UpdateButton(ActionModel action, BattleManager battle)
        {
            // 名前表示
            if (Name)
            {
                Name.text = action.Name;
            }

            // ボタン
            RemoveOnClickListener(_onClicked);
            _onClicked = () => battle.OnSkillSelected(action);
            AddOnClickListener(_onClicked);

            // イベントトリガーエントリー
            _entry.callback.RemoveListener(_onSelected);
            _onSelected = (_) => battle.BattleUI.Description.SetText(action.Description);
            _entry.callback.AddListener(_onSelected);

            // インタラクト
            SetInteractable(action.CanBeExecuted(battle));
        }
    }
}
