using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using SetsunaTsuyuri.Scenario;

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
        Image _activeCombatantImage = null;

        /// <summary>
        /// 行動者がスライドする距離
        /// </summary>
        [SerializeField]
        float _activeCombatantSlideDistance = 25.0f;

        /// <summary>
        /// 行動者がスライドする時間
        /// </summary>
        [SerializeField]
        float _activeCombatantSlideDuration = 0.2f;

        /// <summary>
        /// 行動者のフェードインする時間
        /// </summary>
        [SerializeField]
        float _activeCombatantFadeInDuration = 0.2f;

        /// <summary>
        /// 行動者の初期X座標
        /// </summary>
        Vector2 _activeCombatantInitialAnchoredPosition = Vector2.zero;

        /// <summary>
        /// 行動者のアニメーションシーケンス
        /// </summary>
        Sequence _activeCombatantSpriteSequence = null;

        /// <summary>
        /// 解放ボタンの管理者
        /// </summary>
        public ReleaseButtonsManager ReleaseButtons { get; private set; } = null;
        

        protected override void Awake()
        {
            base.Awake();

            ReleaseButtons = GetComponentInChildren<ReleaseButtonsManager>(true);
        }

        public override void SetUp()
        {
            base.SetUp();

            _activeCombatantInitialAnchoredPosition = _activeCombatantImage.rectTransform.anchoredPosition;

            ReleaseButtons.SetUp();
            ReleaseButtons.SetEnabled(false);

            for (int i = 0; i < _party.Members.Length; i++)
            {
                _party.Members[i].Damaged += _uiArray[i].OnDamage;
            }

            // シナリオ開始
            MessageBrokersManager.ScenarioStart
                .Receive<ScenarioManager>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => SaveEnabledAndHide());

            // シナリオ終了
            MessageBrokersManager.ScenarioEnd
                .Receive<ScenarioManager>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => LoadEnabled());

            // ステータスメニュー
            MessageBrokersManager.StatusMenuUserSet
                .Receive<CombatantContainer>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(DisplayActiveCombatantSprite);

            // ステータスメニュー終了
            MessageBrokersManager.StatusMenuClosed
                .Receive<StatusMenu>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => HideActiveCombatantSprite());

            // 戦闘コマンド選択
            MessageBrokersManager.BattleCommandsSelected
                .Receive<CombatantContainer>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(DisplayActiveCombatantSprite);

            // スキル選択
            MessageBrokersManager.SkillMenuUserSet
                .Receive<CombatantContainer>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(DisplayActiveCombatantSprite);

            // 戦闘中のアイテム選択
            MessageBrokersManager.ItemMenuSelected
                .Receive<CombatantContainer>()
                .Where(_ => Battle.IsRunning)
                .TakeUntilDestroy(gameObject)
                .Subscribe(DisplayActiveCombatantSprite);

            // 戦闘中の対象選択
            MessageBrokersManager.TargetSelectionStart
                .Receive<CombatantContainer>()
                .Where(_ => Battle.IsRunning)
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => HideActiveCombatantSprite());
        }

        /// <summary>
        /// 行動者のスプライトを表示する
        /// </summary>
        /// <param name="container"></param>
        public void DisplayActiveCombatantSprite(CombatantContainer container)
        {
            if (!container)
            {
                return;
            }

            // スプライトが存在しないか、同じスプライトなら処理しない
            Sprite sprite = container.Combatant.Sprite;
            if (!sprite || _activeCombatantImage.sprite == sprite)
            {
                return;
            }

            // Imageにスプライトを設定する
            _activeCombatantImage.sprite = sprite;
            _activeCombatantImage.SetNativeSize();
            _activeCombatantImage.rectTransform.sizeDelta *= container.Combatant.Data.SpriteScale;

            // Imageの位置を調整する
            Vector3 newPosition = _activeCombatantInitialAnchoredPosition;
            float offsetX = container.Combatant.Data.AllySpriteOffsetX;
            newPosition.x = offsetX + _activeCombatantSlideDistance;
            newPosition.y = container.Combatant.Data.AllySpriteOffsetY;
            //newPosition.y = -_activeCombatantImage.sprite.rect.height * _activeCombatantPositionOffset;

            _activeCombatantImage.rectTransform.anchoredPosition = newPosition;

            // Imageを有効化する
            _activeCombatantImage.enabled = true;

            // Imageを透明にする
            _activeCombatantImage.ChangeAlpha(0.0f);

            // シーケンス終了時のX座標
            float endX = offsetX + _activeCombatantInitialAnchoredPosition.x;

            // シーケンス開始
            KillActiveCombatantSpriteSequence();
            _activeCombatantSpriteSequence = DOTween.Sequence()
                .Join(_activeCombatantImage.DOFade(1.0f, _activeCombatantFadeInDuration))
                .Join(_activeCombatantImage.rectTransform.DOAnchorPosX(endX, _activeCombatantSlideDuration))
                .SetLink(_activeCombatantImage.gameObject);
        }

        /// <summary>
        /// 行動者のスプライトを非表示にする
        /// </summary>
        public void HideActiveCombatantSprite()
        {
            KillActiveCombatantSpriteSequence();
            _activeCombatantImage.sprite = null;
            _activeCombatantImage.enabled = false;
        }

        /// <summary>
        /// 行動者のスプライト表示シーケンスを終了する
        /// </summary>
        private void KillActiveCombatantSpriteSequence()
        {
            if (!_activeCombatantSpriteSequence.IsActive())
            {
                return;
            }

            _activeCombatantSpriteSequence.Kill();
        }
    }
}
