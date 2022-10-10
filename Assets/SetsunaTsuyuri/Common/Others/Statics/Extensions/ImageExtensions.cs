using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// Imageの拡張メソッド集
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// RGB値を変える
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="color">色</param>
        public static void ChangeRGB(this Image image, Color color)
        {
            image.ChangeRGB(color.r, color.g, color.b);
        }

        /// <summary>
        /// RGB値を変える
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        public static void ChangeRGB(this Image image, float r, float g, float b)
        {
            Color newColor = image.color;
            newColor.r = r;
            newColor.g = g;
            newColor.b = b;
            
            image.color = newColor;
        }

        /// <summary>
        /// アルファ値(不透明度)を変える
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="alpha">不透明度</param>
        /// <returns></returns>
        public static void ChangeAlpha(this Image image, float alpha)
        {
            Color newColor = image.color;
            newColor.a = alpha;

            image.color = newColor;
        }
    }
}
