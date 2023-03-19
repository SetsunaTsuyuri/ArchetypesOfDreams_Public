using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// プレイヤーメニュー
    /// </summary>
    public class PlayerMenu : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// スキル
        /// </summary>
        [SerializeField]
        GameButton _skill = null;

        /// <summary>
        /// アイテム
        /// </summary>
        [SerializeField]
        GameButton _item = null;

        /// <summary>
        /// ステータス
        /// </summary>
        [SerializeField]
        GameButton _status = null;

        /// <summary>
        /// 編成
        /// </summary>
        [SerializeField]
        GameButton _formation = null;

        /// <summary>
        /// 帰還
        /// </summary>
        [SerializeField]
        SceneChangeButton _return = null;

        /// <summary>
        /// 設定
        /// </summary>
        [SerializeField]
        GameButton _options = null;

        /// <summary>
        /// 説明文
        /// </summary>
        public DescriptionUI DescriptionUI { get; private set; } = null;

        /// <summary>
        /// メニューマップカメラ
        /// </summary>
        [SerializeField]
        Camera _menuMapCamera = null;

        /// <summary>
        /// メニューマップ
        /// </summary>
        [SerializeField]
        GameUI _menuMap = null;

        /// <summary>
        /// 味方
        /// </summary>
        AlliesParty _allies = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="description">説明文UI</param>
        /// <param name="skillMenu">スキルメニュー</param>
        /// <param name="itemMenu">アイテムメニュー</param>
        /// <param name="targetSelection">対象選択UI</param>
        /// <param name="allies">味方</param>
        public void SetUp(DescriptionUI description, SkillMenu skillMenu, ItemMenu itemMenu, TargetSelectionUI targetSelection,  AlliesParty allies)
        {
            SetUp();

            // 説明文
            DescriptionUI = description;

            // 味方
            _allies = allies;

            // スキル
            _skill.AddTrriger(EventTriggerType.Select, _ => DescriptionUI.SetText("スキルを使用・確認します"));
            _skill.AddPressedListener(() =>
            {
                skillMenu.User = _allies.MenuUser;
                Stack(typeof(SkillMenu));
            });

            // アイテム
            _item.AddTrriger(EventTriggerType.Select, _ => DescriptionUI.SetText("アイテムを使用・確認します"));
            _item.AddPressedListener(() =>
            {
                itemMenu.User = _allies.MenuUser;
                Stack(typeof(ItemMenu));
            });

            // ステータスメニュー
            StatusMenu statusMenu = GetComponentInChildren<StatusMenu>();
            statusMenu.SetUp();

            _status.AddTrriger(EventTriggerType.Select, _ => DescriptionUI.SetText("ステータスを確認します"));
            _status.AddPressedListener(() =>
            {
                statusMenu.UpdateTargetables(_allies);
                Stack(typeof(StatusMenu));
            });

            // 編成
            _formation.AddTrriger(EventTriggerType.Select, _ => DescriptionUI.SetText("パーティ編成を行います"));
            _formation.AddPressedListener(() =>
            {
                targetSelection.OnFormationChangeTargetSelection();
                Stack(typeof(TargetSelectionUI));
            });

            // 帰還
            if (SceneManager.GetActiveScene().buildIndex != (int)SceneId.MyRoom)
            {
                _return.SetUp(DescriptionUI);
            }
            else
            {
                _return.Hide();
            }

            // オプションメニュー
            _options.AddTrriger(EventTriggerType.Select, _ => DescriptionUI.SetText("ゲームの設定を変更します"));
            _options.AddPressedListener(() => Stack(typeof(OptionsMenu)));

            UpdateButtons();
            Hide();
        }

        public override void BeSelected()
        {
            UpdateButtons();

            base.BeSelected();

            if (_menuMapCamera)
            {
                _menuMapCamera.enabled = true;
                _menuMap.Show();
            }
            else
            {
                _menuMap.Hide();
            }

            DescriptionUI.Show();
        }

        public override void BeCanceled()
        {
            DescriptionUI.Hide();
            Hide();

            base.BeCanceled();
        }

        /// <summary>
        /// ボタンを更新する
        /// </summary>
        public void UpdateButtons()
        {
            // スキル
            _skill.IsSeald = !_allies.HasAnySkill;

            // アイテム
            _item.IsSeald = !ItemUtility.HasAnyItem();

            // 編成
            _formation.IsSeald = _allies.CountChangeables() < 2;

            UpdateButtonNavigations();
        }
    }
}
