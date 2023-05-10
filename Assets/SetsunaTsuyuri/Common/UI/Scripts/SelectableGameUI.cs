using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 選択可能なUI
    /// </summary>
    /// <typeparam name="TGameButton">ゲームボタン</typeparam>
    public abstract class SelectableGameUI<TGameButton> : SelectableGameUIBase
        where TGameButton : GameButton
    {
        /// <summary>
        /// ゲームボタンの親トランスフォーム
        /// </summary>
        [SerializeField]
        protected Transform _buttonsRoot = null;

        /// <summary>
        /// ゲームボタン配列
        /// </summary>
        protected TGameButton[] _buttons = null;

        /// <summary>
        /// 最後に選ばれたボタン
        /// </summary>
        protected Selectable _lastSelected = null;

        /// <summary>
        /// レイアウトグループ
        /// </summary>
        protected LayoutGroup _layoutGroup = null;

        protected override void Awake()
        {
            base.Awake();

            _layoutGroup = GetComponentInChildren<LayoutGroup>(true);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        public virtual void SetUp()
        {
            // ボタン配列
            if (_buttonsRoot == null)
            {
                _buttonsRoot = transform;
            }
            _buttons = _buttonsRoot.GetComponentsInChildren<TGameButton>(true);

            // イベントトリガー登録
            for (int i = 0; i < _buttons.Length; i++)
            {
                // 選択
                int index = i;
                _buttons[i].AddTrriger(EventTriggerType.Select, _ => _lastSelected = _buttons[index].Button);

                // キャンセル
                _buttons[i].AddTrriger(EventTriggerType.Cancel, _ => BeCanceled());
            }

            // ナビゲーションを更新する
            UpdateButtonNavigations();
        }

        /// <summary>
        /// ボタンがループするようにナビゲーションを更新する
        /// </summary>
        protected void UpdateButtonNavigations()
        {
            Selectable[] selectables = _buttons
                .Where(x => x.isActiveAndEnabled)
                .Where(x => x.Button.interactable)
                .Select(x => x.GetComponent<Selectable>())
                .ToArray();

            int length = selectables.Length;
            for (int i = 0; i < length; i++)
            {
                Navigation navigation = selectables[i].navigation;
                navigation.mode = Navigation.Mode.Explicit;

                int previous = i == 0 ? length - 1 : i - 1;
                int next = (i + 1) % length;

                UnityAction action = _layoutGroup switch
                {
                    HorizontalLayoutGroup _ => () =>
                    {
                        navigation.selectOnRight = selectables[next];
                        navigation.selectOnLeft = selectables[previous];
                    },

                    VerticalLayoutGroup _ => () =>
                    {
                        navigation.selectOnDown = selectables[next];
                        navigation.selectOnUp = selectables[previous];
                    },

                    _ => null
                };
                action?.Invoke();
                selectables[i].navigation = navigation;
            }
        }

        public override void BeSelected()
        {
            base.BeSelected();

            SetEnabled(true);
            SelectAnyButton();
        }

        /// <summary>
        /// いずれかのボタンを選択する
        /// </summary>
        public void SelectAnyButton()
        {
            if (_lastSelected
                && _lastSelected.isActiveAndEnabled
                && _lastSelected.interactable)
            {
                _lastSelected.Select();
                return;
            }

            foreach (var button in _buttons)
            {
                if (button.Button.isActiveAndEnabled
                    && button.Button.interactable)
                {
                    button.Button.Select();
                    break;
                }
            }
        }

        public override void BeDeselected()
        {
            DeselectButton();
        }

        /// <summary>
        /// ボタンの選択を外す
        /// </summary>
        public void DeselectButton()
        {
            EventSystem eventSystem = EventSystem.current;
            if (_buttons.Any(x => x.gameObject == eventSystem.currentSelectedGameObject))
            {
                eventSystem.SetSelectedGameObject(null);
            };

        }

        public override void SetEnabled(bool enabled)
        {
            base.SetEnabled(enabled);

            if (!enabled)
            {
                DeselectButton();
            }
        }

        /// <summary>s
        /// 全てのボタンを表示する、または隠す
        /// </summary>
        /// <param name="value">値</param>
        public void ShowOrHideAllButtons(bool value)
        {
            foreach (var button in _buttons)
            {
                if (value)
                {
                    button.Show();
                }
                else
                {
                    button.Hide();
                }
            }
        }

        /// <summary>
        /// いずれかのボタンが押されるのを待つ
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask WaitForAnyButtonPressed(CancellationToken token)
        {
            await UniTask.Yield(token);

            UniTask[] tasks = _buttons
                .Where(x => x.Button.isActiveAndEnabled)
                .Where(x => x.Button.interactable)
                .Select(x => x.Button.OnClickAsync(token))
                .ToArray();

            if (tasks.Any())
            {
                await UniTask.WhenAny(tasks);
            }
        }
    }
}
