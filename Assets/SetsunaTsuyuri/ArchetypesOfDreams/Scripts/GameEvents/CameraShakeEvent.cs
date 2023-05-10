using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// カメラ振動イベント
    /// </summary>
    [System.Serializable]
    public class CameraShakeEvent : GameEventBase
    {
        /// <summary>
        /// 振動時間
        /// </summary>
        [field: SerializeField]
        public float Duration { get; set; } = 0.0f;

        public CameraShakeEvent(string[] columns)
        {
            Duration = ToFloat(columns, 1);
        }

        public override async UniTask Resolve(CancellationToken token)
        {
            CameraController cameraController = Camera.main.GetComponentInParent<CameraController>();
            if (!cameraController)
            {
                return;
            }

            cameraController.Shake(Duration);

            await UniTask.CompletedTask;
        }
    }
}
