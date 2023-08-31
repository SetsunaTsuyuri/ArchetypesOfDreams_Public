using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// Addressablesによるロード・解放を行うクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AddressableLoader<T>
    {
        AsyncOperationHandle<T> _handle = default;

        /// <summary>
        /// 非同期でロードする
        /// </summary>
        /// <param name="address"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<T> LoadAsync(string address, CancellationToken token)
        {
            _handle = Addressables.LoadAssetAsync<T>(address);

            T asset = await _handle.ToUniTask(cancellationToken: token);
            return asset;
        }

        /// <summary>
        /// ロードする
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public T Load(string address)
        {
            _handle = Addressables.LoadAssetAsync<T>(address);

            T asset = _handle.WaitForCompletion();
            return asset;
        }

        /// <summary>
        /// 解放する
        /// </summary>
        public void Release()
        {
            if (!_handle.IsValid())
            {
                return;
            }

            Addressables.Release(_handle.Result);
        }
    }
}
