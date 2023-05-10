using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// カメラコントローラー
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// 回転を固定する
        /// </summary>
        [SerializeField]
        bool _freezeRotation = false;

        /// <summary>
        /// 視点となるトランスフォーム
        /// </summary>
        Transform _target = null;

        /// <summary>
        /// 視点となるトランスフォーム
        /// </summary>
        public Transform Target
        {
            get => _target;
            set
            {
                _previousTarget = _target;
                _target = value;
                UpdatePositionAndRotation();
            }
        }

        /// <summary>
        /// 直前の視点となるトランスフォーム
        /// </summary>
        Transform _previousTarget = null;

        /// <summary>
        /// カメラ
        /// </summary>
        public Camera Camera { get; private set; } = null;

        /// <summary>
        /// 位置を更新する
        /// </summary>
        public bool CanUpdatePosition { get; set; } = true;

        /// <summary>
        /// カメラシェイクTweener
        /// </summary>
        Tween _shakeTween = null;

        private void Awake()
        {
            Camera = GetComponentInChildren<Camera>();
        }

        private void LateUpdate()
        {
            UpdatePositionAndRotation();
        }

        /// <summary>
        /// 位置と回転を更新する
        /// </summary>
        private void UpdatePositionAndRotation()
        {
            if (!CanUpdatePosition || !Target)
            {
                return;
            }

            if (_freezeRotation)
            {
                transform.position = Target.position;
            }
            else
            {
                transform.SetPositionAndRotation(Target.position, Target.rotation);
            }

        }

        /// <summary>
        /// 直前の視点となるトランスフォームに戻す
        /// </summary>
        public void ToPrevious()
        {
            if (!_previousTarget)
            {
                return;
            }

            Target = _previousTarget;
        }

        /// <summary>
        /// カメラを振動させる
        /// </summary>
        /// <param name="duration"></param>
        public void Shake(float duration)
        {
            float strength = GameSettings.Cameras.ShakeStrength;
            int vibrato = GameSettings.Cameras.ShakeVibrato;
            Shake(duration, strength, vibrato);
        }

        /// <summary>
        /// カメラを振動させる
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="strength"></param>
        /// <param name="vibrato"></param>
        public void Shake(float duration, float strength, int vibrato)
        {
            if (_shakeTween.IsActive())
            {
                _shakeTween.Kill(true);
            }

            Vector3 startPosition = Camera.transform.localPosition;

            _shakeTween = Camera
                .DOShakePosition(duration, strength, vibrato)
                .SetLink(Camera.gameObject)
                .OnComplete(() => Camera.transform.localPosition = startPosition);
        }
    }
}
