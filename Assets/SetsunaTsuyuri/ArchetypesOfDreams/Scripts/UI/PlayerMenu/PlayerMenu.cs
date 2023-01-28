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
        AllyContainersManager _allies = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="description">説明文UI</param>
        /// <param name="skillMenu">スキルメニュー</param>
        /// <param name="itemMenu">アイテムメニュー</param>
        /// <param name="allies">味方</param>
        public void SetUp(DescriptionUI description, SkillMenu skillMenu, ItemMenu itemMenu, AllyContainersManager allies)
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

            // 編成
            _formation.AddTrriger(EventTriggerType.Select, _ => DescriptionUI.SetText("パーティ編成を行います(準備中)"));
            _formation.IsSeald = true;

            // 帰還
            if (SceneManager.GetActiveScene().buildIndex != (int)SceneType.MyRoom)
            {
                _return.SetUp(DescriptionUI);
            }
            else
            {
                _return.Hide();
            }

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
            _skill.IsSeald = !_allies.GetNonEmptyContainers()
                .Any(x => x.HasAnySkill());

            // アイテム
            _item.IsSeald = !ItemUtility.HasAnyItem();

            UpdateButtonNavigations();
        }
    }
}
