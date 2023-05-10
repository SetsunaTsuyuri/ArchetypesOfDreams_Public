using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動メニュー
    /// </summary>
    /// <typeparam name="TActionButton"></typeparam>
    public abstract class ActionMenu<TActionButton> : SelectableGameUI<TActionButton> where TActionButton : ActionButton
    {
        /// <summary>
        /// 使用者
        /// </summary>
        CombatantContainer _user = null;

        /// <summary>
        /// 使用者
        /// </summary>
        public virtual CombatantContainer User
        {
            get => _user;
            set => _user = value;
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="description">説明文UI</param>
        /// <param name="targetSelection">対象選択UI</param>
        public void SetUp(DescriptionUI description, TargetSelectionUI targetSelection)
        {
            SetUp();

            foreach (var button in _buttons)
            {
                button.SetUp(description);

                button.AddPressedListener(() =>
                {
                    OnActionTargetSelection(targetSelection, button.Id);
                    Stack(typeof(TargetSelectionUI));
                    SetEnabled(false);
                });
            }

            SetEnabled(false);
        }

        /// <summary>
        /// 対象を選択する
        /// </summary>
        /// <param name="targetSelection">対象選択UI</param>
        /// <param name="id">ID</param>
        protected abstract void OnActionTargetSelection(TargetSelectionUI targetSelection, int id);

        public override void BeSelected()
        {
            UpdateButtons();

            base.BeSelected();
        }

        public override void BeCanceled()
        {
            SetEnabled(false);

            base.BeCanceled();
        }

        /// <summary>
        /// ボタンを更新する
        /// </summary>
        protected void UpdateButtons()
        {
            // 初期化
            foreach (var button in _buttons)
            {
                button.Initialize();
            }

            // 更新
            (int id, bool canBeUsed)[] ids = GetIdAndCanBeUsed();

            int min = Mathf.Min(_buttons.Length, ids.Length);
            for (int i = 0; i < min; i++)
            {
                _buttons[i].UpdateButton(ids[i].id, ids[i].canBeUsed);
            }

            UpdateButtonNavigations();
        }

        /// <summary>
        /// IDと使用可否のタプル配列を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract (int, bool)[] GetIdAndCanBeUsed();
    }
}
