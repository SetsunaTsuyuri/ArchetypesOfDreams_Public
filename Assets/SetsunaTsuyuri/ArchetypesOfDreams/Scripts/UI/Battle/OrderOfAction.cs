using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 行動順表示UI
    /// </summary>
    public class OrderOfAction : GameUI
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
        /// コンテナのID表示
        /// </summary>
        TextMeshProUGUI id = null;

        /// <summary>
        /// 表示対象の戦闘者コンテナ
        /// </summary>
        CombatantContainer target = null;

        protected override void Awake()
        {
            base.Awake();

            id = GetComponentInChildren<TextMeshProUGUI>(true);

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
            if (Target && Target.ContainsActionable())
            {
                UpdateImages();
                ActivateAndShow();
            }
            else
            {
                DeactivateAndHide();
            }
        }

        /// <summary>
        /// イメージを更新する
        /// </summary>
        private void UpdateImages()
        {
            // 戦闘者データ
            CombatantData data = Target.Combatant.GetData();

            // 顔グラフィック
            face.sprite = data.GetFaceSpriteOrSprite();

            // 枠の色
            Color frameColor = Target switch
            {
                AllyContainer _ => GameSettings.VisualEffects.AllyFrameColor,
                EnemyContainer _ => GameSettings.VisualEffects.EnemyFrameColor,
                _ => Color.white

            };
            frame.color = frameColor;
        }

        ///// <summary>
        ///// IDをアルファベットに変換する
        ///// </summary>
        ///// <param name="id">コンテナのID</param>
        ///// <returns></returns>
        //private string ToAlphabet(int id)
        //{
        //    // IDを26で割った余り
        //    int remainder = id % 26;

        //    string result = ('A' + remainder).ToString();

        //    return result;
        //}
    }
}
