using System.Collections.Generic;
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
        /// シェイクTween
        /// </summary>
        Tween _shakeTween = null;

        /// <summary>
        /// 有効状態の保存
        /// </summary>
        bool? _enabledSave = false;

        /// <summary>
        /// 表示されている
        /// </summary>
        /// <returns></returns>
        public bool IsEnabled => _canvas.enabled && _canvasGroup.alpha > 0.0f;

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// 有効状態を設定する
        /// </summary>
        /// <param name="enabled"></param>
        public virtual void SetEnabled(bool enabled)
        {
            _canvas.enabled = enabled;
            SetInteractable(enabled);
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
        /// 有効状態を保存してから非表示にする
        /// </summary>
        public void SaveEnabledAndHide()
        {
            _enabledSave = IsEnabled;
            if (IsEnabled)
            {
                SetEnabled(false);
            }
        }

        /// <summary>
        /// 有効状態を再生する
        /// </summary>
        public void LoadEnabled()
        {
            if (!_enabledSave.HasValue)
            {
                return;
            }

            SetEnabled(_enabledSave.Value);
            _enabledSave = null;
        }

        /// <summary>
        /// 表示してアクティブにする
        /// </summary>
        public virtual void ActivateAndShow()
        {
            gameObject.SetActive(true);
            SetEnabled(true);
        }

        /// <summary>
        /// 隠して非アクティブにする
        /// </summary>
        public virtual void DeactivateAndHide()
        {
            gameObject.SetActive(false);
            SetEnabled(false);
        }

        /// <summary>
        /// フェードインする
        /// </summary>
        /// <param name="duration"></param>
        public virtual void FadeIn(float duration = 0.5f)
        {
            KillIfFadeTweenIsActive();

            SetEnabled(true);

            _canvasGroup.alpha = 0.0f;
            _fadeTween = _canvasGroup.DOFade(1.0f, duration)
                .SetLink(gameObject);
        }

        /// <summary>
        /// フェードアウトする
        /// </summary>
        /// <param name="duration"></param>
        public virtual void FadeOut(float duration = 0.5f)
        {
            KillIfFadeTweenIsActive();

            SetInteractable(false);

            _fadeTween = _canvasGroup.DOFade(0.0f, duration)
                .SetLink(gameObject)
                .OnComplete(() => _canvas.enabled = false);
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
        
        /// <summary>
        /// パンチする
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="duration"></param>
        /// <param name="vibrato"></param>
        public void Punch(Vector3 vector, float duration, int vibrato)
        {
            if (_shakeTween.IsActive())
            {
                _shakeTween.Complete();
            }

            _shakeTween = transform
                .DOPunchPosition(vector, duration, vibrato)
                .SetLink(gameObject);
        }
    }
}
