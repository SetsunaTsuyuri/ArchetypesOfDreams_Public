using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

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
        //// BGMの制御者
        //// </summary>
        [SerializeField]
        AudioSourceController bgmSource = null;

        private static AudioSourceController BgmSource => Instance.bgmSource;

        /// <summary>
        /// SEの制御者
        /// </summary>
        [SerializeField]
        AudioSourceController seSource = null;

        private static AudioSourceController SESource => Instance.seSource;

        /// <summary>
        /// BGMデータグループ
        /// </summary>
        [SerializeField]
        BgmDataGroup _bgmDataGroup = null;

        private static BgmDataGroup BgmDataGroup => Instance._bgmDataGroup;

        /// <summary>
        /// SEデータグループ
        /// </summary>
        [SerializeField]
        SEDataGroup _seDataGroup = null;

        private static SEDataGroup SEDataGroup => Instance._seDataGroup;

        /// <summary>
        /// 保存されたBGM
        /// </summary>
        AudioData _savedBgm = null;

        private static AudioData SavedBgm
        {
            get => Instance._savedBgm;
            set => Instance._savedBgm = value;
        }

        /// <summary>
        /// 保存されたBGMの再生位置
        /// </summary>
        float _savedBgmPlaybackPosition = 0.0f;

        private static float SavedBgmPlaybackPosition
        {
            get => Instance._savedBgmPlaybackPosition;
            set => Instance._savedBgmPlaybackPosition = value;
        }

        /// <summary>
        /// BGMのキャンセレーショントークンソース
        /// </summary>
        CancellationTokenSource _bgmCancellationTokenSource = null;

        private static CancellationTokenSource BgmCancellationTokenSource
        {
            get => Instance._bgmCancellationTokenSource;
            set => Instance._bgmCancellationTokenSource = value;
        }

        private void OnDestroy()
        {
            _bgmCancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="type">BGMタイプ</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="position">再生位置</param>
        public static void PlayBgm(BgmType type, float duration = 0.0f, float position = 0.0f)
        {
            AudioData data = BgmDataGroup[type];
            if (data is null)
            {
                Debug.LogError($"{type} is not found");
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
            AudioData data = BgmDataGroup[name];
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
        /// <param name="id">ID</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="position">再生位置</param>
        public static void PlayBgm(int id, float duration = 0.0f, float position = 0.0f)
        {
            AudioData data = BgmDataGroup[id];
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
        /// <param name="data">データ</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="position">再生位置</param>
        private static void PlayBgm(AudioData data, float duration, float position)
        {
            if (data != BgmSource.Data)
            {
                BgmSource.Data = data;
                BgmSource.PlaybackPosition = position;
            }

            BgmCancellationTokenSource?.Cancel();
            BgmCancellationTokenSource = new CancellationTokenSource();

            BgmSource.PlayWithFadeIn(duration, BgmCancellationTokenSource.Token).Forget();
        }

        /// <summary>
        /// BGMを止める
        /// </summary>
        /// <param name="duration">フェード時間</param>
        public static void StopBgm(float duration = 0.5f)
        {
            BgmCancellationTokenSource?.Cancel();
            BgmCancellationTokenSource = new CancellationTokenSource();

            BgmSource.StopWithFadeOut(duration, BgmCancellationTokenSource.Token).Forget();
        }

        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="type">SEタイプ</param>
        public static void PlaySE(SEType type)
        {
            AudioData data = SEDataGroup[type];
            if (data is null)
            {
                Debug.LogError($"{type} is not found");
                return;
            }

            PlaySE(data);
        }

        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="name">SEの名前</param>
        public static void PlaySE(string name)
        {
            AudioData data = SEDataGroup[name];
            if (data is null)
            {
                Debug.LogError($"{name} is not found");
                return;
            }

            PlaySE(data);
        }

        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="name">SEの名前</param>
        public static void PlaySE(AudioData data)
        {
            SESource.Data = data;
            SESource.PlayOneShot();
        }

        /// <summary>
        /// BGMを保存する
        /// </summary>
        public static void SaveBgm()
        {
            SavedBgm = BgmSource.Data;
            SavedBgmPlaybackPosition = BgmSource.PlaybackPosition;
        }

        /// <summary>
        /// 保存したBGMを再生する
        /// </summary>
        /// <param name="duration"></param>
        public static void LoadBgm(float duration = 0.0f)
        {
            PlayBgm(SavedBgm, duration, SavedBgmPlaybackPosition);
        }
    }
}
