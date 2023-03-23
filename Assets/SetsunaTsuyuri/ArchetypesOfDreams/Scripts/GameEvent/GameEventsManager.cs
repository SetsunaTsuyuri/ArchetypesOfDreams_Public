using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SetsunaTsuyuri.Scenario;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームイベントの種類
    /// </summary>
    public enum GameEventType
    {
        None = 0,
        Scenario = 1,
    }

    /// <summary>
    /// ゲームイベントの管理者
    /// </summary>
    public class GameEventsManager : Singleton<GameEventsManager>, IInitializable
    {
        /// <summary>
        /// ゲームイベントディクショナリー
        /// </summary>
        Dictionary<string, string> _gameEventDictionary = new();

        public override void Initialize()
        {
            _gameEventDictionary = ResourcesUtility.LoadGameEvents();
        }

        /// <summary>
        /// イベントを解決する
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask Resolve(string key, CancellationToken token)
        {
            if (!Instance._gameEventDictionary.ContainsKey(key))
            {
                return;
            }

            string gameEventAsset = Instance._gameEventDictionary[key];
            List<List<string>> columns = CSVReader.ParseCSV(gameEventAsset);
            List<IGameEvent> gameEvents = new();

            foreach (var column in columns)
            {
                GameEventType type = System.Enum.Parse<GameEventType>(column[0]);
                IGameEvent gameEvent = type switch
                {
                    GameEventType.Scenario => new ScenarioEvent(column.ToArray()),
                    _ => null
                };

                if (gameEvent is not null)
                {
                    gameEvents.Add(gameEvent);
                }
            }

            foreach (var gameEvent in gameEvents)
            {
                await gameEvent.Resolve(token);
            }
        }

        /// <summary>
        /// CSVをゲームイベントにする
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static List<IGameEvent> ToGameEvent(List<List<string>> columns)
        {
            List<IGameEvent> gameEvents = new();

            foreach (var column in columns)
            {
                GameEventType type = System.Enum.Parse<GameEventType>(column[0]);
                IGameEvent gameEvent = type switch
                {
                    GameEventType.Scenario => new ScenarioEvent(column.ToArray()),
                    _ => null
                };

                if (gameEvent is not null)
                {
                    gameEvents.Add(gameEvent);
                }
            }

            return gameEvents;
        }

        /// <summary>
        /// バトルイベントを解決する
        /// </summary>
        /// <param name="battleEvent"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask ResolveBattleEvent(BattleEvent battleEvent, CancellationToken token)
        {
            Battle battle = Battle.InstanceInActiveScene;
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
        /// ID指定シナリオイベントを解決する
        /// </summary>
        /// <param name="scenarioEvent"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask ResolveScenarioEvent(IdScenarioEvent scenarioEvent, CancellationToken token)
        {
            ScenarioManager scenario = ScenarioManager.InstanceInActiveScene;
            if (!scenario)
            {
                return;
            }

            int id = scenarioEvent.Id;

            // csvテキストアセットを取得する
            TextAsset csv = MasterData.GetScenarioData(id).CSVText;

            // シナリオ再生
            await scenario.PlayCsv(csv, token);
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
                SceneChangeManager.StartChange(SceneId.MyRoom);
            }
            else
            {
                // ダンジョンシーンに移行する
                VariableData.DungeonId = travelEvent.DungeonId;
                VariableData.PlayerInitialPosition = travelEvent.Position;
                SceneChangeManager.StartChange(SceneId.Dungeon);
            }
        }
    }
}
