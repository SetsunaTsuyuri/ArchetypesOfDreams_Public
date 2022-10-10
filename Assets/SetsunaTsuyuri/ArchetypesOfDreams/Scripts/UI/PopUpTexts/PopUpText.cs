using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ポップアップするテキスト
    /// </summary>
    public class PopUpText : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; } = null;
        Canvas canvas = null;
        CanvasGroup canvasGroup = null;

        /// <summary>
        /// TMPテキスト
        /// </summary>
        public TextMeshProUGUI Text { get; private set; } = null;

        /// <summary>
        /// 初期のアルファ値
        /// </summary>
        float initialAlpha = 0.0f;

        /// <summary>
        /// ループシーケンス
        /// </summary>
        public Sequence LoopSequence { get; private set; } = null;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            initialAlpha = canvasGroup.alpha;

            Text = GetComponentInChildren<TextMeshProUGUI>();
            RectTransform = GetComponent<RectTransform>();

        }

        /// <summary>
        /// 表示する
        /// </summary>
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
            canvasGroup.alpha = initialAlpha;

            // キャンバス有効化
            canvas.enabled = true;

            // 有効化
            enabled = true;
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public void Hide()
        {
            // シーケンスを止める
            if (LoopSequence != null)
            {
                LoopSequence.Kill();
            }

            // キャンバス無効化
            canvas.enabled = false;

            // 無効化
            enabled = false;
        }

        /// <summary>
        /// 点滅表示シーケンスを実行する
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="text">文字列</param>
        /// <param name="color">色</param>
        public void DoFlashingSequence(Vector3 position, string text, Color color)
        {
            // 表示する
            Show(position, text, color);

            LoopSequence = DOTween.Sequence();

            LoopSequence
                .AppendInterval(GameSettings.PopUpTexts.BlinkingInterval)
                .Append(
                    DOTween.To(
                        () => canvasGroup.alpha,
                        x => canvasGroup.alpha = x,
                        0.0f,
                        GameSettings.PopUpTexts.BlinkingFadeDutation
                    )
                )
                .Append(
                    DOTween.To(
                        () => canvasGroup.alpha,
                        x => canvasGroup.alpha = x,
                        1.0f,
                        GameSettings.PopUpTexts.BlinkingFadeDutation
                    )
                )
                .SetLoops(-1)
                .SetLink(gameObject);
        }

        /// <summary>
        /// 表示シーケンスを実行する
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="text">文字列</param>
        /// <param name="color">色</param>
        public void DoPopUpSequence(Vector3 position, string text, Color color)
        {
            // 表示する
            Show(position, text, color);

            // シーケンス
            Sequence sequence = DOTween.Sequence()
                .SetLink(gameObject)
                .OnComplete(Hide);

            // 移動
            var move = RectTransform
                .DOLocalMove(GameSettings.PopUpTexts.MoveVector, GameSettings.PopUpTexts.MoveDuration)
                .SetRelative();

            sequence.Append(move);

            // フェードアウト
            var fadeout = canvasGroup.DOFade(0.0f, GameSettings.PopUpTexts.FadeOutDuration);
            sequence.Append(fadeout);
        }
    }
}
