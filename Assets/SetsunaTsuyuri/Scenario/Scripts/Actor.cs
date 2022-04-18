using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.Scenario.Attribute
{
    /// <summary>
    /// 位置
    /// </summary>
    public enum Position
    {
        Auto = -1,
        Center = 0,
        Left = 1,
        Right = 2
    }

    /// <summary>
    /// 表示方法
    /// </summary>
    public enum Display
    {
        /// <summary>
        /// 変更なし
        /// </summary>
        NoChange = -1,

        /// <summary>
        /// 通常
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 半透明
        /// </summary>
        Translucent = 1
    }

    /// <summary>
    /// 表情
    /// </summary>
    public enum Expression
    {
        /// <summary>
        /// 変更なし
        /// </summary>
        NoChange = -1,

        /// <summary>
        /// 通常
        /// </summary>
        Normal = 0,

        Smile,
        Angry,
        Sad,
        Fear,
        Surprised,
        Tearful
    }
}

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// 演者
    /// </summary>
    public class Actor : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// 設定
        /// </summary>
        [field: SerializeField]
        public ActorSettings Settings { get; private set; } = null;

        /// <summary>
        /// 位置
        /// </summary>
        [field: SerializeField]
        public Attribute.Position Position { get; private set; } = Attribute.Position.Center;

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// データ
        /// </summary>
        ActorData data = null;

        /// <summary>
        /// データ
        /// </summary>
        public ActorData Data
        {
            get => data;
            set
            {
                data = value;
                OnDataSet();
            }
        }

        /// <summary>
        /// 表示方法
        /// </summary>
        public Attribute.Display Display { get; private set; } = Attribute.Display.Normal;

        /// <summary>
        /// 表情タイプ
        /// </summary>
        public Attribute.Expression Expression { get; set; } = Attribute.Expression.Normal;

        /// <summary>
        /// ImageControllerコンポーネント
        /// </summary>
        public ImageController ImageController { get; private set; } = null;

        /// <summary>
        /// Imageコンポーネント
        /// </summary>
        public Image Image { get => ImageController.MainImage; }

        public void Initialize()
        {
            Data = null;
        }

        private void Awake()
        {
            ImageController = GetComponent<ImageController>();
        }

        private void Start()
        {
            RefreshImage();
        }

        /// <summary>
        /// データが設定されたときの処理
        /// </summary>
        private void OnDataSet()
        {
            if (Data != null)
            {
                ChangeExpression(Attribute.Expression.Normal);
            }

            RefreshImage();
        }

        /// <summary>
        /// Imageを更新する
        /// </summary>
        private void RefreshImage()
        {
            if (Data != null)
            {
                Image.sprite = Data.Expressions.GetValueOrDefault(Expression);
                Image.color = Color.white;
                Image.SetNativeSize();
                Image.rectTransform.anchoredPosition = Data.PositionOffset;
            }
            else
            {
                Image.sprite = null;
                Image.color = Color.clear;
            }
        }

        /// <summary>
        /// フェードインする
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask FadeIn(CancellationToken token)
        {
            await ImageController.ChangeAlphaAsync(ImageController.MainImage, 0.0f, 1.0f, Settings.FadeDuration, token);
        }

        /// <summary>
        /// フェードアウトする
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask FadeOut(CancellationToken token)
        {
            await ImageController.CrossFade(null, Settings.FadeDuration, token);
            //await ImageController.ChangeAlphaAsync(ImageController.MainImage, 1.0f, 0.0f, Settings.FadeDuration, token);
            Data = null;
        }

        /// <summary>
        /// 表示方法を設定する
        /// (未完成 フェードイン・アウト時のアルファ値変更と相性が悪い)
        /// </summary>
        /// <param name="display">表示方法</param>
        public void ChangeDisplayMethod(Attribute.Display display)
        {
            if (display == Attribute.Display.NoChange ||
                display == Display)
            {
                return;
            }

            Display = display;
            switch (display)
            {
                case Attribute.Display.Normal:

                    Image.ChangeAlpha(1.0f);
                    break;

                case Attribute.Display.Translucent:

                    Image.ChangeAlpha(0.75f);
                    break;
            }
        }

        /// <summary>
        /// 表情を設定する
        /// </summary>
        /// <param name="type">種類</param>
        public void ChangeExpression(Attribute.Expression type)
        {
            if (type == Attribute.Expression.NoChange ||
                type == Expression)
            {
                return;
            }

            Sprite sprite = Data.Expressions.GetValueOrDefault(type);
            if (sprite)
            {
                Image.sprite = sprite;
                Expression = type;
            }
            else
            {
                Debug.LogWarning("指定された表情のスプライトが存在しません");
            }
        }

        /// <summary>
        /// 明るくする
        /// </summary>
        public void ToBrightColor()
        {
            Image.ChangeRGB(Color.white);
        }

        /// <summary>
        /// 暗くする
        /// </summary>
        public void ToDarkColor()
        {
            Image.ChangeRGB(Settings.DarkColor);
        }

        /// <summary>
        /// データを持っている
        /// </summary>
        /// <returns></returns>
        public bool HasData()
        {
            return Data != null;
        }

        /// <summary>
        /// データを持っていてなおかつ名前が一致している
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns></returns>
        public bool HasDataAndMatch(string name)
        {
            return HasData() && Data.Name == name;
        }
    }
}
