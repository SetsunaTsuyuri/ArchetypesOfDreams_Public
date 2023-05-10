using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 説明文の管理者
    /// </summary>
    public class DescriptionUI : GameUI
    {
        /// <summary>
        /// 説明文
        /// </summary>
        TextMeshProUGUI _text = null;

        protected override void Awake()
        {
            base.Awake();

            _text = GetComponentInChildren<TextMeshProUGUI>(true);
        }

        private void Start()
        {
            // 戦闘開始
            MessageBrokersManager.BattleStart
                .Receive<Battle>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => SetEnabled(true));

            // 戦闘終了
            MessageBrokersManager.BattleEnd
                .Receive<Battle>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => SetEnabled(false));
        }

        /// <summary>
        /// 文章を設定する
        /// </summary>
        /// <param name="text">文章の文字列</param>
        public void SetText(string text)
        {
            _text.alignment = TextAlignmentOptions.Left;
            _text.text = text;
        }

        /// <summary>
        /// 文章をクリアする
        /// </summary>
        public void ClearText()
        {
            _text.text = string.Empty;
        }

        /// <summary>
        /// 行動名を表示する
        /// </summary>
        /// <param name="action">行動内容</param>
        public void DisplayActionName(ActionInfo action)
        {
            _text.alignment = TextAlignmentOptions.Center;
            _text.text = action.Effect.Name;
        }
    }
}
