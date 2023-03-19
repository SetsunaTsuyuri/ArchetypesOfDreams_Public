using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 音量バー
    /// </summary>
    public abstract class VolumeBar : MonoBehaviour
    {
        private void Awake()
        {
            Image image = GetComponent<Image>();

            GetAudioVolumeObervable()
                .TakeUntilDestroy(gameObject)
                .Subscribe(volume => image.fillAmount = volume);
        }

        /// <summary>
        /// 音量のIObservableを取得する
        /// </summary>
        /// <returns></returns>
        protected abstract System.IObservable<float> GetAudioVolumeObervable();
    }
}
