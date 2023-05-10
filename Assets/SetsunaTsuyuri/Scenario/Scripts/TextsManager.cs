using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// 文章の管理者
    /// </summary>
    public class TextsManager : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// 自動で台詞を送る
        /// </summary>
        [field: SerializeField]
        public bool IsAuto { get; set; } = false;

        /// <summary>
        /// 台詞表示速度
        /// </summary>
        [SerializeField]
        float _waitTimeForDialog = 0.025f;

        /// <summary>
        /// 自動的に台詞を送る際に最低限待つ時間
        /// </summary>
        [SerializeField]
        public float _waitTimeForAuto = 3.0f;

        /// <summary>
        /// 1文字毎の待機時間
        /// </summary>
        [SerializeField]
        public float _waitTimePerCharacter = 0.1f;

        /// <summary>
        /// 演者の名前を伏せる場合に表示される名前
        /// </summary>
        [SerializeField]
        string _anonymous = "???";

        /// <summary>
        /// 名前テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _name = null;

        /// <summary>
        /// 台詞テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _dialog = null;

        /// <summary>
        /// スキップが要求されている
        /// </summary>
        public event System.Func<bool> IsRequestedToSkip = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="isRequestedToSkip"></param>
        public void SetUp(System.Func<bool> isRequestedToSkip)
        {
            IsRequestedToSkip += isRequestedToSkip;
        }

        public void Initialize()
        {
            Clear();
        }

        /// <summary>
        /// クリアする
        /// </summary>
        public void Clear()
        {
            _name.text = string.Empty;
            _dialog.text = string.Empty;
        }

        public void SetNameText(string name)
        {
            _name.text = name;
        }

        public void SetAnonymousText()
        {
            _name.text = _anonymous;
        }

        public async UniTask Play(string dialog, CancellationToken token)
        {
            // 表示できる文字数をゼロにする
            _dialog.maxVisibleCharacters = 0;

            // 台詞を更新する
            _dialog.text = dialog;
            _dialog.ForceMeshUpdate();

            // 1文字ずつ表示する
            int displayCharacters = 0;
            int maxCharacters = _dialog.GetParsedText().Length;
            while (displayCharacters < maxCharacters)
            {
                displayCharacters++;
                _dialog.maxVisibleCharacters = displayCharacters;

                float time = 0.0f;
                bool skip = false;
                while (time < _waitTimeForDialog)
                {
                    time += Time.deltaTime;
                    await UniTask.Yield(token);

                    // スキップ
                    if (IsRequestedToSkip.Invoke())
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip)
                {
                    break;
                }
            }

            // 全ての文字を表示する
            _dialog.maxVisibleCharacters = maxCharacters;
            await UniTask.Yield(token);

            // 自動モード時の待機時間カウンター
            float timeCountForAuto = 0.0f;

            // 文字数に応じて待機時間を変える
            float maxTimeCountForAuto = dialog.Length * _waitTimePerCharacter;

            // 待機時間が短すぎないようにする
            if (maxTimeCountForAuto < _waitTimeForAuto)
            {
                maxTimeCountForAuto = _waitTimeForAuto;
            }

            // 入力 or (自動モードのみ)待機時間終了を待つ
            while (!IsRequestedToSkip())
            {
                if (IsAuto && timeCountForAuto >= maxTimeCountForAuto)
                {
                    break;
                }
                else if (timeCountForAuto < maxTimeCountForAuto)
                {
                    timeCountForAuto += Time.deltaTime;
                }

                await UniTask.Yield(token);
            }
        }
    }
}
