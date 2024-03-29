﻿using System.Collections.Generic;
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

        /// <summary>
        /// シナリオ
        /// </summary>
        Scenario = 1,

        /// <summary>
        /// シナリオ フェード
        /// </summary>
        ScenarioFade = 2,

        /// <summary>
        /// 待機
        /// </summary>
        Wait = 101,

        /// <summary>
        /// 戦闘
        /// </summary>
        Battle = 201,

        /// <summary>
        /// BGM
        /// </summary>
        Bgm = 301,

        /// <summary>
        /// SE
        /// </summary>
        SE = 401,

        /// <summary>
        /// カメラ振動
        /// </summary>
        CameraShake = 601,

        /// <summary>
        /// イベント進行度
        /// </summary>
        Variable = 1001,

        /// <summary>
        /// イベントフラグ
        /// </summary>
        Flag = 1002,

        /// <summary>
        /// 味方追加
        /// </summary>
        AllyAdding = 1101,

        /// <summary>
        /// ステータス効果
        /// </summary>
        StatusEffect = 1201,

        /// <summary>
        /// アイテム入手
        /// </summary>
        ObtainingItems = 1301,
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

        ///// <summary>
        ///// Ifイベントを解決中である
        ///// </summary>
        //bool _isResolvingIfEvents = false;

        ///// <summary>
        ///// Ifイベントの条件を満たした
        ///// </summary>
        //bool _conditionsIfEvent = false;

        ///// <summary>
        ///// Ifイベントをスキップする
        ///// </summary>
        //bool _skipsIfEvents = false;

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

            string csv = Instance._gameEventDictionary[key];
            List<List<string>> columnsList = CSVReader.ParseCSV(csv);
            List<IGameEvent> gameEvents = ToGameEventList(columnsList);

            foreach (var gameEvent in gameEvents)
            {
                await gameEvent.Resolve(token);
            }

            ScenarioManager.EndIfPlaying();
        }

        /// <summary>
        /// CSVをゲームイベントリストにする
        /// </summary>
        /// <param name="columnsList"></param>
        /// <returns></returns>
        public static List<IGameEvent> ToGameEventList(List<List<string>> columnsList)
        {
            List<IGameEvent> gameEvents = new();

            foreach (var columns in columnsList)
            {
                if (System.Enum.TryParse(columns[0], out GameEventType type))
                {
                    IGameEvent gameEvent = type switch
                    {
                        GameEventType.Scenario => new ScenarioEvent(columns.ToArray()),
                        GameEventType.ScenarioFade => new ScenarioFadeEvent(columns.ToArray()),
                        GameEventType.Battle => new BattleEvent(columns.ToArray()),
                        GameEventType.Bgm => new BgmEvent(columns.ToArray()),
                        GameEventType.SE => new SEEvent(columns.ToArray()),
                        GameEventType.CameraShake => new CameraShakeEvent(columns.ToArray()),
                        GameEventType.Variable => new ProgressEvent(columns.ToArray()),
                        GameEventType.Flag => new FlagEvent(columns.ToArray()),
                        GameEventType.AllyAdding => new AllyAddingEvent(columns.ToArray()),
                        GameEventType.StatusEffect => new StatusEffectEvent(columns.ToArray()),
                        _ => null
                    };

                    if (gameEvent is not null)
                    {
                        gameEvents.Add(gameEvent);
                    }
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
            ScenarioManager.EndIfPlaying();

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
                // TODO: 戦闘結果による処理
            }

            await FadeManager.FadeIn(token);
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

            if (travelEvent.DestinationIsMyRoom)
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

        //private void ResolveIf()
        //{
        //    _isResolvingIfEvents = true;

        //    // TODO: 条件を満たして居ない場合はtrue
        //    _skipsIfEvents = false;
        //}

        //private void ResolveElseIf()
        //{
        //    if (true)
        //    {
        //        _hasResolvedIfEvents = true;
        //    }

        //    // 条件を満たした場合にスキップフラグ反転

        //}

        //private void ResolveElse()
        //{
        //    _skipsIfEvents = _hasResolvedIfEvents;
        //}

        //private void ResolveIfEnd()
        //{
        //    _isResolvingIfEvents = false;
        //}
    }
}
