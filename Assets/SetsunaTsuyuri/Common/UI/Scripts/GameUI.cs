﻿using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// UIの基底クラス
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class GameUI : MonoBehaviour
    {
        /// <summary>
        /// キャンバス
        /// </summary>
        protected Canvas _canvas;

        /// <summary>
        /// キャンバスグループ
        /// </summary>
        protected CanvasGroup _canvasGroup = null;

        /// <summary>
        /// フェードTween
        /// </summary>
        Tween _fadeTween = null;

        /// <summary>
        /// 有効である
        /// </summary>
        /// <returns></returns>
        public bool IsEnabled => _canvas.enabled;

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// interactableを設定する
        /// </summary>
        /// <param name="interactable"></param>
        public virtual void SetInteractable(bool interactable)
        {
            _canvasGroup.interactable = interactable;
        }

        /// <summary>
        /// 見せる
        /// </summary>
        public virtual void Show()
        {
            _canvas.enabled = true;
            SetInteractable(true);
        }

        /// <summary>
        /// 隠す
        /// </summary>
        public virtual void Hide()
        {
            _canvas.enabled = false;
            SetInteractable(false);
        }

        /// <summary>
        /// 表示してアクティブにする
        /// </summary>
        public virtual void ActivateAndShow()
        {
            gameObject.SetActive(true);
            Show();
        }

        /// <summary>
        /// 隠して非アクティブにする
        /// </summary>
        public virtual void DeactivateAndHide()
        {
            gameObject.SetActive(false);
            Hide();
        }

        /// <summary>
        /// フェードインする
        /// </summary>
        /// <param name="duration"></param>
        public void FadeIn(float duration = 0.5f)
        {
            KillIfFadeTweenIsActive();

            _canvas.enabled = true;
            _canvasGroup.alpha = 0.0f;

            _fadeTween = _canvasGroup.DOFade(1.0f, duration)
                .SetLink(gameObject);
        }

        /// <summary>
        /// フェードアウトする
        /// </summary>
        /// <param name="duration"></param>
        public void FadeOut(float duration = 0.5f)
        {
            KillIfFadeTweenIsActive();

            _fadeTween = _canvasGroup.DOFade(0.0f, duration)
                .SetLink(gameObject)
                .OnComplete(() => _canvas.enabled = true);
        }

        /// <summary>
        /// フェードTweenがアクティブの場合、それを終了する
        /// </summary>
        private void KillIfFadeTweenIsActive()
        {
            if (_fadeTween.IsActive())
            {
                _fadeTween.Kill();
            }
        }
    }
}
