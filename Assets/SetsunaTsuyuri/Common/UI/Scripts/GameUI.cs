using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    }
}
