using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Audio;
using UniRx;

namespace SetsunaTsuyuri
{
    public class AudioManagerLoader : SingletonBehaviourLoader<AudioManager>
    {
        public override AudioManager LoadPrefab()
        {
            return Resources.Load<AudioManager>("SetsunaTsuyuri_Audio");
        }
    }

    /// <summary>
    /// 音声の管理者
    /// </summary>
    public class AudioManager : SingletonBehaviour<AudioManager, AudioManagerLoader>, IInitializable
    {
        /// <summary>
        /// オーディオミキサー
        /// </summary>
        [SerializeField]
        AudioMixer _audioMixer = null;

        /// <summary>
        //// BGMの制御者
        //// </summary>
        [SerializeField]
        AudioSourceController _bgmSource = null;

        /// <summary>
        /// SEの制御者
        /// </summary>
        [SerializeField]
        AudioSourceController _seSource = null;

        /// <summary>
        /// BGMデータグループ
        /// </summary>
        [SerializeField]
        BgmDataGroup _bgmDataGroup = null;

        /// <summary>
        /// SEデータグループ
        /// </summary>
        [SerializeField]
        SEDataGroup _seDataGroup = null;

        /// <summary>
        /// マスター音量
        /// </summary>
        readonly AudioVolume _masterVolume = new();

        /// <summary>
        /// マスター音量
        /// </summary>
        public static AudioVolume MasterVolume => Instance._masterVolume;

        /// <summary>
        /// BGM音量
        /// </summary>
        readonly AudioVolume _bgmVolume = new();

        /// <summary>
        /// BGM音量
        /// </summary>
        public static AudioVolume BgmVolume => Instance._bgmVolume;

        /// <summary>
        /// SE音量
        /// </summary>
        public AudioVolume _seVolume = new();

        /// <summary>
        /// SE音量
        /// </summary>
        public static AudioVolume SEVolume => Instance._seVolume;

        /// <summary>
        /// 保存されたBGM
        /// </summary>
        AudioData _savedBgm = null;

        /// <summary>
        /// 保存されたBGMの再生位置
        /// </summary>
        float _savedBgmPlaybackPosition = 0.0f;

        /// <summary>
        /// BGMのキャンセレーショントークンソース
        /// </summary>
        CancellationTokenSource _bgmCancellationTokenSource = null;

        protected override void Awake()
        {
            base.Awake();

            MasterVolume.Observable
                .TakeUntilDestroy(gameObject)
                .Subscribe(volume => SetMixierVolume("Master", volume));

            BgmVolume.Observable
                .TakeUntilDestroy(gameObject)
                .Subscribe(volume => SetMixierVolume("BGM", volume));

            SEVolume.Observable
                .TakeUntilDestroy(gameObject)
                .Subscribe(volume => SetMixierVolume("SE", volume));
        }

        private void OnDestroy()
        {
            _bgmCancellationTokenSource?.Cancel();
            _bgmCancellationTokenSource?.Dispose();
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="id">BGMID</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="position">再生位置</param>
        public static void PlayBgm(BgmId id, float duration = 0.0f, float position = 0.0f)
        {
            AudioData data = Instance._bgmDataGroup[id];
            if (data is null)
            {
                Debug.LogError($"{id} is not found");
                return;
            }

            PlayBgm(data, duration, position);
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="position">再生位置</param>
        public static void PlayBgm(int id, float duration = 0.0f, float position = 0.0f)
        {
            AudioData data = Instance._bgmDataGroup[id];
            if (data is null)
            {
                Debug.LogError($"ID:{id} is not found");
                return;
            }

            PlayBgm(data, duration, position);
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="position">再生位置</param>
        public static void PlayBgm(string name, float duration = 0.0f, float position = 0.0f)
        {
            AudioData data = Instance._bgmDataGroup[name];
            if (data is null)
            {
                Debug.LogError($"{name} is not found");
                return;
            }

            PlayBgm(data, duration, position);
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="position">再生位置</param>
        private static void PlayBgm(AudioData data, float duration, float position)
        {
            AudioSourceController controller = Instance._bgmSource;

            if (data != controller.Data)
            {
                controller.Data = data;
                controller.PlaybackPosition = position;
            }

            ResetBgmCancellationTokenSource();
            controller.PlayWithFadeIn(duration, Instance._bgmCancellationTokenSource.Token).Forget();
        }

        /// <summary>
        /// BGMを止める
        /// </summary>
        /// <param name="duration">フェード時間</param>
        public static void StopBgm(float duration = 0.5f)
        {
            ResetBgmCancellationTokenSource();
            Instance._bgmSource.StopWithFadeOut(duration, Instance._bgmCancellationTokenSource.Token).Forget();
        }

        /// <summary>
        /// BGMキャンセレーショントークンソースをリセットする
        /// </summary>
        private static void ResetBgmCancellationTokenSource()
        {
            Instance._bgmCancellationTokenSource?.Cancel();
            Instance._bgmCancellationTokenSource?.Dispose();
            Instance._bgmCancellationTokenSource = new();
        }

        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="id">SEID</param>
        /// <param name="volume"></param>
        public static void PlaySE(SEId id, float volume = 1.0f)
        {
            AudioData data = Instance._seDataGroup[id];
            if (data is null)
            {
                Debug.LogError($"ID: {id} is not found");
                return;
            }

            PlaySE(data, volume);
        }

        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        public static void PlaySE(int id, float volume = 1.0f)
        {
            AudioData data = Instance._seDataGroup[id];
            if (data is null)
            {
                Debug.LogError($"ID: {id} is not found");
                return;
            }

            PlaySE(data, volume);
        }

        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="volume"></param>
        public static void PlaySE(string name, float volume = 1.0f)
        {
            AudioData data = Instance._seDataGroup[name];
            if (data is null)
            {
                Debug.LogError($"{name} is not found");
                return;
            }

            PlaySE(data, volume);
        }


        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="name">SEの名前</param>
        /// <param name="volume"></param>
        public static void PlaySE(AudioData data, float volume = 1.0f)
        {
            Instance._seSource.Data = data;
            Instance._seSource.PlayOneShot();
        }

        /// <summary>
        /// BGMを保存する
        /// </summary>
        public static void SaveBgm()
        {
            Instance._savedBgm = Instance._bgmSource.Data;
            Instance._savedBgmPlaybackPosition = Instance._bgmSource.PlaybackPosition;
        }

        /// <summary>
        /// 保存したBGMを再生する
        /// </summary>
        /// <param name="duration"></param>
        public static void LoadBgm(float duration = 0.0f)
        {
            PlayBgm(Instance._savedBgm, duration, Instance._savedBgmPlaybackPosition);
        }

        /// <summary>
        /// オーディオミキサーの音量を設定する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="volume"></param>
        public static void SetMixierVolume(string name, float volume)
        {
            float dB = ToDecibel(volume);
            Instance._audioMixer.SetFloat(name, dB);
        }

        /// <summary>
        /// 音量をデシベルに変換する
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        private static float ToDecibel(float volume)
        {
            return Mathf.Clamp(Mathf.Log10(volume) * 20.0f, -80.0f, 0.0f);
        }
    }
}
