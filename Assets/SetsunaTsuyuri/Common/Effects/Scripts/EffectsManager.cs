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
        /// プールディクショナリー
        /// </summary>
        readonly Dictionary<int, ObjectPool<EffectObject>> _poolDictionary = new();

        public override void Initialize()
        {
            CreateEffectPools();

            SceneManager.sceneLoaded += (_, _) =>
            {
                foreach (var pool in _poolDictionary.Values)
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
            foreach (var data in _effectDataGroup.Data)
            {
                ObjectPool<EffectObject> pool = CreateEffectPool(data);
                _poolDictionary.Add(data.Id, pool);
            }
        }

        /// <summary>
        /// エフェクトのオブジェクトプールを作る
        /// </summary>
        /// <param name="data">エフェクトデータ</param>
        private ObjectPool<EffectObject> CreateEffectPool(EffectObjectData data)
        {
            string idName = data.Id.ToString();
            if (System.Enum.IsDefined(typeof(EffectDataId), data.Id))
            {
                idName = ((EffectDataId)data.Id).ToString();
            }
            
            GameObject poolObject = new(idName);
            poolObject.transform.SetParent(transform, false);

            ObjectPool<EffectObject> pool = new(
                () => EffectObject.Create(this, poolObject.transform, data),
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
            _poolDictionary[effect.Data.Id].Release(effect);
        }

        /// <summary>
        /// エフェクトを再生する
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="isLoop"></param>
        public static void Play(EffectDataId type, Vector3 position, bool isLoop = false)
        {
            Play((int)type, position, isLoop);
        }

        /// <summary>
        /// エフェクトを再生する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <param name="isLoop"></param>
        public static void Play(int id, Vector3 position, bool isLoop = false)
        {
            EffectObject effect = Instance._poolDictionary[id].Get();
            if (effect == null)
            {
                return;
            }

            effect.transform.position = position;
            effect.Play();
        }
    }
}
