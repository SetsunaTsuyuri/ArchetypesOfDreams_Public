using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// GameButtonの設定
    /// </summary>
    [CreateAssetMenu(fileName = nameof(GameButtonsSettings), menuName = "GameSettings/Buttons")]
    public class GameButtonsSettings : SingletonScriptableObject<GameButtonsSettings>
    {
        /// <summary>
        /// Intaractableな場合のアルファ値
        /// </summary>
        [SerializeField]
        float _interactableAlpha = 1.0f;

        /// <summary>
        /// Intaractableな場合のアルファ値
        /// </summary>
        public static float InteractableAlpha => Instance._interactableAlpha;

        /// <summary>
        /// Intaractableでない場合のアルファ値
        /// </summary>
        [SerializeField]
        float _nonInteractableAlpha = 0.5f;

        /// <summary>
        /// Intaractableでない場合のアルファ値
        /// </summary>
        public static float NonInteractableAlpha => Instance._nonInteractableAlpha;
    }
}
