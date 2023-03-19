using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// シングルトンなMonoBehaviourプレハブローダー
    /// </summary>
    /// <typeparam name="TBehaviour"</typeparam>
    public abstract class SingletonBehaviourLoader<TBehaviour>
        where TBehaviour : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// プレハブをロードする
        /// </summary>
        /// <returns></returns>
        public abstract TBehaviour LoadPrefab();
    }

    /// <summary>
    /// シングルトンなMonoBehaviour
    /// </summary>
    /// <typeparam name="TBehaviour"></typeparam>
    /// <typeparam name="TLoader"></typeparam>
    public abstract class SingletonBehaviour<TBehaviour, TLoader> : MonoBehaviour
        where TBehaviour : MonoBehaviour, IInitializable
        where TLoader : SingletonBehaviourLoader<TBehaviour>, new()
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        static TBehaviour s_instance = null;

        /// <summary>
        /// インスタンス
        /// </summary>
        public static TBehaviour Instance
        {
            get
            {
                if (!s_instance)
                {
                    // プレハブをロードする
                    TLoader loader = new();
                    TBehaviour prefab = loader.LoadPrefab();

                    // ロードしたプレハブをインスタンス化する
                    Instantiate(prefab);

                    // NOTE: Instantiateした直後にAwakeが実行される
                }

                return s_instance;
            }
        }

        protected virtual void Awake()
        {
            if (s_instance)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this as TBehaviour;

            // シーン遷移の際、インスタンスを破壊されないようにする
            DontDestroyOnLoad(s_instance);

            // 初期化する
            Initialize();
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        public virtual void Initialize() { }
    }
}
