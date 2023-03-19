using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 味方コンテナUIの管理UI
    /// </summary>
    public class AlliesUI : CombatantsUI<AllyUI, AlliesParty, AllyContainer>
    {
        /// <summary>
        /// スプライトを表示するImage
        /// </summary>
        [SerializeField]
        Image image = null;

        /// <summary>
        /// 行動者のスプライト
        /// </summary>
        public Sprite ActorSprite => image.sprite;

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

            for (int i = 0; i < _party.Members.Length; i++)
            {
                _party.Members[i].Damaged += _uiArray[i].OnDamage;
            }
        }

        /// <summary>
        /// コマンド選択開始時の処理
        /// </summary>
        /// <param name="combatant">戦闘の管理者</param>
        public void OnPlayerControlledCombatantCommandSelection(Battle battle)
        {
            // スプライトを表示する
            DisplayActorSprite(battle.Actor);
        }

        /// <summary>
        /// 行動者のスプライトを表示する
        /// </summary>
        /// <param name="container"></param>
        public void DisplayActorSprite(CombatantContainer container)
        {
            Sprite sprite = container.Combatant.Sprite;
            if (!sprite)
            {
                return;
            }

            // Imageにスプライトを設定する
            image.sprite = sprite;
            image.SetNativeSize();
            image.rectTransform.sizeDelta *= container.Combatant.Data.SpriteScale;

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
            HideActorSprite();
        }

        /// <summary>
        /// 行動者のスプライトを非表示にする
        /// </summary>
        public void HideActorSprite()
        {
            // シーケンス終了
            if (actorSpriteSequence.IsActive())
            {
                actorSpriteSequence.Kill();
            }

            image.sprite = null;
            image.enabled = false;
        }

        /// <summary>
        /// 対象フラグが設定されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnTargetFlagSet(CombatantContainer container)
        {
            AllyUI ui = _uiArray[container.Id];
            ui.ColorChanger.OnTargetFlagSet(container.IsTargeted);
        }
    }
}
