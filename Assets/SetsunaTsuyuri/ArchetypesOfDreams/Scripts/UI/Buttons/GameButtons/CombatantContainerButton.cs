using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者コンテナボタン
    /// </summary>
    public class CombatantContainerButton : GameButton
    {
        /// <summary>
        /// 名前の表示テキスト
        /// </summary>
        [field: SerializeField]
        public TextMeshProUGUI Name { get; set; } = null;

        /// <summary>
        /// レベルの表示テキスト
        /// </summary>
        [field: SerializeField]
        public TextMeshProUGUI Level { get; set; } = null;

        /// <summary>
        /// ボタンが押されたときに実行する関数
        /// </summary>
        UnityAction _onPressed = () => { };

        protected override void Awake()
        {
            base.Awake();

            AddPressedListener(_onPressed);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="allies">味方の管理者</param>
        /// <param name="container">戦闘者コンテナ</param>
        public void SetUp(AlliesParty allies, CombatantContainer container)
        {
            if (container.ContainsCombatant)
            {
                gameObject.SetActive(true);
                Name.text = container.Combatant?.Data.Name;
                Level.text = GameSettings.Terms.Level + container.Combatant?.Level.ToString();

                if (container.ContainsReleasable)
                {
                    RemovePressedListener(_onPressed);
                    _onPressed = () => allies.ToBeReleased = container;
                    AddPressedListener(_onPressed);

                    SetInteractable(true);
                }
                else
                {
                    SetInteractable(false);
                }
            }
            else
            {
                SetInteractable(false);
                gameObject.SetActive(false);
            }
        }
    }
}
