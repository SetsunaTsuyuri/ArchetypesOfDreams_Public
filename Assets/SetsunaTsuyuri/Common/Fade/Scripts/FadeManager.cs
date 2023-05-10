using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// プレハブローダー
    /// </summary>
    public class FadeManagerLoader : SingletonBehaviourLoader<FadeManager>
    {
        public override FadeManager LoadPrefab()
        {
            return Resources.Load<FadeManager>("SetsunaTsuyuri_Fade");
        }
    }

    /// <summary>
    /// フェード演出の管理者
    /// </summary>
    public class FadeManager : SingletonBehaviour<FadeManager, FadeManagerLoader>, IInitializable
    {
        /// <summary>
        /// フェードの状態
        /// </summary>
        private enum FadeState
        {
            /// <summary>
            /// なし
            /// </summary>
            None = 0,

            /// <summary>
            /// フェードイン
            /// </summary>
            FadeIn = 1,

            /// <summary>
            /// フェードアウト
            /// </summary>
            FadeOut = 2
        }

        /// <summary>
        /// 現在のフェード状態
        /// </summary>
        FadeState state = FadeState.None;

        /// <summary>
        /// フェード時間
        /// </summary>
        [SerializeField]
        float fadeDuration = 0.2f;

        /// <summary>
        /// フェードアウトのマテリアル
        /// </summary>
        [SerializeField]
        Material fadeOut = null;

        /// <summary>
        /// フェードインのマテリアル
        /// </summary>
        [SerializeField]
        Material fadeIn = null;

        /// <summary>
        /// フェードするスプライト
        /// </summary>
        [SerializeField]
        Sprite sprite = null;

        /// <summary>
        /// フェードするイメージ
        /// </summary>
        Image image = null;

        public override void Initialize()
        {
            image = GetComponentInChildren<Image>();
            image.sprite = sprite;
            image.material = fadeIn;
            image.material.SetFloat("_Alpha", 1.0f);
        }

        /// <summary>
        /// フェードアウトを開始する
        /// </summary>
        public static void StartFadeOut()
        {
            CancellationToken token = Instance.GetCancellationTokenOnDestroy();
            FadeOut(token).Forget();
        }

        /// <summary>
        /// フェードインを開始する
        /// </summary>
        public static void StartFadeIn()
        {
            CancellationToken token = Instance.GetCancellationTokenOnDestroy();
            FadeIn(token).Forget();
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask FadeOut(CancellationToken token)
        {
            await Instance.FadeOutInner(Instance.fadeDuration, token);
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask FadeOut(float duration, CancellationToken token)
        {
            await Instance.FadeOutInner(duration, token);
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask FadeOutInner(float duration, CancellationToken token)
        {
            if (state == FadeState.FadeOut)
            {
                return;
            }

            await Fade(fadeOut, duration, token);

            state = FadeState.FadeOut;
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask FadeIn(CancellationToken token)
        {
            await Instance.FadeInInner(Instance.fadeDuration, token);
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask FadeIn(float duration, CancellationToken token)
        {
            await Instance.FadeInInner(duration, token);
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask FadeInInner(float duration, CancellationToken token)
        {
            if (state == FadeState.FadeIn)
            {
                return;
            }

            await Fade(fadeIn, duration, token);

            state = FadeState.FadeIn;
        }

        /// <summary>
        /// フェード処理を行う
        /// </summary>
        /// <param name="material">マテリアル</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask Fade(Material material, float duration, CancellationToken token)
        {
            image.material = material;

            float time = 0.0f;
            while (time < duration)
            {
                material.SetFloat("_Alpha", time / duration);
                await UniTask.Yield(token);
                time += Time.deltaTime;
            }
            material.SetFloat("_Alpha", 1.0f);
        }
    }
}
