using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// スキルボタン
    /// </summary>
    public class SkillButton : GameButton
    {
        /// <summary>
        /// 名前の表示テキスト
        /// </summary>
        [field: SerializeField]
        public TextMeshProUGUI Name { get; set; } = null;

        /// <summary>
        /// コストの表示テキスト
        /// </summary>
        [field: SerializeField]
        public TextMeshProUGUI Cost { get; set; } = null;

        /// <summary>
        /// ボタンが選択されたときに実行するイベントトリガーエントリー
        /// </summary>
        readonly EventTrigger.Entry triggerOnSelected =
            new EventTrigger.Entry() { eventID = EventTriggerType.Select };

        /// <summary>
        /// ボタンが選択されたときに実行する関数
        /// </summary>
        UnityAction<BaseEventData> onSelected = (_) => { };

        /// <summary>
        /// ボタンがクリックされたときに実行する関数
        /// </summary>
        UnityAction onClicked = () => { };

        protected override void Awake()
        {
            base.Awake();

            Button.onClick.AddListener(onClicked);
            EventTrigger.triggers.Add(triggerOnSelected);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="skill">スキル</param>
        /// <param name="battle">戦闘の管理者</param>
        public void SetUp(Skill skill, BattleManager battle)
        {
            // 名前表示
            if (Name)
            {
                Name.text = skill.GetName();
            }

            // コスト表示
            if (Cost)
            {
                Cost.text = skill.Data.Cost.ToString();
            }

            // ボタン
            RemoveOnClickListener(onClicked);
            onClicked = () => battle.OnSkillSelected(skill);
            AddOnClickListener(onClicked);

            // イベントトリガー
            triggerOnSelected.callback.RemoveListener(onSelected);
            onSelected = (_) => battle.BattleUI.Description.SetText(skill.Data.Description);
            triggerOnSelected.callback.AddListener(onSelected);

            // インタラクト
            SetInteractable(skill.CanBeUsed(battle));
        }
    }
}
