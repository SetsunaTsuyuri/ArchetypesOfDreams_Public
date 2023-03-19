using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SetsunaTsuyuri
{
    public class EffectsManagerLoader : SingletonBehaviourLoader<EffectsManager>
    {
        public override EffectsManager LoadPrefab()
        {
            return Resources.Load<EffectsManager>("SetsunaTsuyuri_Effects");
        }
    }

    /// <summary>
    /// エフェクトの管理者
    /// </summary>
    public class EffectsManager : SingletonBehaviour<EffectsManager, EffectsManagerLoader>, IInitializable
    {
        /// <summary>
        /// エフェクトデータグループ
        /// </summary>
        [SerializeField]
        EffectDataGroup _effectDataGroup = null;

        /// <summary>
        /// エフェクトプール配列
        /// </summary>
        public ObjectPool<EffectObject>[] _effectPools = { };

        public override void Initialize()
        {
            CreateEffectPools();

            SceneManager.sceneLoaded += (_, _) =>
            {
                foreach (var pool in _effectPools)
                {
                    pool.ReleaseAll();
                }
            };
        }

        /// <summary>
        /// 各エフェクトのオブジェクトプールを作る
        /// </summary>
        private void CreateEffectPools()
        {
            int length = _effectDataGroup.Data.Length;
            _effectPools = new ObjectPool<EffectObject>[length];
            for (int i = 0; i < length; i++)
            {
                EffectData data = _effectDataGroup[i];
                ObjectPool<EffectObject> pool = CreateEffectPool(data);
                _effectPools[i] = pool;
            }
        }

        /// <summary>
        /// エフェクトのオブジェクトプールを作る
        /// </summary>
        /// <param name="data">エフェクトデータ</param>
        private ObjectPool<EffectObject> CreateEffectPool(EffectData data)
        {
            ObjectPool<EffectObject> pool = new(
                () => EffectObject.Create(this, data),
                (e) => e.OnGot(),
                (e) => e.OnReleased(),
                data.PoolCaptity);

            return pool;
        }

        /// <summary>
        /// エフェクトを解放する
        /// </summary>
        /// <param name="effect"></param>
        public void ReleaseEffect(EffectObject effect)
        {
            _effectPools[effect.Data.Id].Release(effect);
        }

        /// <summary>
        /// エフェクトを再生する
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="isLoop"></param>
        public static void Play(EffectType type, Vector3 position, bool isLoop)
        {
            EffectObject effect = Instance._effectPools[(int)type].Get();
            effect.transform.position = position;
            effect.Play();
        }
    }
}
