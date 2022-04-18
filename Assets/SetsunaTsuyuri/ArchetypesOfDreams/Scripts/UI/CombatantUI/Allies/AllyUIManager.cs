using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 味方UIの管理者
    /// </summary>
    public class AllyUIManager : CombatantContainerUIManager<AllyUI, AllyContainersManager, AllyContainer>
    {
        /// <summary>
        /// スプライトを表示するImage
        /// </summary>
        [SerializeField]
        Image image = null;

        /// <summary>
        /// 行動者のスプライトの表示オフセット
        /// </summary>
        [SerializeField]
        float performerSpriteOffset = 0.25f;

        /// <summary>
        /// 解放ボタンの管理者
        /// </summary>
        public ReleaseButtonsManager ReleaseButtons { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            ReleaseButtons = GetComponentInChildren<ReleaseButtonsManager>(true);
        }

        protected override void Start()
        {
            base.Start();

            ReleaseButtons.SetUpButtons();
            ReleaseButtons.Hide();
        }

        /// <summary>
        /// コマンド選択開始時の処理
        /// </summary>
        /// <param name="combatant">戦闘の管理者</param>
        public void OnPlayerControlledCombatantCommandSelection(BattleManager battle)
        {
            // スプライトを表示する
            DisplayPerformerSprite(battle.Performer.Combatant.GetData().Sprite);
        }

        /// <summary>
        /// 行動者のスプライトを表示する
        /// </summary>
        /// <param name="sprite">スプライト</param>
        private void DisplayPerformerSprite(Sprite sprite)
        {
            // Imageにスプライトを設定する
            image.sprite = sprite;
            image.SetNativeSize();

            // Imageの座標を調整する
            Vector3 newPosition = image.rectTransform.anchoredPosition;
            newPosition.y = -image.sprite.rect.height * performerSpriteOffset;
            image.rectTransform.anchoredPosition = newPosition;

            // Imageを有効化する
            image.enabled = true;
        }

        /// <summary>
        /// コマンド選択終了時の処理
        /// </summary>
        public void OnCommandSelectionExit()
        {
            // Imageを無効化する
            image.enabled = false;
        }

        /// <summary>
        /// 対象フラグが設定されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnTargetFlagSet(CombatantContainer container)
        {
            AllyUI ui = uiArray[container.Id];
            ui.ColorChanger.OnTargetFlagSet(container.IsTargeted);
        }
    }
}
