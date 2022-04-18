using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// Imageを操作するもの
    /// </summary>
    public class ImageController : MonoBehaviour
    {
        public Image MainImage { get; set; } = null;

        public Image OutImage { get; set; } = null;

        /// <summary>
        /// スキップが要求されている
        /// </summary>
        public event Func<bool> IsRequestedToSkip = null;

        /// <summary>
        /// Imageにスプライトが設定されたとき、Imageの大きさをスプライトに合わせる
        /// </summary>
        [SerializeField]
        bool setNativeSizeOnSpriteSet = false;

        protected virtual void Awake()
        {
            Image[] images = GetComponentsInChildren<Image>();
            MainImage = images.GetValueOrDefault(0);
            OutImage = images.GetValueOrDefault(1);
        }

        /// <summary>
        /// クロスフェードを行う
        /// </summary>
        /// <param name="sprite">新しいスプライト</param>
        /// <param name="duration">時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask CrossFade(Sprite sprite, float duration, CancellationToken token)
        {
            List<UniTask> tasks = new List<UniTask>();

            // 古いスプライトをフェードアウト
            if (MainImage.sprite)
            {
                OutImage.sprite = MainImage.sprite;
                OutImage.color = MainImage.color;

                if (setNativeSizeOnSpriteSet)
                {
                    OutImage.SetNativeSize();
                }

                // 座標を合わせる
                OutImage.rectTransform.anchoredPosition = MainImage.rectTransform.anchoredPosition;

                MainImage.ChangeAlpha(0.0f);
                tasks.Add(ChangeAlphaAsync(OutImage, 1.0f, 0.0f, duration, token));
            }

            // 新しいスプライトをフェードイン
            if (sprite)
            {
                MainImage.sprite = sprite;
                
                if (setNativeSizeOnSpriteSet)
                {
                    MainImage.SetNativeSize();
                }

                tasks.Add(ChangeAlphaAsync(MainImage, 0.0f, 1.0f, duration, token));
            }

            await UniTask.WhenAll(tasks);
        }

        /// <summary>
        /// Imageの不透明度を非同期的に変更する
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="start">開始時の不透明度</param>
        /// <param name="end">終了時の不透明度</param>
        /// <param name="duration">時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask ChangeAlphaAsync(Image target, float start, float end, float duration, CancellationToken token)
        {
            // フェード経過時間
            float elapsed = 0.0f;

            // フェードループ
            while (elapsed < duration)
            {
                float lerp = elapsed / duration;
                float lerpAlpha = Mathf.Lerp(start, end, lerp);

                target.ChangeAlpha(lerpAlpha);

                elapsed += Time.deltaTime;
                await UniTask.Yield(token);

                // スキップ
                if (IsRequestedToSkip != null && IsRequestedToSkip.Invoke())
                {
                    break;
                }
            }

            // フェード終了
            target.ChangeAlpha(end);
        }

        /// <summary>
        /// Imageの色を非同期的に変更する
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="start">開始時の色</param>
        /// <param name="end">終了時の色</param>
        /// <param name="duration">時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask ChangeColorAsync(Image target, Color start, Color end, float duration, CancellationToken token)
        {
            // フェード経過時間
            float elapsed = 0.0f;

            // フェードループ
            while (elapsed < duration)
            {
                float lerp = elapsed / duration;
                Color lerpColor = Color.Lerp(start, end, lerp);

                target.color = lerpColor;

                elapsed += Time.deltaTime;
                await UniTask.Yield(token);

                // スキップ
                if (IsRequestedToSkip != null && IsRequestedToSkip.Invoke())
                {
                    break;
                }
            }

            // フェード終了
            target.color = end;
        }
    }
}
