using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// MonoBehaviourをプールする者
    /// </summary>
    /// <typeparam name="TPooled"></typeparam>
    [System.Serializable]
    public class MonoBehaviourPool<TPooled> : MonoBehaviour, IInitializable
        where TPooled : MonoBehaviour, IPooled
    {
        /// <summary>
        /// プレファブ
        /// </summary>
        [SerializeField]
        TPooled prefab = null;

        /// <summary>
        /// 初期化時にインスタンスをいくつ作るか
        /// </summary>
        [SerializeField]
        int numberToInstantiate = 0;

        /// <summary>
        /// インスタンスリスト
        /// </summary>
        readonly List<TPooled> instanceList = new List<TPooled>();

        protected virtual void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            // 決められた数だけインスタンスを作る
            for (int i = 0; i < numberToInstantiate; i++)
            {
                TPooled instance = CreateAndAdd();
                instance.Release();
            }
        }

        /// <summary>
        /// プレファブを子オブジェクトとしてしてインスタンス化し、それをリストに加える
        /// </summary>
        /// <returns>インスタンス</returns>
        private TPooled CreateAndAdd()
        {
            TPooled instance = Instantiate(prefab, transform);
            instanceList.Add(instance);
            return instance;
        }

        /// <summary>
        /// 利用可能なインスタンスを探す
        /// </summary>
        /// <returns>見つからなければ初期値を返す</returns>
        private TPooled FindAvailableOrDefault()
        {
            return instanceList.FirstOrDefault(x => x.IsAvailable());
        }

        /// <summary>
        /// インスタンスの取得を試み、取得できなければ作って取得する
        /// </summary>
        /// <returns></returns>
        public TPooled GetOrCreate()
        {
            TPooled instance = FindAvailableOrDefault();
            if (!instance)
            {
                instance = CreateAndAdd();
            }
            instance.OnGot();
            return instance;
        }

        /// <summary>
        /// インスタンスの取得を試みる
        /// </summary>
        /// <param name="instance">インスタンス</param>
        /// <returns></returns>
        public bool TryGet(out TPooled instance)
        {
            instance = FindAvailableOrDefault();
            if (instance)
            {
                instance.OnGot();
            }
            return instance;
        }
    }
}
