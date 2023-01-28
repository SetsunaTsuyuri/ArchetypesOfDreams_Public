using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// ゲームボタン(ボタンとイベントリガーを纏めたクラス)
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class GameButton : MonoBehaviour, IInitializable
    {
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

        /// <summary>
        /// 封印されている
        /// </summary>
        bool isSealed = false;

        /// <summary>
        /// 封印されている
        /// </summary>
        public bool IsSeald
        {
            get => isSealed;
            set
            {
                isSealed = value;
                if (isSealed)
                {
                    _canvasGroup.alpha = GameButtonsSettings.NonInteractableAlpha;
                }
                else
                {
                    _canvasGroup.alpha = GameButtonsSettings.InteractableAlpha;
                }
            }
        }

        /// <summary>
        /// 押されたときのイベント
        /// </summary>
        readonly UnityEvent _pressed = new();

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Button = GetComponent<Button>();
            EventTrigger = GetComponent<EventTrigger>();

            Button.onClick.AddListener(OnPressed);

            AddPressedListener(() => AudioManager.PlaySE(SEType.Select));
            AddTrriger(EventTriggerType.Move, _ => AudioManager.PlaySE(SEType.CursolMove));
            AddTrriger(EventTriggerType.Cancel, _ => AudioManager.PlaySE(SEType.Cancel));
        }

        public virtual void Initialize()
        {
            IsSeald = false;
        }

        /// <summary>
        /// 押されたときの処理
        /// </summary>
        private void OnPressed()
        {
            if (IsSeald)
            {
                AudioManager.PlaySE(SEType.Cancel);
                return;
            }

            _pressed.Invoke();
        }

        /// <summary>
        /// ボタンのonClickイベントリスナーを追加する
        /// </summary>
        /// <param name="action">UnityAction</param>
        public void AddPressedListener(UnityAction action)
        {
            _pressed.AddListener(action);
        }

        /// <summary>
        /// ボタンのonClickイベントリスナーを取り除く
        /// </summary>
        /// <param name="action">UnityAction</param>
        public void RemovePressedListener(UnityAction action)
        {
            _pressed.RemoveListener(action);
        }

        /// <summary>
        /// イベントトリガーエントリーを追加する
        /// </summary>
        /// <param name="button">ボタン</param>
        /// <param name="type">トリガーの種類</param>
        /// <param name="action">UnityAction</param>
        public void AddTrriger(EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger.Entry entry = new()
            {
                eventID = type
            };

            entry.callback.AddListener(action);
            EventTrigger.triggers.Add(entry);
        }

        /// <summary>
        /// interactableを設定する
        /// </summary>
        /// <param name="interactable"></param>
        public void SetInteractable(bool interactable)
        {
            Button.interactable = interactable;

            if (interactable)
            {
                _canvasGroup.alpha = GameButtonsSettings.InteractableAlpha;
            }
            else
            {
                _canvasGroup.alpha = GameButtonsSettings.NonInteractableAlpha;
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

        /// <summary>
        /// ロックする
        /// </summary>
        public void Lock()
        {
            Button.navigation = new Navigation();
            Button.onClick.RemoveAllListeners();
        }
    }
}
