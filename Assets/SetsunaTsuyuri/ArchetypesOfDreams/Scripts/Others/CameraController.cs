using System.Collections;
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
        /// 直前の視点となるトランスフォーム
        /// </summary>
        Transform _previousTarget = null;

        /// <summary>
        /// カメラ
        /// </summary>
        public Camera Camera { get; private set; } = null;

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

        private void Awake()
        {
            Camera = GetComponent<Camera>();
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
            if (!Target)
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
    }
}
