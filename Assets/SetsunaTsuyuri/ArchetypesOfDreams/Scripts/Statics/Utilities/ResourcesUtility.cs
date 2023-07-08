using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// リソースのユーティリティクラス
    /// </summary>
    public static class ResourcesUtility
    {
        /// <summary>
        /// ゲームイベントフォルダ
        /// </summary>
        static readonly string s_gameEvents = "GameEvents";

        /// <summary>
        /// 戦闘者スプライトをロードする
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AsyncOperationHandle<Sprite[]> LoadCombatantSprites(string name)
        {
            return Load<Sprite[]>(name);
        }

        /// <summary>
        /// 戦闘者スプライトを解放する
        /// </summary>
        /// <param name="handle"></param>
        public static void ReleaseCombatantSprites(AsyncOperationHandle<Sprite[]> handle)
        {
            Release(handle);
        }

        /// <summary>
        /// ロードする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <returns></returns>
        private static AsyncOperationHandle<T> Load<T>(string address)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            return handle;
        }

        /// <summary>
        /// 解放する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        private static void Release<T>(AsyncOperationHandle<T> handle)
        {
            Addressables.Release(handle);
        }

        /// <summary>
        /// ゲームイベントをロードする
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> LoadGameEvents()
        {
            TextAsset[] gameEventAssets = Resources.LoadAll<TextAsset>($"{s_gameEvents}");

            Dictionary<string, string> gameEventDictionary = gameEventAssets
                .ToDictionary(x => x.name, x => x.text);
            return gameEventDictionary;
        }
    }
}
