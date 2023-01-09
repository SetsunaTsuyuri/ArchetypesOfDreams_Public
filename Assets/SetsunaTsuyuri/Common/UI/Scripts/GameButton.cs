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
        public static readonly float InteractableTextAlpha = 1.0f;
        public static readonly float NonInteractableTextAlpha = 0.5f;

        /// <summary>
        /// キャンバスグループ
        /// </summary>
        CanvasGroup _canvasGroup = null;

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
            _canvasGroup = GetComponent<CanvasGroup>();
            Button = GetComponent<Button>();
            EventTrigger = GetComponent<EventTrigger>();

            AddTrrigerEntry(EventTriggerType.Submit, (_) => AudioManager.PlaySE("決定"));
            AddTrrigerEntry(EventTriggerType.Move, (_) => AudioManager.PlaySE("ボタン移動"));
            AddTrrigerEntry(EventTriggerType.Cancel, (_) => AudioManager.PlaySE("キャンセル"));
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
        /// イベントトリガーエントリーを追加する
        /// </summary>
        /// <param name="button">ボタン</param>
        /// <param name="type">トリガーの種類</param>
        /// <param name="action">UnityAction</param>
        public void AddTrrigerEntry(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger.Entry entry = new()
            {
                eventID = type
            };

            entry.callback.AddListener(action);
            EventTrigger.triggers.Add(entry);
        }

        public void SetInteractable(bool interactable)
        {
            Button.interactable = interactable;

            if (interactable)
            {
                _canvasGroup.alpha = InteractableTextAlpha;
            }
            else
            {
                _canvasGroup.alpha = NonInteractableTextAlpha;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
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
