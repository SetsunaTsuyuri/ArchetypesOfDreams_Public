using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// スクロールテキスト
    /// </summary>
    public class ScrollText : GameUI
    {
        /// <summary>
        /// スクロール速度
        /// </summary>
        [SerializeField]
        float _scrollSpeed = 0.0f;

        /// <summary>
        /// 最小スクロールオフセット
        /// </summary>
        [SerializeField]
        float _scrollOffsetMin = 0.0f;

        /// <summary>
        /// 最大スクロールオフセット
        /// </summary>
        [SerializeField]
        float _scrollOffsetMax = 0.0f;

        /// <summary>
        /// テキスト
        /// </summary>
        [SerializeField]
        TextAsset _textAsset = null;

        /// <summary>
        /// テキスト表示
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _textView = null;

        /// <summary>
        /// スクロールオフセット
        /// </summary>
        float _scrollOffset = 0.0f;

        /// <summary>
        /// スクロール方向
        /// </summary>
        public float ScrollDirection { get; set; } = 0.0f;

        private void OnValidate()
        {
            if (!_textView)
            {
                _textView = GetComponentInChildren<TextMeshProUGUI>();
            }

            if (_textAsset)
            {
                _textView.text = _textAsset.text;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (!_textView)
            {
                _textView = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void Update()
        {
            if (ScrollDirection == 0.0f)
            {
                return;
            }

            UpdateOffset();
            UpdatePosition();
        }

        /// <summary>
        /// スクロールオフセットを更新する
        /// </summary>
        private void UpdateOffset()
        {
            float speed = _scrollSpeed  * -ScrollDirection * Time.deltaTime;
            _scrollOffset += speed;
            _scrollOffset = Mathf.Clamp(_scrollOffset, _scrollOffsetMin, _scrollOffsetMax);
        }

        /// <summary>
        /// テキストの位置を更新する
        /// </summary>
        private void UpdatePosition()
        {
            Vector2 position = _textView.rectTransform.anchoredPosition;
            position.y = _scrollOffset;
            _textView.rectTransform.anchoredPosition = position;
        }
    }
}
