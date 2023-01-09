using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;

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
        /// 行動者の表示位置オフセット
        /// </summary>
        [SerializeField]
        float performerPositionOffset = 0.25f;

        /// <summary>
        /// 行動者がスライドする距離
        /// </summary>
        [SerializeField]
        float performerSlideDistance = 25.0f;

        /// <summary>
        /// 行動者がスライドする時間
        /// </summary>
        [SerializeField]
        float performerSlideDuration = 0.2f;

        [SerializeField]
        float perfomerFadeDuration = 0.2f;

        /// <summary>
        /// 行動者のアニメーションシーケンス
        /// </summary>
        Sequence actorSpriteSequence = null;

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

            ReleaseButtons.SetUp();
            ReleaseButtons.Hide();

            for (int i = 0; i < containers.Members.Length; i++)
            {
                containers.Members[i].OnDamageEvent += uiArray[i].OnDamage;
            }
        }

        /// <summary>
        /// コマンド選択開始時の処理
        /// </summary>
        /// <param name="combatant">戦闘の管理者</param>
        public void OnPlayerControlledCombatantCommandSelection(BattleManager battle)
        {
            // スプライトを表示する
            Sprite sprite = battle.Actor.Combatant.Data.Sprite;
            DisplayActorSprite(sprite);
        }

        /// <summary>
        /// 行動者のスプライトを表示する
        /// </summary>
        /// <param name="sprite">スプライト</param>
        private void DisplayActorSprite(Sprite sprite)
        {
            // Imageにスプライトを設定する
            image.sprite = sprite;
            image.SetNativeSize();

            // Imageの位置を調整する
            Vector3 newPosition = image.rectTransform.anchoredPosition;
            newPosition.x = performerSlideDistance;
            newPosition.y = -image.sprite.rect.height * performerPositionOffset;
            image.rectTransform.anchoredPosition = newPosition;

            // Imageを有効化する
            image.enabled = true;

            // Imageを透明にする
            image.ChangeAlpha(0.0f);

            // シーケンス開始
            actorSpriteSequence = DOTween.Sequence()
                .Join(image.DOFade(1.0f, perfomerFadeDuration))
                .Join(image.rectTransform.DOAnchorPosX(0.0f, performerSlideDuration))
                .SetLink(image.gameObject);
        }

        /// <summary>
        /// コマンド選択終了時の処理
        /// </summary>
        public void OnCommandSelectionExit()
        {
            // シーケンス終了
            if (actorSpriteSequence.IsActive())
            {
                actorSpriteSequence.Kill();
            }

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
