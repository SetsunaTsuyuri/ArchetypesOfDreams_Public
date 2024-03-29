﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動順表示UI
    /// </summary>
    public class OrderOfActionsElement : GameUI
    {
        /// <summary>
        /// 顔グラフィック
        /// </summary>
        [SerializeField]
        Image face = null;

        /// <summary>
        /// 顔グラフィックの枠
        /// </summary>
        [SerializeField]
        Image frame = null;

        /// <summary>
        /// 待機時間の表示テキスト
        /// </summary>
        TextMeshProUGUI _waitTime = null;

        /// <summary>
        /// 表示対象の戦闘者コンテナ
        /// </summary>
        CombatantContainer target = null;

        protected override void Awake()
        {
            base.Awake();

            _waitTime = GetComponentInChildren<TextMeshProUGUI>(true);

            DeactivateAndHide();
        }

        /// <summary>
        /// 表示対象の戦闘者コンテナ
        /// </summary>
        public CombatantContainer Target
        {
            get => target;
            set
            {
                target = value;
                OnTargetSet();
            }
        }

        /// <summary>
        /// 対象が設定されたときの処理
        /// </summary>
        private void OnTargetSet()
        {
            if (Target && Target.ContainsFightable)
            {
                UpdateDisplay();
                ActivateAndShow();
            }
            else
            {
                DeactivateAndHide();
            }
        }

        /// <summary>
        /// 表示を更新する
        /// </summary>
        private void UpdateDisplay()
        {
            // 顔スプライト
            face.sprite = Target.Combatant.GetFaceSpriteOrSprite();

            // 枠の色
            Color frameColor = Target switch
            {
                AllyContainer _ => GameSettings.VisualEffects.AllyFrameColor,
                EnemyContainer _ => GameSettings.VisualEffects.EnemyFrameColor,
                _ => Color.white

            };
            frame.color = frameColor;

            // 待機時間
            if (_waitTime)
            {
                _waitTime.text = Target.Combatant.WaitTime.ToString();
            }
        }
    }
}
