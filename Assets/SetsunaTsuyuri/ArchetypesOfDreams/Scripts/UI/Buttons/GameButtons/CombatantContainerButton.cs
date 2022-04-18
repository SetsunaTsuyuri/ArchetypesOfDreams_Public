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
        UnityAction onClick = () => { };

        protected override void Awake()
        {
            base.Awake();

            Button.onClick.AddListener(onClick);
        }

        /// <summary>
        /// 使えるようにする
        /// </summary>
        /// <param name="allies">味方の管理者</param>
        /// <param name="container">戦闘者コンテナ</param>
        public void SetUp(AllyContainersManager allies, CombatantContainer container)
        {
            if (container.ContainsCombatant())
            {
                gameObject.SetActive(true);
                Name.text = container.Combatant?.GetData().Name;
                Level.text = GameSettings.Terms.Level + container.Combatant?.Level.ToString();

                if (container.ContainsReleasable())
                {
                    Button.onClick.RemoveListener(onClick);
                    onClick = () => allies.ToBeReleased = container;
                    Button.onClick.AddListener(onClick);

                    Button.interactable = true;
                }
                else
                {
                    Button.interactable = false;
                }
            }
            else
            {
                Button.interactable = false;
                gameObject.SetActive(false);
            }
        }
    }
}
