using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 自室メニューの管理者
    /// </summary>
    public class MyRoomMenu : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// 説明文のUI
        /// </summary>
        [SerializeField]
        DescriptionUI _descriptionUI = null;

        /// <summary>
        /// 冒険ボタン
        /// </summary>
        [SerializeField]
        GameButton _adventureButton = null;

        /// <summary>
        /// 冒険ボタンの説明文
        /// </summary>
        [SerializeField]
        string _adventureDescription = string.Empty;

        /// <summary>
        /// セーブボタン
        /// </summary>
        [SerializeField]
        GameButton _saveButton = null;

        /// <summary>
        /// セーブボタンの説明文
        /// </summary>
        [SerializeField]
        string _saveDescription = string.Empty;

        /// <summary>
        /// プレイヤーインプット
        /// </summary>
        PlayerInput _playerInput = null;

        protected override void Awake()
        {
            base.Awake();

            _playerInput = GetComponent<PlayerInput>();
        }

        public override void BeSelected()
        {
            base.BeSelected();

            _descriptionUI.Show();
            _playerInput.enabled = true;
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();

            // 冒険ボタン
            _adventureButton.AddPressedListener(() =>
            {
                _playerInput.enabled = false;
                SceneChangeManager.StartChange(SceneType.DungeonSelection);
                _adventureButton.Lock();
            });
            _adventureButton.AddTrriger(EventTriggerType.Select, (_) => _descriptionUI.SetText(_adventureDescription));

            // セーブボタン
            _saveButton.AddPressedListener(() =>
            {
                _playerInput.enabled = false;
                Stack(typeof(SaveDataMenu));
            });
            _saveButton.AddTrriger(EventTriggerType.Select, (_) => _descriptionUI.SetText(_saveDescription));
        }

        /// <summary>
        /// メニュー開く入力時の処理
        /// </summary>
        /// <param name="context"></param>
        public void OnMenuOpen(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            AudioManager.PlaySE(SEType.Select);
            _playerInput.enabled = false;
            Stack(typeof(PlayerMenu));
        }
    }
}
