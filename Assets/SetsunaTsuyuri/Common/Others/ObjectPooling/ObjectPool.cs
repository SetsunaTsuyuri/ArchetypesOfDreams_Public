using System;
using System.Linq;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// オブジェクトプール
    /// </summary>
    /// <typeparam name="T">オブジェクトの型</typeparam>
    public class ObjectPool<T>
        where T : class
    {
        /// <summary>
        /// コンテナ配列
        /// </summary>
        readonly PooledObjectContainer<T>[] _containers = null;

        /// <summary>
        /// オブジェクトが取得された際の処理
        /// </summary>
        readonly Action<T> _onGot = null;

        /// <summary>
        /// オブジェクトが解放された際の処理
        /// </summary>
        readonly Action<T> _onReleased = null;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="create">オブジェクトを作る関数</param>
        /// <param name="onGot">取得する際に呼ばれる関数</param>
        /// <param name="onReleased">解放する際に呼ばれる関数</param>
        /// <param name="poolCapatity">プールする数</param>
        public ObjectPool(Func<T> create, Action<T> onGot, Action<T> onReleased, int poolCapatity)
        {
            _containers = CreateObjects(create, poolCapatity);
            _onGot = onGot;
            _onReleased = onReleased;

            ReleaseAll();
        }

        /// <summary>
        /// オブジェクトを作る
        /// </summary>
        /// <param name="create">オブジェクトを作る関数</param>
        /// <param name="poolCapatity">プールする数</param>
        /// <returns></returns>
        private PooledObjectContainer<T>[] CreateObjects(Func<T> create, int poolCapatity)
        {
            PooledObjectContainer<T>[] containers = new PooledObjectContainer<T>[poolCapatity];
            for (int i = 0; i < poolCapatity; i++)
            {
                T pooledObject = create?.Invoke();
                PooledObjectContainer<T> container = new(pooledObject);
                containers[i] = container;
            }

            return containers;
        }

        /// <summary>
        /// オブジェクトを取得する
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            T pooledObject = null;
            
            PooledObjectContainer<T> container = _containers
                .FirstOrDefault(x => x.IsAvailable);

            if (container is not null)
            {
                pooledObject = container.Get(_onGot);
            }

            return pooledObject;
        }

        /// <summary>
        /// オブジェクトを解放する
        /// </summary>
        /// <param name="pooledObject">解放するオブジェクト</param>
        public void Release(T pooledObject)
        {
            PooledObjectContainer<T> container = _containers
                .FirstOrDefault(x => x.PooledObject == pooledObject);

            if (container is not null)
            {
                container.Release(_onReleased);
            }
        }

        /// <summary>
        /// 全てのオブジェクトを解放する
        /// </summary>
        public void ReleaseAll()
        {
            foreach (var container in _containers)
            {
                container.Release(_onReleased);
            }
        }
    }
}
