using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 対象選択UI
    /// </summary>
    public class TargetSelectionUI : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// ボタン
        /// </summary>
        GameButton _button = null;

        /// <summary>
        /// 味方
        /// </summary>
        ICombatantContainerManager _allies = null;

        /// <summary>
        /// 敵
        /// </summary>
        ICombatantContainerManager _enemies = null;

        /// <summary>
        /// 選択の種類
        /// </summary>
        TargetSelectionType _selectionType = TargetSelectionType.Single;

        /// <summary>
        /// 説明文UI
        /// </summary>
        DescriptionUI _description = null;

        /// <summary>
        /// 対象インデックス
        /// </summary>
        int _targetIndex = 0;

        /// <summary>
        /// 選択可能である
        /// </summary>
        bool _canSelect = false;

        /// <summary>
        /// ボタンが押されたときのイベント
        /// </summary>
        public UnityAction OnButtonPressed { get; set; } = null;

        /// <summary>
        /// 対象リスト
        /// </summary>
        List<CombatantContainer> _targetables = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="description"></param>
        /// <param name="allies"></param>
        /// <param name="enemies"></param>
        public void SetUp(DescriptionUI description, ICombatantContainerManager allies = null, ICombatantContainerManager enemies = null)
        {
            SetUp();

            _allies = allies;
            _enemies = enemies;
            _description = description;

            _button = _buttons[0];
            
            _button.AddPressedListener(() => OnButtonPressed?.Invoke());

            _button.AddTrriger(EventTriggerType.Move, (e) =>
            {
                if (_canSelect)
                {
                    OnMove(e);
                }
            });
        }

        public override void BeCanceled()
        {
            SetTargetFlagAll(false);

            base.BeCanceled();
        }

        /// <summary>
        /// スキル対象選択の処理
        /// </summary>
        /// <param name="user"></param>
        /// <param name="skillId"></param>
        public void OnSkillTargetSelection(CombatantContainer user, int skillId)
        {
            ActionInfo action = ActionInfo.CreateSkillAction(skillId);
            UpdateTargetSelectionType(user, action.Effect);
            UpdateButtonPressed(user, action);
        }

        /// <summary>
        /// アイテム使用者選択の処理
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemId"></param>
        public void OnItemTargetSelection(CombatantContainer user, int itemId)
        {
            ActionInfo action = ActionInfo.CreateItemAction(itemId);
            UpdateTargetSelectionType(user, action.Effect);
            UpdateButtonPressed(user, action);
        }

        /// <summary>
        /// ボタンが押されたときのイベントを更新する
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        private void UpdateButtonPressed(CombatantContainer user, ActionInfo action)
        {
            OnButtonPressed = () =>
            {
                // 選択解除
                EventSystem.current.SetSelectedGameObject(null);

                // 対象
                CombatantContainer[] targets = _targetables
                .Where(x => x.IsTargeted)
                .ToArray();

                // 対象フラグ解除
                SetTargetFlagAll(false);

                // 行動実行
                user.ActInMenu(action, targets, () => BeCanceled());
            };
        }

        /// <summary>
        /// 対象選択タイプを更新する
        /// </summary>
        /// <param name="user">使用者</param>
        /// <param name="effect">効果データ</param>
        private void UpdateTargetSelectionType(CombatantContainer user, EffectData effect)
        {
            _targetables = user.GetTargetables(effect, _allies, _enemies);
            _selectionType = effect.TargetSelection;

            // 対象が複数存在し、単体を選択する行動に限り対象を変更できる
            _canSelect = _selectionType == TargetSelectionType.Single;
            if (_canSelect)
            {
                _targetIndex = 0;
                SetTargetFlag(true);
            }
            else
            {
                SetTargetFlagAll(true);
            }
        }

        /// <summary>
        /// ボタン移動時のイベント
        /// </summary>
        /// <param name="baseEventData"></param>
        private void OnMove(BaseEventData baseEventData)
        {
            SetTargetFlag(false);

            AxisEventData axisEventData = baseEventData as AxisEventData;
            switch (axisEventData.moveDir)
            {
                case MoveDirection.Left:
                case MoveDirection.Up:
                    ChangeTargetIndex(_targetables.Count - 1);
                    break;

                case MoveDirection.Right:
                case MoveDirection.Down:
                    ChangeTargetIndex(1);
                    break;

                default:
                    break;
            }

            SetTargetFlag(true);
        }

        /// <summary>
        /// 対象フラグを設定する
        /// </summary>
        /// <param name="value"></param>
        private void SetTargetFlag(bool value)
        {
            _targetables[_targetIndex].IsTargeted = value;

            if (value)
            {
                _description.SetText($"【対象選択】 {_targetables[_targetIndex].Combatant.Data.Name}");
            }
        }

        /// <summary>
        /// 対象にできるコンテナ全てに対して対象フラグを設定する
        /// </summary>
        /// <param name="value"></param>
        private void SetTargetFlagAll(bool value)
        {
            foreach (var targetable in _targetables)
            {
                targetable.IsTargeted = value;
            }
        }

        /// <summary>
        /// 対称インデックスを変更する
        /// </summary>
        /// <param name="value"></param>
        private void ChangeTargetIndex(int value)
        {
            _targetIndex = (_targetIndex + value) % _targetables.Count;
        }
    }
}
