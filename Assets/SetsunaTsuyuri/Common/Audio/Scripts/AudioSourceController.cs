using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// AudioSourceの制御者
    /// </summary>
    public class AudioSourceController : MonoBehaviour
    {
        /// <summary>
        /// AudioSource
        /// </summary>
        AudioSource _audioSource = null;

        /// <summary>
        /// 再生中のBGMID
        /// </summary>
        int playingBgmId = -1;

        /// <summary>
        /// オーディオデータ
        /// </summary>
        AudioData _data = null;

        /// <summary>
        /// オーディオデータ
        /// </summary>
        public AudioData Data
        {
            get => _data;
            set
            {
                _data = value;
                _audioSource.clip = _data.Clip;
                _audioSource.volume = _data.Volume;
            }
        }

        /// <summary>
        /// AudioClip
        /// </summary>
        public AudioClip Clip
        {
            get => _audioSource.clip;
            set => _audioSource.clip = value;
        }

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume
        {
            get => _audioSource.volume;
            set => _audioSource.volume = value;
        }

        /// <summary>
        /// 再生位置
        /// </summary>
        public float PlaybackPosition
        {
            get => _audioSource.time;
            set => _audioSource.time = value;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// 1度だけ再生する
        /// </summary>
        public void PlayOneShot()
        {
            _audioSource.PlayOneShot(_audioSource.clip, Data.Volume);
        }

        /// <summary>
        /// フェードインしながら再生する
        /// </summary>
        /// <param name="fadeDuration">フェード時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask PlayWithFadeIn(float fadeDuration, CancellationToken token)
        {
            // 既に同じBGMを再生しているなら、PlayしないでBGMを継続する
            if (Data.Id != playingBgmId)
            {
                _audioSource.Play();
                playingBgmId = Data.Id;
            }

            await ChangeVolumeAsync(0.0f, Data.Volume, fadeDuration, token);
        }

        /// <summary>
        /// フェードアウトしながら停止する
        /// </summary>
        /// <param name="fadeDuration"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask StopWithFadeOut(float fadeDuration, CancellationToken token)
        {
            // 再生中のBGMIDをリセットする
            ResetPlayingBGMId();

            await ChangeVolumeAsync(_audioSource.volume, 0.0f, fadeDuration, token);
        }

        /// <summary>
        /// 音量を非同期的に変更する
        /// </summary>
        /// <param name="start">開始時の音量</param>
        /// <param name="end">終了時の音量</param>
        /// <param name="duration">フェード時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask ChangeVolumeAsync(float start, float end, float duration, CancellationToken token)
        {
            try
            {
                // 開始時の音量にする
                _audioSource.volume = start;

                // フェード経過時間
                float elapsed = 0.0f;

                // フェードループ
                while (elapsed < duration)
                {
                    // 音量変更
                    float lerp = elapsed / duration;
                    float lerpVolume = Mathf.Lerp(start, end, lerp);
                    _audioSource.volume = lerpVolume;

                    // 経過時間を増やす
                    elapsed += Time.deltaTime;

                    // 待機
                    await UniTask.Yield(token);
                }

                // 終了時の音量にする
                _audioSource.volume = end;

            }
            catch (OperationCanceledException) { }
        }

        /// <summary>
        /// 再生中のBGMIDをリセットする
        /// </summary>
        private void ResetPlayingBGMId()
        {
            playingBgmId = -1;
        }
    }
}
