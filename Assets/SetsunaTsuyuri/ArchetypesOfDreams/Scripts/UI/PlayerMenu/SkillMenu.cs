using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// スキルメニュー
    /// </summary>
    public class SkillMenu : ActionMenu<SkillButton>
    {
        /// <summary>
        /// 使用者配列
        /// </summary>
        CombatantContainer[] _users = null;
        
        /// <summary>
        /// 使用者インデックス
        /// </summary>
        int _userIndex = 0;
        
        /// <summary>
        /// 使用者を変更できる
        /// </summary>
        bool _canChangeUser = false;

        /// <summary>
        /// 味方UI
        /// </summary>
        AlliesUI _alliesUI = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="description"></param>
        /// <param name="targetSelection"></param>
        /// <param name="alliesUI"></param>
        public void SetUp(DescriptionUI description, TargetSelectionUI targetSelection, AlliesUI alliesUI)
        {
            SetUp(description, targetSelection);

            _alliesUI = alliesUI;

            foreach (var button in _buttons)
            {
                button.AddTrriger(EventTriggerType.Move, e =>
                {
                    if (_canChangeUser)
                    {
                        MoveDirection direction = button.GetMoveDirection(e);
                        OnUserChange(direction);
                    }
                });
            }
        }

        public override void BeSelected()
        {
            // 非戦闘中は使用者を変更できる
            if (!Battle.IsRunning)
            {
                _users = User.Allies.GetNonEmptyContainers() 
                    .Where(x => x.HasAnySkill())
                    .ToArray();

                _userIndex = System.Array.IndexOf(_users, User);
                _canChangeUser = _users.Length > 1;

                if (User.Combatant.Sprite != _alliesUI.ActorSprite)
                {
                    _alliesUI.DisplayActorSprite(User);
                }
            }

            base.BeSelected();
        }

        public override void BeCanceled()
        {
            if (!Battle.IsRunning)
            {
                _alliesUI.HideActorSprite();
            }

            base.BeCanceled();
        }

        protected override void OnActionTargetSelection(TargetSelectionUI targetSelection, int id)
        {
            targetSelection.OnSkillTargetSelection(User, id);
        }

        protected override (int, bool)[] GetIdAndCanBeUsed()
        {
            var ids = User.Combatant.GetAcquisitionSkillIds();
            
            (int, bool)[] result = ids
                .Select(x => (x, User.CanUseSkill(x)))
                .ToArray();
                
            return result;
        }

        /// <summary>
        /// 使用者変更時の処理
        /// </summary>
        /// <param name="direction"></param>
        private void OnUserChange(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Left:
                    ChangeUser(-1);
                    break;

                case MoveDirection.Right:
                    ChangeUser(1);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 使用者を変更する
        /// </summary>
        /// <param name="value"></param>
        private void ChangeUser(int value)
        {
            AudioManager.PlaySE(SEId.FocusMove);

            _userIndex += value;
            if (_userIndex < 0)
            {
                _userIndex = _users.Length - 1;
            }
            else if (_userIndex >= _users.Length)
            {
                _userIndex = 0;
            }

            User = _users[_userIndex];
            _alliesUI.DisplayActorSprite(User);
            UpdateButtons();
            SelectAnyButton();
        }
    }
}
