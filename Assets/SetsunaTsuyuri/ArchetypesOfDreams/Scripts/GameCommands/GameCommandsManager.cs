using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using SetsunaTsuyuri.Scenario;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームコマンドの管理者
    /// </summary>
    public class GameCommandsManager : Singleton<GameCommandsManager>, IInitializable
    {
        /// <summary>
        /// ゲームコマンドの解決を中止するフラグ
        /// </summary>
        bool stopCommandsResolution = false;

        public override void Initialize()
        {
            stopCommandsResolution = false;
        }

        /// <summary>
        /// ゲームコマンドを解決する
        /// </summary>
        /// <param name="commands">ゲームコマンド配列</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask<GameCommandResult> ResolveCommands(GameCommand[] commands, CancellationToken token)
        {
           return await Instance.ResolveCommandsInner(commands, token);
        }

        /// <summary>
        /// ゲームコマンドの配列を解決する
        /// </summary>
        /// <param name="commands">ゲームコマンド配列</param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask<GameCommandResult> ResolveCommandsInner(GameCommand[] commands, CancellationToken token)
        {
            // 初期化する
            Initialize();

            // ゲームコマンドの実行結果
            GameCommandResult result = new GameCommandResult();

            // 順番にコマンドを解決する
            foreach (var command in commands)
            {
                // 中止フラグが立っているなら止める
                if (stopCommandsResolution)
                {
                    break;
                }

                // コマンドを解決する
                await ResolveCommand(command, result, token);
            }

            return result;
        }

        /// <summary>
        /// ゲームコマンドを解決する
        /// </summary>
        /// <param name="command">ゲームコマンド</param>
        /// <param name="result">ゲームコマンドの実行結果</param>
        private async UniTask ResolveCommand(GameCommand command, GameCommandResult result, CancellationToken token)
        {
            // コマンドの属性に応じて、適切なメソッドを実行する
            System.Func<GameCommand, GameCommandResult, CancellationToken, UniTask> func = command.Command switch
            {
                Attribute.GameCommand.Battle => OnBattle,
                Attribute.GameCommand.Scenario => OnScenario,
                _ => null
            };

            if (func != null)
            {
                await func.Invoke(command, result, token);
            }
        }

        /// <summary>
        /// 戦闘の場合
        /// </summary>
        /// <param name="command">ゲームコマンド</param>
        /// <param name="result">ゲームコマンドの実行結果</param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask OnBattle(GameCommand command, GameCommandResult result, CancellationToken token)
        {
            BattleManager battle = Object.FindObjectOfType<BattleManager>();
            if (!battle)
            {
                return;
            }

            // ボス戦以外の場合
            if (!command.IsBossBattle)
            {
                // 通常戦闘BGMを再生する
                AudioManager.PlayBGM("通常戦闘");
            }

            // 戦闘を行う
            await battle.ExecuteBattle(command, token);

            // プレイヤーが負けた場合
            if (!battle.Allies.CanFight())
            {
                // プレイヤー敗北フラグON
                result.PlayerHasBeenDefeated = true;

                // 中止フラグON
                stopCommandsResolution = true;
            }

            // ★暫定処理 選択解除
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>
        /// シナリオの場合
        /// </summary>
        /// <param name="command">ゲームコマンドデータ</param>
        /// <param name="result">ゲームコマンドの実行結果</param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask OnScenario(GameCommand command, GameCommandResult result, CancellationToken token)
        {
            ScenarioManager scenario = Object.FindObjectOfType<ScenarioManager>();
            if (!scenario)
            {
                return;
            }

            // メインストーリーの場合、クリア済みのダンジョンでは再生しない
            if (command.Scenario == Attribute.Scenario.MainStory &&
                SaveDataManager.CurrentSaveData.ClearedDungeons[RuntimeData.DungeonToPlay.Id])
            {
                return;
            }

            // csvテキストアセットを取得する
            TextAsset csv;
            if (command.Scenario == Attribute.Scenario.MainStory)
            {
                csv = MasterData.Scenarios.GetValue(command.Id);
            }
            else
            {
                csv = MasterData.Scenarios.CommonScenarios.GetValueOrDefault(command.Scenario);
            }

            // フェードイン
            await FadeManager.FadeIn(token);

            await scenario.PlayAsync(csv, token);
        }
    }
}
