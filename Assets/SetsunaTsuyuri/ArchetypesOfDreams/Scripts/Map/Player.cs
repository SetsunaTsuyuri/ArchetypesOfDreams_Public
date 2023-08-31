using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MapObject
    {
        /// <summary>
        /// ミニマップのトランスフォーム
        /// </summary>
        [field: SerializeField]
        public Transform MiniMapTransform { get; private set; } = null;

        /// <summary>
        /// 移動する方向
        /// </summary>
        Vector2Int _moveDirection = Vector2Int.zero;

        /// <summary>
        /// 回転する方向
        /// </summary>
        Vector2Int _rotationDirection = Vector2Int.zero;

        /// <summary>
        /// 調査を求めている
        /// </summary>
        public bool WantsToCheck { get; private set; } = false;

        /// <summary>
        /// メニューを開きたい
        /// </summary>
        public bool WantsToOpenMenu { get; private set; } = false;

        /// <summary>
        /// 移動を求めている
        /// </summary>
        /// <returns></returns>
        public bool WantsToMove => _moveDirection != Vector2Int.zero;

        /// <summary>
        /// 回転を求めている
        /// </summary>
        /// <returns></returns>
        public bool WantsToRotate => _rotationDirection != Vector2Int.zero;

        /// <summary>
        /// 何かしらの動作を求めている
        /// </summary>
        public bool WantsToAnyAction
        {
            get
            {
                return WantsToCheck
                    || WantsToOpenMenu
                    || WantsToMove
                    || WantsToRotate;
            }
        }

        /// <summary>
        /// プレイヤーインプット
        /// </summary>
        PlayerInput _playerInput = null;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        /// <summary>
        /// マップイベントの取得を試みる
        /// </summary>
        /// <param name="map">マップ</param>
        /// <param name="trigger">マップイベントの起動条件</param>
        /// <param name="mapEventObject">マップイベントオブジェクト</param>
        /// <returns>イベントが起動可能ならtrueを返す</returns>
        public bool TryGetMapEventObject(Map map, MapEventTriggerType trigger, out MapEventObject mapEventObject)
        {
            // 同じセル
            mapEventObject = map.GetMapEventObject(Position, trigger);
            if (!mapEventObject)
            {
                // 目の前のセル
                mapEventObject = map.GetMapEventObject(Position + Direction, trigger, true);
            }

            return mapEventObject;
        }

        /// <summary>
        /// 移動先に存在するマップオブジェクトの取得を試みる
        /// </summary>
        /// <param name="map"></param>
        /// <param name="mapObject"></param>
        /// <returns></returns>
        public bool TryGetMapObjectAtDestination(Map map, out MapObject mapObject)
        {
            Vector2Int position = Position + GetDirection(_moveDirection);
            mapObject = map.GetMapObject(position);
            return mapObject;
        }

        /// <summary>
        /// 移動できる
        /// </summary>
        /// <param name="map">マップ</param>
        /// <returns></returns>
        public bool CanMoveRelative(Map map)
        {
            return CanMoveRelative(_moveDirection, map);
        }

        /// <summary>
        /// 移動する
        /// </summary>
        public void Move()
        {
            MoveRelative(_moveDirection);
        }

        /// <summary>
        /// 回転する
        /// </summary>
        public void Rotate()
        {
            Rotate(_rotationDirection);
        }

        /// <summary>
        /// 入力を有効にする
        /// </summary>
        public void EnableInput()
        {
            _playerInput.enabled = true;
        }

        /// <summary>
        /// 入力を無効にする
        /// </summary>
        public void DisableInput()
        {
            _playerInput.enabled = false;
        }

        /// <summary>
        /// 移動を入力されたときの処理
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    Vector2 input = context.ReadValue<Vector2>();
                    UpdateMoveOrRotationDirection(input);
                    break;

                case InputActionPhase.Canceled:
                    _moveDirection = Vector2Int.zero;
                    _rotationDirection = Vector2Int.zero;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 調査を入力されたときの処理
        /// </summary>
        /// <param name="context"></param>
        public void OnCheck(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    WantsToCheck = true;
                    break;

                case InputActionPhase.Canceled:
                    WantsToCheck = false;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// メニュー表示を入力したときの処理
        /// </summary>
        /// <param name="context"></param>
        public void OnMenuOpen(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    WantsToOpenMenu = true;
                    break;

                case InputActionPhase.Canceled:
                    WantsToOpenMenu = false;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 入力された値から移動または回転の方向を更新する
        /// </summary>
        /// <param name="input">入力</param>
        /// <returns></returns>
        private Vector2Int UpdateMoveOrRotationDirection(Vector2 input)
        {
            Vector2Int direction = Vector2Int.FloorToInt(input);

            if (direction.x != 0)
            {
                direction.y = 0;
                _rotationDirection = direction;
            }
            else
            {
                direction.y = -direction.y;
                _moveDirection = direction;
            }

            return direction;
        }
    }
}
