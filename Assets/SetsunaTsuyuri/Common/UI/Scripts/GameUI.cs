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
        protected Canvas canvas;

        /// <summary>
        /// キャンバスグループ
        /// </summary>
        protected CanvasGroup canvasGroup = null;

        protected virtual void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// interactableを設定する
        /// </summary>
        /// <param name="interactable"></param>
        public virtual void SetInteractable(bool interactable)
        {
            canvasGroup.interactable = interactable;
        }

        /// <summary>
        /// 表示する
        /// </summary>
        public virtual void Show()
        {
            canvas.enabled = true;
            SetInteractable(true);
        }

        /// <summary>
        /// 隠す
        /// </summary>
        public virtual void Hide()
        {
            canvas.enabled = false;
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
