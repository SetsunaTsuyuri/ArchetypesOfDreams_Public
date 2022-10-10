using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// エフェクトオブジェクト
    /// </summary>
    public class EffectObject : MonoBehaviour
    {
        /// <summary>
        /// データ
        /// </summary>
        public EffectData Data { get; set; } = null;

        /// <summary>
        /// パーティクルシステム
        /// </summary>
        public ParticleSystem ParticleSystem { get; set; } = null;

        /// <summary>
        /// パーティクルシステムが停止した際の処理
        /// </summary>
        public event UnityAction ParticleSystemStopped = null;

        /// <summary>
        /// エフェクトを作る
        /// </summary>
        /// <param name="manager">エフェクトの管理者</param>
        /// <param name="data">エフェクトデータ</param>
        /// <returns></returns>
        public static EffectObject Create(EffectsManager manager, EffectData data)
        {
            ParticleSystem particleSystem = Instantiate(data.ParticleSystem, manager.transform);

            EffectObject effect = particleSystem.gameObject.AddComponent<EffectObject>();
            effect.Data = data;
            effect.ParticleSystem = particleSystem;
            effect.ParticleSystemStopped += () => manager.ReleaseEffect(effect);

            return effect;
        }

        /// <summary>
        /// 取得された際の処理
        /// </summary>
        public void OnGot()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 解放された際の処理
        /// </summary>
        public void OnReleased()
        {
            Stop();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 再生する
        /// </summary>
        public void Play()
        {
            ParticleSystem.Play();
        }

        /// <summary>
        /// 停止する
        /// </summary>
        public void Stop()
        {
            ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        /// <summary>
        /// 再生中である
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return ParticleSystem.isPlaying;
        }

        private void OnParticleSystemStopped()
        {
            ParticleSystemStopped?.Invoke();
        }
    }
}
