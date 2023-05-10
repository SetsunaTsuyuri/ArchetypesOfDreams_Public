using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// BGMイベントタイプ
    /// </summary>
    public enum BgmEventType
    {
        None = 0,

        /// <summary>
        /// 再生
        /// </summary>
        Play = 1,

        /// <summary>
        /// 停止
        /// </summary>
        Stop = 2,

        /// <summary>
        /// 保存
        /// </summary>
        Save = 3,

        /// <summary>
        /// 保存されたBGMを再生
        /// </summary>
        Load = 4,
    }

    /// <summary>
    /// BGMイベント
    /// </summary>
    public class BgmEvent : GameEventBase
    {
        /// <summary>
        /// デフォルト音量
        /// </summary>
        private static readonly float s_defaultVolume = 1.0f;

        /// <summary>
        /// デフォルトフェード時間
        /// </summary>
        private static readonly float s_defaultFadeDuration = 0.5f;

        /// <summary>
        /// イベントタイプ
        /// </summary>
        [field: SerializeField]
        public BgmEventType EventType { get; set; } = BgmEventType.None;

        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; set; } = 0;

        /// <summary>
        /// 音量
        /// </summary>
        [field: SerializeField]
        public float Volume { get; set; } = 0.0f;

        /// <summary>
        /// フェード時間
        /// </summary>
        [field: SerializeField]
        public float FadeDuration { get; set; } = 0.0f;

        public BgmEvent(string[] columns)
        {
            EventType = ToEnum<BgmEventType>(columns, 1);
            Id = ToInt(columns, 2);
            Volume = ToFloat(columns, 3, s_defaultVolume);
            FadeDuration = ToFloat(columns, 4, s_defaultFadeDuration);
        }

        public override async UniTask Resolve(CancellationToken token)
        {
            UnityAction action = EventType switch
            {
                BgmEventType.Play => () => AudioManager.PlayBgm(Id, FadeDuration),
                BgmEventType.Stop => () => AudioManager.StopBgm(FadeDuration),
                BgmEventType.Save => AudioManager.SaveBgm,
                BgmEventType.Load => () => AudioManager.LoadBgm(FadeDuration),
                _ => null
            };
            action?.Invoke();

            await UniTask.CompletedTask;
        }
    }
}
