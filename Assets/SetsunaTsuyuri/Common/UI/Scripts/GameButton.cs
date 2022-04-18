using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// ゲームボタン(ボタンとイベントリガーを纏めたクラス)
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class GameButton : MonoBehaviour
    {
        /// <summary>
        /// ボタン
        /// </summary>
        public Button Button { get; private set; } = null;

        /// <summary>
        /// イベントトリガー
        /// </summary>
        public EventTrigger EventTrigger { get; private set; } = null;

        protected virtual void Awake()
        {
            Button = GetComponent<Button>();
            EventTrigger = GetComponent<EventTrigger>();

            AddTrriger(EventTriggerType.Submit, (_) => AudioManager.PlaySE("決定"));
            AddTrriger(EventTriggerType.PointerDown, (_) => AudioManager.PlaySE("決定"));
            
            AddTrriger(EventTriggerType.Move, (_) => AudioManager.PlaySE("ボタン移動"));
            AddTrriger(EventTriggerType.Cancel, (_) => AudioManager.PlaySE("キャンセル"));
        }

        /// <summary>
        /// ボタンのonClickイベントリスナーを追加する
        /// </summary>
        /// <param name="action">UnityAction</param>
        public void AddOnClickListener(UnityAction action)
        {
            Button.onClick.AddListener(action);
        }

        /// <summary>
        /// ボタンのonClickイベントリスナーを取り除く
        /// </summary>
        /// <param name="action">UnityAction</param>
        public void RemoveOnClickListener(UnityAction action)
        {
            Button.onClick.RemoveListener(action);
        }

        /// <summary>
        /// イベントトリガーを追加する
        /// </summary>
        /// <param name="button">ボタン</param>
        /// <param name="type">トリガーの種類</param>
        /// <param name="action">UnityAction</param>
        public void AddTrriger(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = type
            };

            entry.callback.AddListener(action);
            EventTrigger.triggers.Add(entry);
        }

        /// <summary>
        /// ボタンのinteractableを設定する
        /// </summary>
        /// <param name="interactable"></param>
        public void SetInteractable(bool interactable)
        {
            Button.interactable = interactable;
        }

        /// <summary>
        /// 選択する
        /// </summary>
        public void Select()
        {
            Button.Select();
        }
    }
}
