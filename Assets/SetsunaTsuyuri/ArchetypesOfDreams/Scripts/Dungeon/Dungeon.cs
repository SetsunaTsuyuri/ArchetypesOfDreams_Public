using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SetsunaTsuyuri.Scenario;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョン
    /// </summary>
    public partial class Dungeon : MonoBehaviour
    {
        /// <summary>
        /// UI
        /// </summary>
        [SerializeField]
        Canvas _ui = null;

        /// <summary>
        /// 戦闘の管理者
        /// </summary>
        [SerializeField]
        BattleManager _battle = null;

        /// <summary>
        /// 味方コンテナの管理者
        /// </summary>
        [SerializeField]
        AllyContainersManager allies = null;

        /// <summary>
        /// シナリオの管理者
        /// </summary>
        [SerializeField]
        ScenarioManager scenario = null;

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
            State.Add<PlayerControl>();
            State.Add<PlayerTransformRotation>();
            State.Add<MapObjectsTransformMove>();
            State.Add<RandomEncounter>();

            Map = GetComponentInChildren<Map>(true);
        }

        private void Start()
        {
            SetUp();

            AudioManager.PlayBgm(BgmType.Dungeon);

            State.StartChange<MapEventsResolution>(this);

            // プレイ開始
            //Play(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private void SetUp()
        {
            // セーブデータの戦闘者を味方コンテナへ移す
            allies.TransferCombatantsSaveDataToContainers();

            // jsonからマップを作る
            string json = RuntimeData.DungeonToPlay.MapJson.text;
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
            _battle.BattleUI.Description.Hide();
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
                MapEventObject mapEvent = context._reservedMapEventObject;
                if (mapEvent)
                {
                    await mapEvent.ResolveEvents(token);
                }
                context._reservedMapEventObject = null;
                //------------------------------------------------------------------------------------------------
                // 全てのマップオブジェクトの中から、イベント起動条件「プレイヤーに接触」のものを抽出する
                // さらにその中から、プレイヤーに接触しているもの(イベント床のイメージ)を抽出する(近いものが優先)
                // 順番に実行する
                //var a = dungeon.Map.Objects
                //    .Where(x => x.)
                // イベントを解決する
                //await GameCommandsManager.ResolveCommands(null, token);
                //------------------------------------------------------------------------------------------------

                // プレイヤー操作を開始する
                context.State.SetNextState<PlayerControl>();
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
            }

            public override void Update(Dungeon context)
            {
                Player player = context._player;

                // 【移動】
                if (player.WantsToMove())
                {
                    OnMove(context);
                }
                // 【回転】
                else if (player.WantsToRotate())
                {
                    OnRotation(context);
                }
                // 【調べる】
                else if (player.WantsToCheck)
                {
                    OnCheck(context);
                }
                // 【メニュー】
            }

            public override void Exit(Dungeon context)
            {
                // プレイヤーの操作を無効にする
                context._player.DisableInput();
            }

            /// <summary>
            /// 移動
            /// </summary>
            /// <param name="dungeon"></param>
            private void OnMove(Dungeon dungeon)
            {
                Player player = dungeon._player;
                Map map = dungeon.Map;

                if (player.CanMove(map))
                {
                    // 移動する
                    player.Move();

                    // マップオブジェクトのトランスフォーム移動を開始する
                    dungeon.State.StartChange<MapObjectsTransformMove>(dungeon);
                }
                else if (player.GetMapEventObject(map, MapEventTriggerType.Touch, out MapEventObject mapEventObject))
                {
                    // 起動するイベントを予約する
                    dungeon._reservedMapEventObject = mapEventObject;

                    // マップイベントの処理を開始する
                    dungeon.State.StartChange<MapEventsResolution>(dungeon);
                }
            }

            /// <summary>
            /// 回転
            /// </summary>
            /// <param name="dungeon"></param>
            private void OnRotation(Dungeon dungeon)
            {
                // 回転する
                dungeon._player.Rotate();

                // プレイヤーのトランスフォーム回転を開始する
                dungeon.State.StartChange<PlayerTransformRotation>(dungeon);
            }

            /// <summary>
            /// 調査
            /// </summary>
            /// <param name="dungeon"></param>
            private void OnCheck(Dungeon dungeon)
            {
                Player player = dungeon._player;
                Map map = dungeon.Map;

                if (player.GetMapEventObject(map, MapEventTriggerType.Check, out MapEventObject mapEvent))
                {
                    // 起動するイベントを予約する
                    dungeon._reservedMapEventObject = mapEvent;

                    // マップイベントの処理を開始する
                    dungeon.State.StartChange<MapEventsResolution>(dungeon);
                }
            }

            /// <summary>
            /// メニュー
            /// </summary>
            /// <param name="dungeon"></param>
            private void OnMenu(Dungeon dungeon)
            {

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

                // マップイベントを解決する
                context.State.SetNextState<MapEventsResolution>();
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

                // その他マップオブジェクトの移動
                // 配列にしてWhenAll

                if (context._player.GetMapEventObject(context.Map, MapEventTriggerType.Touch, out MapEventObject mapEventObject))
                {
                    context._reservedMapEventObject = mapEventObject;
                    context.State.SetNextState<MapEventsResolution>();
                }
                else
                {
                    // ランダムエンカウントの処理
                    context.State.SetNextState<RandomEncounter>();
                }

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

                context._enemiesEncounterValue += context._enemiesEncounterValueIncrease;

                Debug.Log(context._enemiesEncounterValue);

                if (context._enemiesEncounterValue >= context._enemiesEncounterThreshold)
                {
                    context._enemiesEncounterValue = 0;

                    // BGMを保存する
                    AudioManager.SaveBgm();
                    AudioManager.StopBgm(1.0f);
                    AudioManager.PlaySE(SEType.BattleStart);

                    // フェードアウト
                    await FadeManager.FadeOut(token);

                    // 非アクティブにする
                    context.gameObject.SetActive(false);

                    // ダンジョンUIを非表示にする
                    context._ui.enabled = false;

                    // 戦闘を行う
                    BattleResultType result = await context._battle.ExecuteRandomBattle(context.Map, token);

                    // 負けた場合
                    if (result == BattleResultType.Lose)
                    {
                        // マイルームに戻る
                        SceneChangeManager.ChangeScene(SceneNames.MyRoom);
                    }
                    else
                    {

                        // アクティブにする
                        context.gameObject.SetActive(true);

                        // カメラをプレイヤー視点にする
                        context._cameraController.Target = context._player.transform;

                        // UIを表示する
                        context._ui.enabled = true;

                        // BGMを再生する
                        AudioManager.LoadBgm(1.0f);

                        // フェードイン
                        await FadeManager.FadeIn(token);
                        context.State.SetNextState<MapEventsResolution>();
                    }
                }
                else
                {
                    context.State.SetNextState<MapEventsResolution>();
                }
            }

            public override void Enter(Dungeon context)
            {
                // 説明文UIを表示する
                context._battle.BattleUI.Description.Show();
            }

            public override void Exit(Dungeon context)
            {
                // 説明文UIを非表示にする
                context._battle.BattleUI.Description.Hide();
            }
        }

        ///// <summary>
        ///// ダンジョンをプレイする(非同期)
        ///// </summary>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //private async UniTask Play(CancellationToken token)
        //{
        //    // 接触地面イベント実行

        //    // セルイベントリスト消化

        //    // プレイヤー行動決定フェイズ → イベントリストに追加、接触イベント実行へ

        //    // エネミー行動決定フェイズ → イベントリスト追加、 接触イベント実行へ

        //    // Moveフェイズ

        //    // エンカウンターフェイズ エンカウントゲージMAXで戦闘イベントをイベントリスト追加、接触イベント実行へ

        //    // ダンジョンデータ
        //    DungeonData dungeon = RuntimeData.DungeonToPlay;

        //    // 無限ループ
        //    while (true)
        //    {
        //        // ゲームコマンドを取得する
        //        GameCommand[] commands = dungeon.DungeonSections[_currentPlayerSection].GameCommands;

        //        // 各コマンドに情報を追加する
        //        foreach (var command in commands)
        //        {
        //            command.CurrentPlayerSection = _currentPlayerSection;
        //        }

        //        // ゲームコマンドマネージャーに全てのコマンドを解決させる
        //        GameCommandResult result = await GameEventsManager.ResolveCommands(commands, token);

        //        // 戦いに負けた場合
        //        if (result.PlayerHasBeenDefeated)
        //        {
        //            // ループ終了
        //            break;
        //        }
        //        else
        //        {
        //            // 次のセクションが存在する場合
        //            if (ExistsNextSection(dungeon))
        //            {


        //                // 次のセクションへ進む
        //                _currentPlayerSection++;
        //            }
        //            else
        //            {
        //                // クリアフラグON
        //                _playerHasClearedThis = true;

        //                // ループ終了
        //                break;
        //            }
        //        }
        //    }

        //    // クリアした場合
        //    if (_playerHasClearedThis)
        //    {
        //        // セーブデータ
        //        SaveData save = SaveDataManager.CurrentSaveData;

        //        // 初めてクリアした場合
        //        if (!save.ClearedDungeons[dungeon.Id])
        //        {
        //            save.ClearedDungeons[dungeon.Id] = true;

        //            // 次のダンジョンを開放できる場合
        //            if (CanOpenNextDungeon(dungeon))
        //            {
        //                save.OpenDungeons[dungeon.Id + 1] = true;

        //                // 最初のダンジョン以外の場合
        //                if (dungeon.Id != 0)
        //                {
        //                    // フェードイン
        //                    await FadeManager.FadeIn(token);

        //                    // メッセージ表示
        //                    TextAsset text = MasterData.Scenarios.CommonScenarios.GetValueOrDefault(Attribute.Scenario.OpenDungeon);
        //                    await scenario.PlayAsync(text, token);
        //                }
        //            }

        //            // その他のダンジョン開放
        //            foreach (var id in dungeon.OpenDungeonsId)
        //            {
        //                save.OpenDungeons[id] = true;
        //            }
        //        }

        //        // 拠点に戻る
        //        SceneChangeManager.ChangeScene(SceneNames.MyRoom);
        //    }
        //    else // クリアしなかった場合
        //    {
        //        // 最初のダンジョンの場合
        //        if (dungeon.Id == 0)
        //        {
        //            // タイトルに戻る
        //            SceneChangeManager.ChangeScene(SceneNames.Title);
        //        }
        //        else
        //        {
        //            // 拠点に戻る
        //            SceneChangeManager.ChangeScene(SceneNames.MyRoom);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 次のセクションが存在する
        ///// </summary>
        ///// <param name="dungeon">ダンジョンデータ</param>
        ///// <returns></returns>
        //private bool ExistsNextSection(DungeonData dungeon)
        //{
        //    return _currentPlayerSection + 1 < dungeon.DungeonSections.Length;
        //}

        ///// <summary>
        ///// 次のダンジョンを開放できる
        ///// </summary>
        ///// <param name="dungeon">ダンジョンデータ</param>
        ///// <returns></returns>
        //private bool CanOpenNextDungeon(DungeonData dungeon)
        //{
        //    return dungeon.OpenNextDungeon && dungeon.Id < MasterData.Dungeons.Data.Length - 1;
        //}
    }
}
