using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SetsunaTsuyuri.Scenario;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョンの管理者
    /// </summary>
    public class DungeonManager : MonoBehaviour
    {
        /// <summary>
        /// シナリオの管理者
        /// </summary>
        [SerializeField]
        ScenarioManager scenario = null;

        /// <summary>
        /// 味方コンテナの管理者
        /// </summary>
        AllyContainersManager allies = null;

        /// <summary>
        /// 現在、プレイヤーが存在するセクション
        /// </summary>
        int currentPlayerSection = 0;

        /// <summary>
        /// プレイヤーがこのダンジョンをクリアした
        /// </summary>
        bool playerHasClearedThis = false;

        private void Awake()
        {
            allies = GetComponentInChildren<AllyContainersManager>();
        }

        private void Start()
        {
            // セーブデータの戦闘者を味方コンテナへ移す
            allies.TransferCombatantsSaveDataToContainers();

            // プレイ開始
            Play(this.GetCancellationTokenOnDestroy()).Forget();
        }

        /// <summary>
        /// ダンジョンをプレイする(非同期)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask Play(CancellationToken token)
        {
            // ダンジョンデータ
            DungeonData dungeon = RuntimeData.DungeonToPlay;

            // 無限ループ
            while (true)
            {
                // ゲームコマンドを取得する
                GameCommand[] commands = dungeon.DungeonSections[currentPlayerSection].GameCommands;

                // 各コマンドに情報を追加する
                foreach (var command in commands)
                {
                    command.CurrentPlayerSection = currentPlayerSection;
                }

                // ゲームコマンドマネージャーに全てのコマンドを解決させる
                GameCommandResult result = await GameCommandsManager.ResolveCommands(commands, token);

                // 戦いに負けた場合
                if (result.PlayerHasBeenDefeated)
                {
                    // ループ終了
                    break;
                }
                else
                {
                    // 次のセクションが存在する場合
                    if (ExistsNextSection(dungeon))
                    {
                        

                        // 次のセクションへ進む
                        currentPlayerSection++;
                    }
                    else
                    {
                        // クリアフラグON
                        playerHasClearedThis = true;

                        // ループ終了
                        break;
                    }
                }
            }

            // クリアした場合
            if (playerHasClearedThis)
            {
                // セーブデータ
                SaveData save = SaveDataManager.CurrentSaveData;

                // 初めてクリアした場合
                if (!save.ClearedDungeons[dungeon.Id])
                {
                    save.ClearedDungeons[dungeon.Id] = true;

                    // 次のダンジョンを開放できる場合
                    if (CanOpenNextDungeon(dungeon))
                    {
                        save.OpenDungeons[dungeon.Id + 1] = true;

                        // 最初のダンジョン以外の場合
                        if (dungeon.Id != 0)
                        {
                            // フェードイン
                            await FadeManager.FadeIn(token);

                            // メッセージ表示
                            TextAsset text = MasterData.Scenarios.CommonScenarios.GetValueOrDefault(Attribute.Scenario.OpenDungeon);
                            await scenario.PlayAsync(text, token);
                        }
                    }

                    // その他のダンジョン開放
                    foreach (var id in dungeon.OpenDungeonsId)
                    {
                        save.OpenDungeons[id] = true;
                    }
                }

                // 拠点に戻る
                SceneChangeManager.ChangeScene(ScenesName.MyRoom);
            }
            else // クリアしなかった場合
            {
                // 最初のダンジョンの場合
                if (dungeon.Id == 0)
                {
                    // タイトルに戻る
                    SceneChangeManager.ChangeScene(ScenesName.Title);
                }
                else
                {
                    // 拠点に戻る
                    SceneChangeManager.ChangeScene(ScenesName.MyRoom);
                }
            }
        }

        /// <summary>
        /// 次のセクションが存在する
        /// </summary>
        /// <param name="dungeon">ダンジョンデータ</param>
        /// <returns></returns>
        private bool ExistsNextSection(DungeonData dungeon)
        {
            return currentPlayerSection + 1 < dungeon.DungeonSections.Length;
        }

        /// <summary>
        /// 次のダンジョンを開放できる
        /// </summary>
        /// <param name="dungeon">ダンジョンデータ</param>
        /// <returns></returns>
        private bool CanOpenNextDungeon(DungeonData dungeon)
        {
            return dungeon.OpenNextDungeon && dungeon.Id < MasterData.Dungeons.Data.Length - 1;
        }
    }
}
