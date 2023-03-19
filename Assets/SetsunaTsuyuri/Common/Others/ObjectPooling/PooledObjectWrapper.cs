using System;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// プールされるオブジェクトのラッパークラス
    /// </summary>
    /// <typeparam name="TPooledObject">オブジェクトの型</typeparam>
    public class PooledObjectWrapper<TPooledObject> where TPooledObject : class
    {
        /// <summary>
        /// 利用可能である
        /// </summary>
        public bool IsAvailable { get; private set; } = true;

        /// <summary>
        /// プールされるオブジェクト
        /// </summary>
        public TPooledObject PooledObject { get; private set; } = null;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="pooledObject">プールされるオブジェクト</param>
        public PooledObjectWrapper(TPooledObject pooledObject)
        {
            PooledObject = pooledObject;
        }

        /// <summary>
        /// オブジェクトを取得する
        /// </summary>
        /// <param name="onGot"></param>
        public TPooledObject Get(Action<TPooledObject> onGot)
        {
            IsAvailable = false;
            onGot?.Invoke(PooledObject);
            return PooledObject;
        }

        /// <summary>
        /// オブジェクトを解放する
        /// </summary>
        /// <param name="onReleased"></param>
        public void Release(Action<TPooledObject> onReleased)
        {
            IsAvailable = true;
            onReleased?.Invoke(PooledObject);
        }
    }
}
