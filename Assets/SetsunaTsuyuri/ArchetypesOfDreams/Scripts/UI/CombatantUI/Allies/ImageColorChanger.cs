using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// Imageの色を変化させるもの
    /// </summary>
    public class ImageColorChanger : MonoBehaviour
    {
        Image image = null;

        Color defaultColor = Color.white;

        private void Awake()
        {
            image = GetComponent<Image>();
            defaultColor = image.color;
        }

        /// <summary>
        /// 対象フラグが設定されたときの処理
        /// </summary>
        /// <param name="targeted">対象になった</param>
        public void OnTargetFlagSet(bool targeted)
        {
            if (targeted)
            {
                ToTargetedColor();
            }
            else
            {
                ToDefaultColor();
            }
        }

        /// <summary>
        /// 元の色に戻す
        /// </summary>
        public void ToDefaultColor()
        {
            image.color = defaultColor;
        }

        /// <summary>
        /// 対象選択時の色にする
        /// </summary>
        private void ToTargetedColor()
        {
            image.color = GameSettings.VisualEffects.TargetedAllyUIColor;
        }
    }
}
