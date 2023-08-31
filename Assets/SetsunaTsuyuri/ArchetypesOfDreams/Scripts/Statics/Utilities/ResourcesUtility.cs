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
