using System.Collections.Generic;
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
        /// スプライトをロードする
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Sprite[] LoadSprites(string name)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>($"{s_sprites}/{name}");
            return sprites;
        }
    }
}
