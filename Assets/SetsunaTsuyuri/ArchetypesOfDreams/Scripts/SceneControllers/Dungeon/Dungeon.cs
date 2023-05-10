using System.Collections;
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
        /// プレイヤー
        /// </summary>
        [SerializeField]
        Player _player = null;

        /// <summary>
        /// ダンジョンデータ
        /// </summary>
        DungeonData _data = null;

        /// <summary>
        /// マップ
        /// </summary>
        public Map Map { get; private set; } = null;

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
        /// ランダムエンカウントを許可する
        /// </summary>
        bool _allowRandomEncounter = false;

        /// <summary>
        /// 敵遭遇値がこの値以上になると戦闘が発生する
        /// </summary>
        [SerializeField]
        int _enemiesEncounterThreshold = 100;

        /// <summary>
        /// ステートマシン
        /// </summary>
        public UniTaskStateMachine<Dungeon> State { get; private set; } = null;

        private void Awake()
        {
            State = new(this);
            State.Add<MapEventsResolution>();
            State.Add<RandomEncounter>();
            State.Add<PlayerControl>();
            State.Add<ReservedMapEventsResolution>();
            State.Add<PlayerTransformRotation>();
            State.Add<MapObjectsTransformMove>();

            Map = GetComponentInChildren<Map>(true);
        }

        private void Start()
        {
            SetUp();

            CancellationToken token = this.GetCancellationTokenOnDestroy();
            StartAsync(token).Forget();
        }

        private async UniTask StartAsync(CancellationToken token)
        {
            await FadeManager.FadeIn(token);
            State.StartChange<MapEventsResolution>(this);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        private void SetUp()
        {
            _ui.SetUp(_player, _allies, _battle);

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
            _data = MasterData.GetDungeonData(id);

            // 選択可能なダンジョンであれば選択画面で選べるようにする
            if (!_data.CannotBeSelected)
            {
                VariableData.SelectableDungeons[id] = true;
            }

            // 戦闘者を味方コンテナへ移す
            _allies.TransferCombatantsViriableDataToContainers();

            // jsonからマップを作る
            string json = _data.MapJson.text;
            Map.SetUp(json);

            // プレイヤーを開始位置に置く
            (Vector2Int position, Vector2Int direction) = Map.GetPlayerStartPositionAndDirection();
            _player.Position = position;
            _player.Direction = direction;
            _player.UpdateTransform();

            // カメラの視点をプレイヤーにする
            _cameraController.Target = _player.transform;
            _miniMapCamera.Target = _player.MiniMapTransform;

            // 説明文UIを非表示にする
            _ui.Description.SetEnabled(false);

            // BGM再生
            AudioManager.PlayBgm(BgmId.Dungeon);
        }

        private void Update()
        {
            State.Update();
        }

        /// <summary>
        /// マップイベント解決
        /// </summary>
        public class MapEventsResolution : UniTaskStateMachine<Dungeon>.State
        {
            public override async UniTask EnterAsync(Dungeon context, CancellationToken token)
            {
                Map map = context.Map;
                Player player = context._player;

                Vector2Int previousPlayerPosition = player.Position;

                if (map.TryGetMapEventObject(player.Position, MapEventTriggerType.Touch, out MapEventObject mapEventObject))
                {
                    if (context._ui.Main.IsEnabled)
                    {
                        context._ui.Main.FadeOut();
                    }

                    await mapEventObject.ResolveEvents(token);

                    // イベントにより移動した場合、移動先でもイベントが発生する可能性があるためやり直す
                    if (player.Position != previousPlayerPosition)
                    {
                        context.State.SetNextState<MapEventsResolution>();
                    }
                    else
                    {
                        context.State.SetNextState<PlayerControl>();
                    }
                }
                else if (context._allowRandomEncounter)
                {
                    context.State.SetNextState<RandomEncounter>();
                }
                else
                {
                    context.State.SetNextState<PlayerControl>();
                }
            }
        }

        /// <summary>
        /// プレイヤー操作
        /// </summary>
        public class PlayerControl : UniTaskStateMachine<Dungeon>.State
        {
            public override void Enter(Dungeon context)
            {
                // プレイヤーの操作を有効にする
                context._player.EnableInput();

                // ランダムエンカウントを許可する
                context._allowRandomEncounter = true;

                // メインUI表示
                if (!context._ui.Main.IsEnabled)
                {
                    context._ui.Main.FadeIn();
                }
            }

            public override void Update(Dungeon context)
            {
                Player player = context._player;

                // メニュー
                if (player.WantsToOpenMenu)
                {
                    OnMenu(context);
                }
                // 移動
                else if (player.WantsToMove())
                {
                    OnMove(context);
                }
                // 回転
                else if (player.WantsToRotate())
                {
                    OnRotation(context);
                }
                // 調べる
                else if (player.WantsToCheck)
                {
                    OnCheck(context);
                }
            }

            public override void Exit(Dungeon context)
            {
                // プレイヤーの操作を無効にする
                context._player.DisableInput();
            }

            /// <summary>
            /// メニュー
            /// </summary>
            /// <param name="context"></param>
            private void OnMenu(Dungeon context)
            {
                AudioManager.PlaySE(SEId.Select);

                context._ui.Main.SetEnabled(false);
                context._player.DisableInput();
                context._playerMenu.BeSelected();
            }

            /// <summary>
            /// 移動
            /// </summary>
            /// <param name="context"></param>
            private void OnMove(Dungeon context)
            {
                Player player = context._player;
                Map map = context.Map;

                if (player.CanMove(map))
                {
                    // 移動する
                    player.Move();

                    // 歩いたときの処理
                    context._allies.OnWalk();

                    // マップオブジェクトのトランスフォーム移動を開始する
                    context.State.StartChange<MapObjectsTransformMove>(context);
                }
                else if (player.TryGetMapEventObject(map, MapEventTriggerType.Touch, out MapEventObject mapEventObject))
                {
                    // 起動するイベントを予約する
                    context._reservedMapEventObject = mapEventObject;

                    // マップイベントの処理を開始する
                    context.State.StartChange<ReservedMapEventsResolution>(context);
                }
            }

            /// <summary>
            /// 回転
            /// </summary>
            /// <param name="context"></param>
            private void OnRotation(Dungeon context)
            {
                // 回転する
                context._player.Rotate();

                // プレイヤーのトランスフォーム回転を開始する
                context.State.StartChange<PlayerTransformRotation>(context);
            }

            /// <summary>
            /// 調査
            /// </summary>
            /// <param name="context"></param>
            private void OnCheck(Dungeon context)
            {
                Player player = context._player;
                Map map = context.Map;

                if (player.TryGetMapEventObject(map, MapEventTriggerType.Check, out MapEventObject mapEvent))
                {
                    // 起動するイベントを予約する
                    context._reservedMapEventObject = mapEvent;

                    // マップイベントの処理を開始する
                    context.State.StartChange<ReservedMapEventsResolution>(context);
                }
            }
        }

        /// <summary>
        /// 予約済みマップイベント解決
        /// </summary>
        public class ReservedMapEventsResolution : UniTaskStateMachine<Dungeon>.State
        {
            public override async UniTask EnterAsync(Dungeon context, CancellationToken token)
            {
                MapEventObject mapEventObject = context._reservedMapEventObject;
                if (mapEventObject)
                {
                    if (context._ui.Main.IsEnabled)
                    {
                        context._ui.Main.FadeOut();
                    }

                    Vector2Int previousPlayerPosition = context._player.Position;

                    await mapEventObject.ResolveEvents(token);
                    context._reservedMapEventObject = null;

                    if (context._player.Position != previousPlayerPosition)
                    {
                        // マップイベントを解決する
                        context.State.SetNextState<MapEventsResolution>();
                    }
                    else
                    {
                        // プレイヤー操作を開始する
                        context.State.SetNextState<PlayerControl>();
                    }
                }
                else
                {
                    // プレイヤー操作を開始する
                    context.State.SetNextState<PlayerControl>();
                }
            }
        }

        /// <summary>
        /// プレイヤーのトランスフォーム回転
        /// </summary>
        public class PlayerTransformRotation : UniTaskStateMachine<Dungeon>.State
        {
            public override async UniTask EnterAsync(Dungeon context, CancellationToken token)
            {
                // プレイヤーの回転
                UniTask rotation = context._player.CreateTransformRotationUniTask(token);
                await rotation;

                // プレイヤー操作を開始する
                context.State.SetNextState<PlayerControl>();
            }
        }

        /// <summary>
        /// マップオブジェクトのトランスフォーム移動
        /// </summary>
        public class MapObjectsTransformMove : UniTaskStateMachine<Dungeon>.State
        {
            public override async UniTask EnterAsync(Dungeon context, CancellationToken token)
            {
                // プレイヤーの移動
                UniTask move = context._player.CreateTransformMoveUniTask(token);
                await move;

                // TODO: その他マップオブジェクトの移動

                context.State.SetNextState<MapEventsResolution>();
            }
        }

        /// <summary>
        /// ランダムエンカウント
        /// </summary>
        public class RandomEncounter : UniTaskStateMachine<Dungeon>.State
        {
            public override async UniTask EnterAsync(Dungeon context, CancellationToken token)
            {
                await base.EnterAsync(context, token);

                if (!context.Map.EnablesRandomEncounterBattle)
                {
                    context.State.SetNextState<PlayerControl>();
                    return;
                }

                context._enemiesEncounterValue += context._enemiesEncounterValueIncrease;
                if (context._enemiesEncounterValue < context._enemiesEncounterThreshold)
                {
                    context.State.SetNextState<PlayerControl>();
                    return;
                }

                context._enemiesEncounterValue = 0;

                // BGMを保存する
                AudioManager.SaveBgm();
                AudioManager.StopBgm(1.0f);

                // 戦闘を行う
                BattleResultType result = await context._battle.ExecuteRandomBattle(context.Map, token);

                // 負けた場合
                if (result == BattleResultType.Lose)
                {
                    // マイルームに戻る
                    SceneChangeManager.StartChange(SceneId.MyRoom);
                }
                else
                {
                    // カメラをプレイヤー視点にする
                    context._cameraController.Target = context._player.transform;

                    // BGMを再生する
                    AudioManager.LoadBgm(1.0f);

                    // フェードイン
                    await FadeManager.FadeIn(token);

                    context.State.SetNextState<PlayerControl>();
                }
            }

            public override void Exit(Dungeon context)
            {
                // 説明文UIを非表示にする
                context._ui.Description.SetEnabled(false);
            }
        }
    }
}
