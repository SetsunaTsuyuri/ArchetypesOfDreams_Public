using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョン
    /// </summary>
    public partial class Dungeon : MonoBehaviour
    {
        /// <summary>
        /// プレイヤーの入力後発生するイベントタイプ
        /// </summary>
        public enum NextEventType
        {
            None,
            Move,
            Rotation,
            Event,
        }

        /// <summary>
        /// ダンジョンUI
        /// </summary>
        [SerializeField]
        DungeonSceneUIManager _ui = null;

        /// <summary>
        /// プレイヤー
        /// </summary>
        [SerializeField]
        PlayerMenu _playerMenu = null;

        /// <summary>
        /// 戦闘の管理者
        /// </summary>
        [SerializeField]
        Battle _battle = null;

        /// <summary>
        /// 味方コンテナの管理者
        /// </summary>
        [SerializeField]
        AlliesParty _allies = null;

        /// <summary>
        /// カメラ
        /// </summary>
        [SerializeField]
        CameraController _cameraController = null;

        /// <summary>
        /// ミニマップカメラ
        /// </summary>
        [SerializeField]
        CameraController _miniMapCamera = null;

        /// <summary>
        /// ダンジョンデータ
        /// </summary>
        public DungeonData Data { get; private set; } = null;

        /// <summary>
        /// マップ
        /// </summary>
        public Map Map { get; private set; } = null;

        /// <summary>
        /// プレイヤー
        /// </summary>
        public Player Player => Map.Player;

        /// <summary>
        /// 起動するマップイベント
        /// </summary>
        MapEventObject _reservedMapEventObject = null;

        /// <summary>
        /// 敵遭遇値
        /// </summary>
        int _enemiesEncounterValue = 0;

        /// <summary>
        /// 敵遭遇値の増加
        /// </summary>
        [SerializeField]
        int _enemiesEncounterValueIncrease = 0;

        /// <summary>
        /// 敵遭遇値がこの値以上になると戦闘が発生する
        /// </summary>
        [SerializeField]
        int _enemiesEncounterThreshold = 100;

        /// <summary>
        /// イベント解決を許可する
        /// </summary>
        bool _allowsEventsResolution = true;

        /// <summary>
        /// ランダムエンカウント解決を許可する
        /// </summary>
        bool _allowsRandomEncounterResolution = false;

        /// <summary>
        /// 次に処理するイベントタイプ
        /// </summary>
        NextEventType _nextEventType = NextEventType.None;

        /// <summary>
        /// ダンジョンTextAsset
        /// </summary>
        readonly AddressableLoader<TextAsset> _loader = new();

        /// <summary>
        /// 現在のランダムエンカウントエリア
        /// </summary>
        public int CurrentRandomEncounterArea
        {
            get => Map.RandomEncounterAreas[Map.Player.Position.y, Map.Player.Position.x];
        }

        /// <summary>
        /// 現在のプレイヤー位置でランダムエンカウントする敵が1体でも存在するか
        /// </summary>
        public bool ExistsAnyRandomEncounterEnemyAtCurrentPlayerPosition
        {
            get => Data.Enemies.Any(x => x.AreaId == CurrentRandomEncounterArea);
        }

        /// <summary>
        /// ランダムエンカウント敵データ配列を取得する
        /// </summary>
        /// <returns></returns>
        public RandomEncounterEnemyData[] GetRandomEncounterEnemies()
        {
            return Data.Enemies
                .Where(x => x.AreaId == CurrentRandomEncounterArea)
                .ToArray();
        }

        private void Awake()
        {
            Map = GetComponentInChildren<Map>(true);
        }

        private void Start()
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            StartAsync(token).Forget();
        }

        private void OnDestroy()
        {
            _loader.Release();
        }

        private async UniTask StartAsync(CancellationToken token)
        {
            await SetUp(token);
            await FadeManager.FadeIn(token);
            await MainLoop(token);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        private async UniTask SetUp(CancellationToken token)
        {
            _ui.SetUp(Player, _allies, _battle);

            _allies.SetUp(_battle.Enemies);
            _battle.Enemies.SetUp(_allies);

            // 戦闘開始
            MessageBrokersManager.BattleStart
                .Receive<Battle>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ =>
                {
                    Map.gameObject.SetActive(false);
                    _ui.Main.SetEnabled(false);
                });

            // 戦闘終了
            MessageBrokersManager.BattleEnd
                .Receive<Battle>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ =>
                {
                    Map.gameObject.SetActive(true);
                    _ui.Main.SetEnabled(true);
                });

            // データを設定する
            int id = VariableData.DungeonId;
            Data = MasterData.GetDungeonData(id);

            // 選択可能なダンジョンであれば選択画面で選べるようにする
            if (!Data.CannotBeSelected)
            {
                VariableData.Dungeons.TrySetValue(id, true);
            }

            // 戦闘者を味方コンテナへ移す
            _allies.TransferCombatantsViriableDataToContainers();

            // jsonからマップを作る
            TextAsset textAsset = await _loader.LoadAsync(Data.Map, token);
            string json = textAsset.text;
            Map.SetUp(json);

            // プレイヤーを開始位置に置く
            (Vector2Int position, Vector2Int direction) = Map.GetPlayerStartPositionAndDirection();
            Player.Position = position;
            Player.Direction = direction;
            Player.UpdateTransform();

            // カメラの視点をプレイヤーにする
            _cameraController.Target = Player.transform;
            _miniMapCamera.Target = Player.MiniMapTransform;

            // 説明文UIを非表示にする
            _ui.Description.SetEnabled(false);

            // BGM再生
            AudioManager.PlayBgm(Data.BgmId);
        }

        /// <summary>
        /// メインループ処理
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask MainLoop(CancellationToken token)
        {
            while (true)
            {
                // イベント
                await ResolveEvents(token);

                // ランダムエンカウント
                await ResolveRandomEncounter(token);

                // プレイヤー操作
                await ResolvePlayerControll(token);
                
                switch (_nextEventType)
                {
                    // 移動
                    case NextEventType.Move:
                        await ResolveMove(token);
                        break;

                    // 回転
                    case NextEventType.Rotation:
                        await ResolvePlayerRotation(token);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// イベント
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask ResolveEvents(CancellationToken token)
        {
            if (!_allowsEventsResolution)
            {
                return;
            }

            while (true)
            {
                // 直前のプレイヤー位置
                Vector2Int previousPlayerPosition = Player.Position;

                // 予約イベントがあればそれを優先する
                if (_reservedMapEventObject)
                {
                    await _reservedMapEventObject.ResolveEvents(token);
                    _reservedMapEventObject = null;

                    // プレイヤーの位置が変わっていなければ終了
                    if (Player.Position == previousPlayerPosition)
                    {
                        break;
                    }
                }
                // プレイヤー接触によるイベント起動
                else if (Map.TryGetMapEventObject(Player.Position, MapEventTriggerType.Touch, out MapEventObject mapEventObject))
                {
                    if (_ui.Main.IsEnabled)
                    {
                        _ui.Main.FadeOut();
                    }

                    // イベント解決
                    await mapEventObject.ResolveEvents(token);

                    // プレイヤーの位置が変わっていなければ終了
                    if (Player.Position == previousPlayerPosition)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// ランダムエンカウント
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask ResolveRandomEncounter(CancellationToken token)
        {
            // ランダムエンカウントが許可されていなければ終了
            if (!_allowsRandomEncounterResolution)
            {
                return;
            }

            // 敵が存在しなければ終了
            if (!ExistsAnyRandomEncounterEnemyAtCurrentPlayerPosition)
            {
                return;
            }

            // 敵遭遇値加算
            _enemiesEncounterValue += _enemiesEncounterValueIncrease;
            
            // 敵遭遇値が閾値に達していなければ終了
            if (_enemiesEncounterValue < _enemiesEncounterThreshold)
            {
                return;
            }

            // 敵遭遇値リセット
            _enemiesEncounterValue = 0;

            // BGM保存
            AudioManager.SaveBgm();
            AudioManager.StopBgm();

            // 戦闘を行う
            BattleResultType result = await _battle.ExecuteRandomEncounterBattle(this, token);

            // 負けた場合
            if (result == BattleResultType.Lose)
            {
                // マイルームに戻る
                SceneChangeManager.StartChange(SceneId.MyRoom);
            }
            else
            {
                // カメラをプレイヤー視点にする
                _cameraController.Target = Player.transform;

                _ui.Description.SetEnabled(false);

                // BGMを再生する
                AudioManager.LoadBgm(1.0f);

                // フェードイン
                await FadeManager.FadeIn(token);

                // ランダムエンカウント不許可
                _allowsRandomEncounterResolution = false;
            }
        }

        /// <summary>
        /// プレイヤー操作
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask ResolvePlayerControll(CancellationToken token)
        {
            // メインUI表示
            if (!_ui.Main.IsEnabled)
            {
                _ui.Main.FadeIn();
            }

            _nextEventType = NextEventType.None;
            await UniTask.Yield(token);

            // 操作有効化
            Player.EnableInput();
            while (true)
            {
                // 調べる
                if (Player.WantsToCheck)
                {
                    OnCheck();
                }
                // メニュー
                else if (Player.WantsToOpenMenu)
                {
                    OnMenu();
                }
                // 移動
                else if (Player.WantsToMove)
                {
                    OnMove();
                }
                // 回転
                else if (Player.WantsToRotate)
                {
                    _nextEventType = NextEventType.Rotation;
                }

                if (_nextEventType != NextEventType.None)
                {
                    break;
                }

                await UniTask.Yield(token);
            }
        }

        /// <summary>
        /// 調査
        /// </summary>
        private void OnCheck()
        {
            if (Player.TryGetMapEventObject(Map, MapEventTriggerType.Check, out MapEventObject mapEvent))
            {
                // 起動するイベントを予約する
                _reservedMapEventObject = mapEvent;
                _nextEventType = NextEventType.Event;
                _allowsEventsResolution = true;
            }
        }

        /// <summary>
        /// メニュー
        /// </summary>
        private void OnMenu()
        {
            AudioManager.PlaySE(SEId.Select);

            _ui.Main.SetEnabled(false);
            Player.DisableInput();
            _playerMenu.BeSelected();
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void OnMove()
        {
            if (Player.CanMoveRelative(Map))
            {
                _nextEventType = NextEventType.Move;
            }
            else if (Player.TryGetMapObjectAtDestination(Map, out MapObject mapObject))
            {
                if (mapObject is MapEventObject mapEventObject
                    && mapEventObject.Trigger == MapEventTriggerType.Touch)
                {
                    _reservedMapEventObject = mapEventObject;
                    _nextEventType = NextEventType.Event;
                    _allowsEventsResolution = true;
                }
            }
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask ResolveMove(CancellationToken token)
        {
            // プレイヤー移動
            Player.Move();
            Player.DisableInput();

            List<UniTask> moveList = new();

            // イベント移動
            foreach (var mapEventObject in Map.Events)
            {
                // 目的地更新
                mapEventObject.UpdateDestination();

                // 移動方向決定
                Vector2Int direction = mapEventObject.DecideNextMoveDirection();
                if (direction == Vector2Int.zero)
                {
                    continue;
                }

                // 方向更新
                mapEventObject.Direction = direction;
                mapEventObject.UpdateTransformRotation();

                // 予約イベントが存在せず、自身がタッチ起動し、移動先にプレイヤーがいる
                if (!_reservedMapEventObject
                    && mapEventObject.Trigger == MapEventTriggerType.Touch
                    && mapEventObject.Position + direction == Map.Player.Position)
                {
                    // 自身を予約
                    _reservedMapEventObject = mapEventObject;
                }
                else if (mapEventObject.CanMove(direction, Map))
                {
                    mapEventObject.Move(direction);
                    UniTask moveTask = mapEventObject.CreateTransformMoveUniTask(token);
                    moveList.Add(moveTask);
                }
            }

            // トランスフォーム移動
            UniTask move = Player.CreateTransformMoveUniTask(token);

            await UniTask.WhenAll(move);

            // 歩いたときの処理
            _allies.OnWalk();

            // イベント許可
            _allowsEventsResolution = true;

            // ランダムエンカウント許可
            _allowsRandomEncounterResolution = !_reservedMapEventObject;
        }

        /// <summary>
        /// プレイヤー回転
        /// </summary>
        private async UniTask ResolvePlayerRotation(CancellationToken token)
        {
            // プレイヤー回転
            Player.Rotate();
            Player.DisableInput();

            // トランスフォーム回転
            UniTask rotation = Player.CreateTransformRotationUniTask(token);
            await rotation;

            // イベント・ランダムエンカウント不許可
            _allowsEventsResolution = false;
            _allowsRandomEncounterResolution = false;
        }
    }
}
