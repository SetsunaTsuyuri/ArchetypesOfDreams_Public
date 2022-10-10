using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// シングルトンなMonoBehaviourプレハブをロードする者
    /// </summary>
    /// <typeparam name="TBehaviour">シングルトンの型</typeparam>
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
    /// <typeparam name="TBehaviour">シングルトンの型</typeparam>
    /// <typeparam name="TLoader">プレハブローダーの型</typeparam>
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
                if (s_instance is null)
                {
                    // プレハブをロードする
                    TLoader loader = new TLoader();
                    TBehaviour prefab = loader.LoadPrefab();

                    // ロードしたプレハブをインスタンス化し、クラス変数に代入する
                    s_instance = Instantiate(prefab);

                    // シーン遷移の際、インスタンスを破壊されないようにする
                    DontDestroyOnLoad(s_instance);

                    // インスタンスを初期化する
                    s_instance.Initialize();
                }

                return s_instance;
            }
        }

        public virtual void Initialize() { }
    }
}
