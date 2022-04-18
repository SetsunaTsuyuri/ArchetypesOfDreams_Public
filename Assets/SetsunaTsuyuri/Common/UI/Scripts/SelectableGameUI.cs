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
        public ISelectableGameUI Previous = null;

        /// <summary>
        /// ゲームボタン配列
        /// </summary>
        protected TGameButton[] buttons = null;

        /// <summary>
        /// 最後に選ばれたボタン
        /// </summary>
        protected Selectable lastSelected = null;

        /// <summary>
        /// レイアウトグループ
        /// </summary>
        protected LayoutGroup layoutGroup = null;

        protected override void Awake()
        {
            base.Awake();

            layoutGroup = GetComponentInChildren<LayoutGroup>(true);
            buttons = GetComponentsInChildren<TGameButton>(true);
        }

        /// <summary>
        /// ボタンをセットアップする
        /// </summary>
        public virtual void SetUpButtons()
        {
            // イベントトリガー登録
            for (int i = 0; i < buttons.Length; i++)
            {
                // 選ばれたとき、自身を最後に選ばれたボタンとする
                int index = i;
                buttons[i].AddTrriger(EventTriggerType.Select, (_) => lastSelected = buttons[index].Button);

                // キャンセルされたとき、前のUIに戻る
                buttons[i].AddTrriger(EventTriggerType.Cancel, (_) =>
                {
                    if (Previous != null)
                    {
                        Hide();
                        Previous.Select();
                    }
                });
            }

            UpdateButtonNavigationsToLoop();
        }

        /// <summary>
        /// ボタンがループするようにナビゲーションを更新する
        /// </summary>
        protected void UpdateButtonNavigationsToLoop()
        {
            Selectable[] selectables = buttons
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

                UnityAction action = layoutGroup switch
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

            if (selectLastSelected &&
                lastSelected &&
                lastSelected.isActiveAndEnabled &&
                lastSelected.interactable)
            {
                lastSelected.Select();
            }
            else
            {
                foreach (var button in buttons)
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
        /// 全てのボタンをアクティブまたは非アクティブにする
        /// </summary>
        /// <param name="value">値</param>
        public void SetActiveToAllButtons(bool value)
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// いずれかのボタンが押されるのを待つ
        /// </summary>
        /// <param name="token">トークン</param>
        /// <returns></returns>
        public async UniTask WaitForAnyButtonPressed(CancellationToken token)
        {
            await UniTask.Yield(token);

            UniTask[] tasks = buttons
                .Where(x => x.Button.interactable)
                .Select(x => x.Button.OnClickAsync(token))
                .ToArray();
            await UniTask.WhenAny(tasks);
        }
    }
}
