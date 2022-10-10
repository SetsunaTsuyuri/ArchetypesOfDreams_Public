using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マップ上のオブジェクト
    /// </summary>
    public class MapObject : MonoBehaviour
    {
        /// <summary>
        /// マップ上の位置
        /// </summary>
        [field: SerializeField]
        public Vector2Int Position { get; set; } = Vector2Int.zero;

        /// <summary>
        /// マップ上の向き
        /// </summary>
        [field: SerializeField]
        public Vector2Int Direction { get; set; } = Vector2Int.up;

        /// <summary>
        /// 他のマップオブジェクトと衝突する
        /// </summary>
        [field: SerializeField]
        public bool Collides { get; set; } = false;

        /// <summary>
        /// 進入可能なセルの種類配列
        /// </summary>
        [field: SerializeField]
        public MapCellType[] AccessibleCells { get; set; } = { };

        /// <summary>
        /// 指定した位置に移動できる
        /// </summary>
        /// <param name="direction">移動方向</param>
        /// <param name="map">マップ</param>
        /// <returns></returns>
        public bool CanMove(Vector2Int direction, Map map)
        {
            bool result = false;

            // 移動先の位置
            Vector2Int position = Position + GetDirection(direction);
            
            // 移動先のセル
            MapCell cell = map.GetCell(position);
            
            // 移動先のオブジェクト
            MapObject other = map.GetMapEventObject(position);

            // 移動先に進入可能なセルがあり、
            // オブジェクトが存在しないか衝突しない場合に移動できる
            if (cell is not null
                && cell.IsAccessible(this)
                && (!other || (other && !other.Collides)))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 移動する
        /// </summary>
        /// <param name="move">移動方向</param>
        public void Move(Vector2Int move)
        {
            Vector2Int direction = GetDirection(move);
            Position += direction;
        }

        /// <summary>
        /// 回転する
        /// </summary>
        /// <param name="rotation">回転方向</param>
        public void Rotate(Vector2Int rotation)
        {
            Direction = GetDirection(rotation);
        }

        /// <summary>
        /// 移動のUniTaskを作る
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UniTask CreateTransformMoveUniTask(CancellationToken token)
        {
            Vector3 position = PositionToTransformPosition();

            UniTask move = transform.DOLocalMove(position, GameSettings.Maps.ObjectsMoveDuration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject)
                .ToUniTask(cancellationToken: token);

            return move;
        }

        /// <summary>
        /// 回転のUniTaskを作る
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UniTask CreateTransformRotationUniTask(CancellationToken token)
        {
            Quaternion quaternion = DirectionToTransformRotation();

            float duration = GameSettings.Maps.ObjectsRotationDuration;
            UniTask rotation = transform.DOLocalRotateQuaternion(quaternion, duration)
                .SetEase(Ease.OutQuad)
                .SetLink(gameObject)
                .ToUniTask(cancellationToken: token);

            return rotation;
        }

        /// <summary>
        /// トランスフォームを更新する
        /// </summary>
        public void UpdateTransform()
        {
            transform.localPosition = PositionToTransformPosition();
            transform.localRotation = DirectionToTransformRotation();
        }

        /// <summary>
        /// 位置からトランスフォームの位置を求める
        /// </summary>
        /// <returns></returns>
        protected Vector3 PositionToTransformPosition()
        {
            Vector3 position = new(Position.x, 0.0f, -Position.y);
            return position;
        }

        /// <summary>
        /// 方向からトランスフォームの回転の値を求める
        /// </summary>
        /// <returns></returns>
        protected Quaternion DirectionToTransformRotation()
        {
            Vector3 forward = new Vector3(Direction.x, 0.0f, -Direction.y).normalized;
            Quaternion quaternion = Quaternion.LookRotation(forward, Vector3.up);
            return quaternion;
        }

        /// <summary>
        /// 現在の向きを正面として方向を取得する
        /// </summary>
        /// <param name="move">移動方向</param>
        /// <returns></returns>
        protected Vector2Int GetDirection(Vector2Int move)
        {
            Vector2Int direction = move;

            if (Direction.y > 0)
            {
                direction = -direction;
            }
            else if (Direction.x != 0)
            {
                int x = direction.y;
                int y = direction.x;
                direction.x = x;
                direction.y = y;

                if (Direction.x > 0 && move.y != 0)
                {
                    direction = -direction;
                }
                else if (Direction.x < 0 && move.x != 0)
                {
                    direction = -direction;
                }
            }

            return direction;
        }

        /// <summary>
        /// 前方の位置を取得する
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetForwardPosition()
        {
            return Position + Direction;
        }

        /// <summary>
        /// 指定したマップオブジェクトに重なっている
        /// </summary>
        /// <param name="mapObject">マップオブジェクト</param>
        /// <returns></returns>
        public bool Overlaps(MapObject mapObject)
        {
            return Position == mapObject.Position;
        }
    }
}
