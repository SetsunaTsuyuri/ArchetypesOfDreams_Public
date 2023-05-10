using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ポップアップテキスト
    /// </summary>
    public class PopUpText : GameUI
    {
        /// <summary>
        /// レクトトランスフォーム
        /// </summary>
        public RectTransform RectTransform { get; private set; } = null;

        /// <summary>
        /// TMPテキスト
        /// </summary>
        public TextMeshProUGUI Text { get; private set; } = null;

        /// <summary>
        /// 初期のアルファ値
        /// </summary>
        float _initialAlpha = 0.0f;

        /// <summary>
        /// ループするTweener
        /// </summary>
        Tweener _loopTweener = null;

        public bool IsLoop => _loopTweener.IsActive();

        protected override void Awake()
        {
            base.Awake();

            _initialAlpha = _canvasGroup.alpha;
            Text = GetComponentInChildren<TextMeshProUGUI>();
            RectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// 表示する
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="text">文字列</param>
        /// <param name="color">色</param>
        public void Show(Vector3 position, string text, Color color)
        {
            // 位置変更
            transform.position = position;

            // テキスト変更
            Text.text = text;

            // 色変更
            Text.color = color;

            // アルファ値初期化
            _canvasGroup.alpha = _initialAlpha;

            enabled = true;
            SetEnabled(true);
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public override void SetEnabled(bool enabled)
        {
            base.SetEnabled(enabled);

            if (!enabled)
            {
                _loopTweener.Kill();
                this.enabled = false;
            }
        }

        /// <summary>
        /// 繰り返し点滅する
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="text">文字列</param>
        /// <param name="color">色</param>
        public void BlinkRepeatedly(Vector3 position, string text, Color color)
        {
            // 表示する
            Show(position, text, color);

            // 繰り返し点滅する
            _loopTweener = DOVirtual.Float(
                0.0f,
                1.0f,
                GameSettings.PopUpTexts.BlinkingDutation,
                value => _canvasGroup.alpha = GameSettings.PopUpTexts.BlinkingCurve.Evaluate(value))
                .SetLoops(-1)
                .SetLink(gameObject);
        }

        /// <summary>
        /// ポップアップする
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="text">文字列</param>
        /// <param name="color">色</param>
        public void PopUp(Vector3 position, string text, Color color)
        {
            // 表示する
            Show(position, text, color);

            // 移動
            var move = RectTransform
                .DOLocalMove(GameSettings.PopUpTexts.MoveVector, GameSettings.PopUpTexts.MoveDuration)
                .SetRelative();

            // フェードアウト
            var fadeout = _canvasGroup.DOFade(0.0f, GameSettings.PopUpTexts.FadeOutDuration);

            // シーケンス
            Sequence sequence = DOTween.Sequence()
                .Append(move)
                .Append(fadeout)
                .SetLink(gameObject)
                .OnComplete(() => SetEnabled(false));
        }
    }
}
