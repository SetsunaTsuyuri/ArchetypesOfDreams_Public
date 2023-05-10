using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 権利表記UI
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class CreditsUI : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// プレイヤーインプット
        /// </summary>
        PlayerInput _playerInput = null;

        /// <summary>
        /// スクロールテキスト
        /// </summary>
        ScrollText _scrollText = null;

        protected override void Awake()
        {
            base.Awake();

            _playerInput = GetComponent<PlayerInput>();
            _scrollText = GetComponentInChildren<ScrollText>();

            InputAction move = _playerInput.currentActionMap["Move"];
            move.performed += StartScroll;
            move.canceled += StopScroll;
        }

        public override void SetUp()
        {
            base.SetUp();

            _buttons[0].AddPressedListener(BeCanceled);

            SetEnabled(false);
        }

        public override void BeSelected()
        {
            _playerInput.enabled = true;

            base.BeSelected();
        }

        public override void BeDeselected()
        {
            _playerInput.enabled = false;

            base.BeDeselected();
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
                _playerInput.enabled = false;
            }
        }

        /// <summary>
        /// スクロールを開始する
        /// </summary>
        /// <param name="context"></param>
        private void StartScroll(InputAction.CallbackContext context)
        {
            _scrollText.ScrollDirection = context.ReadValue<Vector2>().y;
        }

        /// <summary>
        /// スクロールを停止する
        /// </summary>
        private void StopScroll(InputAction.CallbackContext _)
        {
            _scrollText.ScrollDirection = 0.0f;
        }
    }
}
