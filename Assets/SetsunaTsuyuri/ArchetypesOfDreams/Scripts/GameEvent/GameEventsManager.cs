using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SetsunaTsuyuri.Scenario;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームイベントの管理者
    /// </summary>
    public class GameEventsManager : Singleton<GameEventsManager>, IInitializable
    {
        ///// <summary>
        ///// ゲームコマンドの解決を中止するフラグ
        ///// </summary>
        //bool _stopCommandsResolution = false;

        /// <summary>
        /// シナリオの管理者
        /// </summary>
        ScenarioManager _scenario = null;

        /// <summary>
        /// シナリオの管理者
        /// </summary>
        public static ScenarioManager Scenario
        {
            get
            {
                if (Instance._scenario == null)
                {
                    Instance._scenario = Object.FindObjectOfType<ScenarioManager>();
                }

                return Instance._scenario;
            }
        }

        public override void Initialize()
        {
            //_stopCommandsResolution = false;
        }

        /// <summary>
        /// バトルイベントを解決する
        /// </summary>
        /// <param name="battleEvent"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask ResolveBattleEvent(BattleEvent battleEvent, CancellationToken token)
        {
            BattleManager battle = BattleManager.InstanceInActiveScene;
            if (!battle)
            {
                return;
            }

            // 戦闘を行う
            BattleResultType result = await battle.ExecuteEventBattle(battleEvent, token);

            // 勝利後のフェードイン
            if (result == BattleResultType.Win)
            {
                await FadeManager.FadeIn(token);
            }
        }

        /// <summary>
        /// シナリオイベントを解決する
        /// </summary>
        /// <param name="scenarioEvent"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask ResolveScenarioEvent(ScenarioEvent scenarioEvent, CancellationToken token)
        {
            if (!Scenario)
            {
                return;
            }

            int id = scenarioEvent.Id;

            // csvテキストアセットを取得する
            TextAsset csv = MasterData.GetScenarioData(id).CSVText;

            // フェードイン
            await FadeManager.FadeIn(token);
            
            // シナリオ再生
            await Scenario.PlayAsync(csv, token);
        }

        /// <summary>
        /// 場所移動イベントを解決する
        /// </summary>
        /// <param name="travelEvent">移動イベント</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask ResolveTravelEvent(TravelEvent travelEvent, CancellationToken token)
        {
            // フェードアウト
            await FadeManager.FadeOut(token);

            if (travelEvent.DestinationIsMyRoom())
            {
                // 自室シーンに移行する
                SceneChangeManager.StartChange(SceneNames.MyRoom);
            }
            else
            {
                // ダンジョンシーンに移行する
                VariableData.DungeonId = travelEvent.DungeonId;
                VariableData.PlayerInitialPosition = travelEvent.Position;
                SceneChangeManager.StartChange(SceneNames.Dungeon);
            }
        }

        ///// <summary>
        ///// 戦闘の場合
        ///// </summary>
        ///// <param name="command">ゲームコマンド</param>
        ///// <param name="result">ゲームコマンドの実行結果</param>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //private async UniTask OnBattle(GameCommand command, GameCommandResult result, CancellationToken token)
        //{
        //    BattleManager battle = Object.FindObjectOfType<BattleManager>();
        //    if (!battle)
        //    {
        //        return;
        //    }

        //    // ボス戦以外の場合
        //    if (!command.IsBossBattle)
        //    {
        //        // 通常戦闘BGMを再生する
        //        AudioManager.PlayBgm("通常戦闘");
        //    }

        //    // 戦闘を行う
        //    await battle.ExecuteBattle(command, token);

        //    // プレイヤーが負けた場合
        //    if (!battle.Allies.CanFight())
        //    {
        //        // プレイヤー敗北フラグON
        //        result.PlayerHasBeenDefeated = true;

        //        // 中止フラグON
        //        _stopCommandsResolution = true;
        //    }

        //    // ★暫定処理 選択解除
        //    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        //}

        ///// <summary>
        ///// シナリオの場合
        ///// </summary>
        ///// <param name="command">ゲームコマンドデータ</param>
        ///// <param name="result">ゲームコマンドの実行結果</param>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //private async UniTask OnScenario(GameCommand command, GameCommandResult result, CancellationToken token)
        //{
        //    ScenarioManager scenario = Object.FindObjectOfType<ScenarioManager>();
        //    if (!scenario)
        //    {
        //        return;
        //    }

        //    // メインストーリーの場合、クリア済みのダンジョンでは再生しない
        //    if (command.ScenarioAttribute == Attribute.Scenario.MainStory &&
        //        SaveDataManager.CurrentSaveData.ClearedDungeons[RuntimeData.DungeonToPlay.Id])
        //    {
        //        return;
        //    }

        //    // csvテキストアセットを取得する
        //    TextAsset csv;
        //    if (command.ScenarioAttribute == Attribute.Scenario.MainStory)
        //    {
        //        csv = MasterData.Scenarios.GetData(command.Id).CSVText;
        //    }
        //    else
        //    {
        //        csv = MasterData.Scenarios.CommonScenarios.GetValueOrDefault(command.ScenarioAttribute);
        //    }

        //    // フェードイン
        //    await FadeManager.FadeIn(token);

        //    await scenario.PlayAsync(csv, token);
        //}

        ///// <summary>
        ///// ゲームコマンドを解決する
        ///// </summary>
        ///// <param name="commands">ゲームコマンド配列</param>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //public static async UniTask<GameCommandResult> ResolveCommands(GameCommand[] commands, CancellationToken token)
        //{
        //    return await Instance.ResolveCommandsInner(commands, token);
        //}

        ///// <summary>
        ///// ゲームコマンドの配列を解決する
        ///// </summary>
        ///// <param name="commands">ゲームコマンド配列</param>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //private async UniTask<GameCommandResult> ResolveCommandsInner(GameCommand[] commands, CancellationToken token)
        //{
        //    // 初期化する
        //    Initialize();

        //    // ゲームコマンドの実行結果
        //    GameCommandResult result = new();

        //    // 順番にコマンドを解決する
        //    foreach (var command in commands)
        //    {
        //        // 中止フラグが立っているなら止める
        //        if (_stopCommandsResolution)
        //        {
        //            break;
        //        }

        //        // コマンドを解決する
        //        await ResolveCommand(command, result, token);
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// ゲームコマンドを解決する
        ///// </summary>
        ///// <param name="command">ゲームコマンド</param>
        ///// <param name="result">ゲームコマンドの実行結果</param>
        //private async UniTask ResolveCommand(GameCommand command, GameCommandResult result, CancellationToken token)
        //{
        //    // コマンドの属性に応じて、適切なメソッドを実行する
        //    System.Func<GameCommand, GameCommandResult, CancellationToken, UniTask> func = command.Command switch
        //    {
        //        Attribute.GameCommand.Battle => OnBattle,
        //        Attribute.GameCommand.Scenario => OnScenario,
        //        _ => null
        //    };

        //    if (func != null)
        //    {
        //        await func.Invoke(command, result, token);
        //    }
        //}
    }
}
