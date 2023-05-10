using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ステータスメニュー
    /// </summary>
    public class StatusMenu : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// 対象配列
        /// </summary>
        CombatantContainer[] _targetables = null;

        /// <summary>
        /// 対象インデックス
        /// </summary>
        int _targetIndex = 0;

        /// <summary>
        /// 対象の変更が可能である
        /// </summary>
        bool _canChangeTarget = false;

        /// <summary>
        /// 詳細ステータス
        /// </summary>
        DetailedStatusUI _detailedStatus = null;

        /// <summary>
        /// 使用者
        /// </summary>
        public CombatantContainer User => _targetables[_targetIndex];

        public override void SetUp()
        {
            base.SetUp();

            foreach (var button in _buttons)
            {
                button.AddTrriger(EventTriggerType.Move, (e) =>
                {
                    if (!_canChangeTarget)
                    {
                        return;
                    }

                    MoveDirection direction = button.GetMoveDirection(e);
                    OnMove(direction);
                });
            }

            _detailedStatus = GetComponentInChildren<DetailedStatusUI>();
            SetEnabled(false);
        }

        public override void BeSelected()
        {
            _targetIndex = 0;
            UpdateView();

            base.BeSelected();
        }

        public override void BeCanceled()
        {
            SetEnabled(false);

            base.BeCanceled();
        }

        public override void SetEnabled(bool enabled)
        {
            base.SetEnabled(enabled);

            if (!enabled)
            {
                MessageBrokersManager.StatusMenuClosed.Publish(this);
            }
        }

        /// <summary>
        /// 対象配列を更新する
        /// </summary>
        /// <param name="containers"></param>
        public void UpdateTargetables(CombatantsPartyBase containers)
        {
            _targetables = containers.GetNonEmptyContainers()
                .ToArray();

            _canChangeTarget = _targetables.Length > 1;
        }

        /// <summary>
        /// フォーカス移動処理
        /// </summary>
        /// <param name="direction"></param>
        private void OnMove(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Left:
                case MoveDirection.Up:
                    ChangeTarget(-1);
                    break;

                case MoveDirection.Right:
                case MoveDirection.Down:
                    ChangeTarget(1);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 対象を変更する
        /// </summary>
        /// <param name="value"></param>
        private void ChangeTarget(int value)
        {
            AudioManager.PlaySE(SEId.FocusMove);

            _targetIndex += value;
            if (_targetIndex < 0)
            {
                _targetIndex = _targetables.Length - 1;
            }
            else if(_targetIndex >= _targetables.Length)
            {
                _targetIndex = 0;
            }

            UpdateView();
        }

        /// <summary>
        /// 表示を更新する
        /// </summary>
        private void UpdateView()
        {
            CombatantContainer target = _targetables[_targetIndex];
            _detailedStatus.UpdateView(target.Combatant);
            MessageBrokersManager.StatusMenuUserSet.Publish(target);
        }
    }
}
