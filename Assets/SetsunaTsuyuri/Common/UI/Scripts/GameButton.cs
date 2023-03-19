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
        bool _isSealed = false;

        /// <summary>
        /// 封印されている
        /// </summary>
        public bool IsSeald
        {
            get => _isSealed;
            set
            {
                _isSealed = value;
                UpdateAlpha();
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

            // 移動
            AddTrriger(EventTriggerType.Move, e =>
            {
                MoveDirection direction = GetMoveDirection(e);
                Navigation navigation = Button.navigation;

                bool canMove = direction switch
                {
                    MoveDirection.Left => navigation.selectOnLeft != null,
                    MoveDirection.Up => navigation.selectOnUp != null,
                    MoveDirection.Right => navigation.selectOnRight != null,
                    MoveDirection.Down => navigation.selectOnDown != null,
                    _ => false
                };

                if (canMove)
                {
                    AudioManager.PlaySE(SEId.FocusMove);
                }
            });

            // キャンセル
            AddTrriger(EventTriggerType.Cancel, _ => AudioManager.PlaySE(SEId.Cancel));
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
                AudioManager.PlaySE(SEId.Cancel);
                return;
            }

            AudioManager.PlaySE(SEId.Select);
            _pressed.Invoke();
        }

        /// <summary>
        /// ボタンのonClickイベントリスナーを追加する
        /// </summary>
        /// <param name="action"></param>
        public void AddPressedListener(UnityAction action)
        {
            _pressed.AddListener(action);
        }

        /// <summary>
        /// ボタンのonClickイベントリスナーを取り除く
        /// </summary>
        /// <param name="action"></param>
        public void RemovePressedListener(UnityAction action)
        {
            _pressed.RemoveListener(action);
        }

        /// <summary>
        /// ボタンのonClickイベントリスナーを全て取り除く
        /// </summary>
        public void RemoveAllPressedListener()
        {
            _pressed.RemoveAllListeners();
        }

        /// <summary>
        /// イベントトリガーエントリーを追加する
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
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
            UpdateAlpha();
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
            EventTrigger.triggers.Clear();
        }

        /// <summary>
        /// 移動方向を取得する
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public MoveDirection GetMoveDirection(BaseEventData eventData)
        {
            MoveDirection direction = MoveDirection.None;
            if (eventData is AxisEventData axis)
            {
                direction = axis.moveDir;
            }

            return direction;
        }

        /// <summary>
        /// アルファ値を更新する
        /// </summary>
        private void UpdateAlpha()
        {
            if (!_canvasGroup)
            {
                return;
            }

            _canvasGroup.alpha = Button.interactable && !IsSeald ?
                GameButtonsSettings.InteractableAlpha :
                GameButtonsSettings.NonInteractableAlpha;
        }
    }
}
