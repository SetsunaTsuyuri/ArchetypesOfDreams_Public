using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// リソースのユーティリティクラス
    /// </summary>
    public static class ResourcesUtility
    { 
        /// <summary>
        /// スプライトフォルダ
        /// </summary>
        static readonly string s_sprites = "Sprites";

        /// <summary>
        /// ゲームイベントフォルダ
        /// </summary>
        static readonly string s_gameEvents = "GameEvents";

        /// <summary>
        /// スプライトをロードする
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Sprite[] LoadSprites(string name)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>($"{s_sprites}/{name}");
            return sprites;
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
