using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 選択可能なUIの基底クラス
    /// </summary>
    /// <typeparam name="TGameButton">ゲームボタン</typeparam>
    public abstract class SelectableGameUI<TGameButton> : GameUI, ISelectableGameUI
        where TGameButton : GameButton
    {
        /// <summary>
        /// キャンセルした際の遷移先UI
        /// </summary>
        public ISelectableGameUI Previous { get; set; } = null;

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

        public override void Hide()
        {
            base.Hide();

            // ボタンが選択されている場合、それを外す
            EventSystem eventSystem = EventSystem.current;
            if (_lastSelected &&
                _lastSelected.gameObject == eventSystem.currentSelectedGameObject)
            {
                eventSystem.SetSelectedGameObject(null);
            }
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        public virtual void SetUp()
        {
            // ボタンキャッシュ
            _buttons = GetComponentsInChildren<TGameButton>(true);

            // イベントトリガー登録
            for (int i = 0; i < _buttons.Length; i++)
            {
                // 選ばれたとき、自身を最後に選ばれたボタンとする
                int index = i;
                _buttons[i].AddTrrigerEntry(EventTriggerType.Select, _ =>
                {
                    _lastSelected = _buttons[index].Button;
                });

                // キャンセルされたとき、前のUIに戻る
                if (Previous != null)
                {
                    _buttons[i].AddTrrigerEntry(EventTriggerType.Cancel, _ =>
                    {
                        Previous.Select();
                        Hide();
                    });
                }
            }

            // ナビゲーションを更新する
            UpdateButtonNavigationsToLoop();
        }

        /// <summary>
        /// ボタンがループするようにナビゲーションを更新する
        /// </summary>
        protected void UpdateButtonNavigationsToLoop()
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
                    }
                    ,

                    VerticalLayoutGroup _ => () =>
                    {
                        navigation.selectOnDown = selectables[next];
                        navigation.selectOnUp = selectables[previous];
                    }
                    ,

                    _ => null
                };
                action?.Invoke();
                selectables[i].navigation = navigation;
            }
        }

        /// <summary>
        /// 選択状態にする
        /// </summary>
        /// <param name="selectLastSelected">最後に選ばれたボタンを選択する</param>
        public void Select(bool selectLastSelected = true)
        {
            Show();

            if (selectLastSelected
                && _lastSelected
                && _lastSelected.isActiveAndEnabled
                && _lastSelected.interactable)
            {
                _lastSelected.Select();
            }
            else
            {
                foreach (var button in _buttons)
                {
                    if (button.Button.interactable)
                    {
                        button.Button.Select();
                        break;
                    }
                }
            }
        }

        /// <summary>
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
