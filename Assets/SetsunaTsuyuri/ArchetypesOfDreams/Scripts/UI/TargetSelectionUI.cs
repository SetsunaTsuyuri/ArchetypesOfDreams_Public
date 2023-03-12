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
        /// 選択の種類
        /// </summary>
        TargetSelectionType _selectionType = TargetSelectionType.Single;

        /// <summary>
        /// 説明文UI
        /// </summary>
        DescriptionUI _description = null;

        /// <summary>
        /// 味方
        /// </summary>
        AlliesParty _allies = null;

        /// <summary>
        /// 戦闘
        /// </summary>
        Battle _battle = null;

        /// <summary>
        /// 対象インデックス
        /// </summary>
        int _targetIndex = 0;

        /// <summary>
        /// 対象の変更が可能である
        /// </summary>
        bool _canChangeTargetIndex = false;

        /// <summary>
        /// ボタンが押されたときのイベント
        /// </summary>
        UnityAction _buttonPressed = null;

        /// <summary>
        /// スキル・アイテムの使用者
        /// </summary>
        CombatantContainer _user = null;

        /// <summary>
        /// 対象リスト
        /// </summary>
        List<CombatantContainer> _targetables = null;

        /// <summary>
        /// 交代の対象A
        /// </summary>
        CombatantContainer _positionSwappingTargetA = null;

        /// <summary>
        /// 交代の対象B
        /// </summary>
        CombatantContainer _postionSwappingTargetB = null;

        /// <summary>
        /// 味方の対象選択開始時のイベント
        /// </summary>
        public event UnityAction<EffectData, List<CombatantContainer>> AllyTargetSelectionStart = null;

        /// <summary>
        /// 味方の対象選択終了時のイベント
        /// </summary>
        public event UnityAction AllyTargetSelectionEnd = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="description"></param>
        /// <param name="allies"></param>
        /// <param name="battle"></param>
        public void SetUp(DescriptionUI description, AlliesParty allies, Battle battle)
        {
            SetUp();

            _description = description;
            _allies = allies;
            _battle = battle;

            _button = _buttons[0];
            _button.AddPressedListener(() => _buttonPressed?.Invoke());
            _button.AddTrriger(EventTriggerType.Move, (e) =>
            {
                if (_canChangeTargetIndex)
                {
                    MoveDirection direction = _button.GetMoveDirection(e);
                    ChangeTargetIndex(direction);
                }
            });
        }

        public override void BeDeselected()
        {
            base.BeDeselected();

            SetTargetFlagAll(false);
            _positionSwappingTargetA = null;
            _postionSwappingTargetB = null;

            AllyTargetSelectionEnd?.Invoke();
        }

        /// <summary>
        /// スキル対象選択の処理
        /// </summary>
        /// <param name="user"></param>
        /// <param name="skillId"></param>
        public void OnSkillTargetSelection(CombatantContainer user, int skillId)
        {
            ActionInfo action = ActionInfo.CreateSkillAction(skillId);
            OnActionTargetSelection(user, action);
        }

        /// <summary>
        /// アイテム対象選択の処理
        /// </summary>
        /// <param name="user"></param>
        /// <param name="itemId"></param>
        public void OnItemTargetSelection(CombatantContainer user, int itemId)
        {
            ActionInfo action = ActionInfo.CreateItemAction(itemId);
            OnActionTargetSelection(user, action);
        }

        /// <summary>
        /// 行動対象選択の処理
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        private void OnActionTargetSelection(CombatantContainer user, ActionInfo action)
        {
            _user = user;
            UpdateTargetSelectionType(user, action.Effect);
            UpdateButtonPressedOnActionTargetSelection(user, action);

            _user.Combatant.CreateActionResultsOnTargetSelection(action, _targetables);
            AllyTargetSelectionStart?.Invoke(action.Effect, _targetables);
        }

        /// <summary>
        /// ボタンが押されたときのイベントを更新する
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        private void UpdateButtonPressedOnActionTargetSelection(CombatantContainer user, ActionInfo action)
        {
            _buttonPressed = () =>
            {
                // 選択解除
                EventSystem.current.SetSelectedGameObject(null);

                // 対象
                CombatantContainer[] targets = GetTargers();

                // 対象フラグ解除
                SetTargetFlagAll(false);

                // 行動実行
                if (Battle.IsRunning)
                {
                    BeDeselectedAndClearHistory();
                    user.Act(action, targets, () => _battle.OnActionEnd(action));
                }
                else
                {
                    user.Act(action, targets, BeCanceled);
                }
            };
        }

        /// <summary>
        /// 編成変更対象選択の処理
        /// </summary>
        public void OnFormationChangeTargetSelection()
        {
            //主人公以外は全て選択できる
            _targetables = _allies.GetChangeables()
                .ToList();

            UpdateButtonPressedOnFormationChange();

            _canChangeTargetIndex = true;
            _targetIndex = 0;
            SetTargetFlag(true);
        }

        /// <summary>
        /// ボタンが押されたときのイベントを更新する
        /// </summary>
        private void UpdateButtonPressedOnFormationChange()
        {
            _buttonPressed = () =>
            {
                CombatantContainer target = _targetables.FirstOrDefault(x => x.IsTargeted);
                if (_positionSwappingTargetA == null)
                {
                    _positionSwappingTargetA = target;
                }
                else if (_positionSwappingTargetA == target)
                {
                    return;
                }
                else if (_postionSwappingTargetB == null)
                {
                    _postionSwappingTargetB = target;

                    _positionSwappingTargetA.Swap(_postionSwappingTargetB);

                    _positionSwappingTargetA = null;
                    _postionSwappingTargetB = null;
                }
            };
        }

        /// <summary>
        /// 対象配列を取得する
        /// </summary>
        /// <returns></returns>
        private CombatantContainer[] GetTargers()
        {
            var targets = _targetables
                .Where(x => x.IsTargeted)
                .ToArray();

            return targets;
        }

        /// <summary>
        /// 対象選択タイプを更新する
        /// </summary>
        /// <param name="user">使用者</param>
        /// <param name="effect">効果データ</param>
        private void UpdateTargetSelectionType(CombatantContainer user, EffectData effect)
        {
            _canChangeTargetIndex = false;
            _targetables = user.GetTargetables(effect);
            _selectionType = effect.TargetSelection;

            // 対象が複数存在し、単体を選択する行動に限り対象を変更できる
            if (_selectionType == TargetSelectionType.Single)
            {
                _targetIndex = 0;
                SetTargetFlag(true);

                if (_targetables.Count > 1)
                {
                    _canChangeTargetIndex = true;
                }
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
        private void ChangeTargetIndex(MoveDirection direction)
        {
            AudioManager.PlaySE(SEId.FocusMove);

            SetTargetFlag(false);

            switch (direction)
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

            // TODO: イベントにする
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
        /// 対象インデックスを変更する
        /// </summary>
        /// <param name="value"></param>
        private void ChangeTargetIndex(int value)
        {
            _targetIndex = (_targetIndex + value) % _targetables.Count;
        }
    }
}
