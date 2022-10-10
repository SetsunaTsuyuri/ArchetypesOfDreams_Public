using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マップイベントの種類
    /// </summary>
    public enum MapEventObjectType
    {
        /// <summary>
        /// なし
        /// </summary>
        None = 0,

        /// <summary>
        /// イベント
        /// </summary>
        Event = 1,

        /// <summary>
        /// 開始地点
        /// </summary>
        Start = 2,

        /// <summary>
        /// 終了地点
        /// </summary>
        Goal = 3
    }

    /// <summary>
    /// マップイベントの起動条件
    /// </summary>
    public enum MapEventTriggerType
    {
        /// <summary>
        /// なし
        /// </summary>
        None = 0,

        /// <summary>
        /// 接触
        /// </summary>
        Touch = 1,

        /// <summary>
        /// 調べる
        /// </summary>
        Check = 2
    }

    /// <summary>
    /// マップイベント
    /// </summary>
    public class MapEventObject : MapObject
    {
        /// <summary>
        /// 大きさの拡張
        /// </summary>
        public Vector2Int SizeExtension { get; private set; } = Vector2Int.zero;

        /// <summary>
        /// 種類
        /// </summary>
        [field: SerializeField]
        public MapEventObjectType Type { get; private set; } = MapEventObjectType.None;

        /// <summary>
        /// イベントの起動条件
        /// </summary>
        [field: SerializeField]
        public MapEventTriggerType Trigger { get; private set; } = MapEventTriggerType.None;

        /// <summary>
        /// 出現条件リスト
        /// </summary>
        [field: SerializeReference]
        public List<IAppearanceTrigger> AppearanceTriggers { get; set; } = new();

        /// <summary>
        /// イベントリスト
        /// </summary>
        [field: SerializeReference]
        public List<IGameEvent> Events { get; private set; } = new();

        /// <summary>
        /// イベントを解決する
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask ResolveEvents(CancellationToken token)
        {
            foreach (var gameEvent in Events)
            {
                await gameEvent.GetUniTask(token);
            }
        }

        /// <summary>
        /// 存在する
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            bool result = true;

            foreach (var trigger in AppearanceTriggers)
            {
                if (!trigger.Evaluate())
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="mapEventObject">マップイベントオブジェクトのJToken</param>
        /// <param name="map">マップ</param>
        public void SetUp(JToken mapEventObject, Map map)
        {
            //// 種類とゲームオブジェクトの名前
            string type = TiledMapUtility.GetPropertyValue(mapEventObject, "type", string.Empty);
            if (Enum.TryParse(type, out MapEventObjectType result))
            {
                Type = result;
                gameObject.name = type;
            }

            // 大きさ
            int width = GetSizeOrPositionValue(mapEventObject, "width", map.CellSize);
            int height = GetSizeOrPositionValue(mapEventObject, "height", map.CellSize);
            SizeExtension = new(width, height);

            // 位置
            int positionX = GetSizeOrPositionValue(mapEventObject, "x", map.CellSize);
            int positionY = GetSizeOrPositionValue(mapEventObject, "y", map.CellSize);
            Position = new(positionX, positionY);

            // 向き
            int directionX = TiledMapUtility.GetPropertyValue(mapEventObject, "direction_x", 0);
            int directionY = TiledMapUtility.GetPropertyValue(mapEventObject, "direction_y", 1);
            Direction = new(directionX, directionY);

            // 起動条件
            Trigger = TiledMapUtility.GetPropertyValue(mapEventObject, "trigger", MapEventTriggerType.None);

            // 他のオブジェクトと衝突するか
            Collides = TiledMapUtility.GetPropertyValue(mapEventObject, "collides", true);

            // 進入可能なセルの種類
            //AccessibleCells = 

            // 移動パターン
            // 不動、プレイヤー追跡等

            // プロパティ
            JToken properties = mapEventObject["properties"];
            foreach (var property in properties)
            {
                if (property["propertytype"] is not null)
                {
                    AddEventOrExtraTrigger(property);
                }
            }
            // トランスフォーム
            UpdateTransform();
        }

        /// <summary>
        /// 大きさまたは位置の値を取得する
        /// </summary>
        /// <param name="mapEvent">マップイベントのJToken</param>
        /// <param name="valueName">値の名前</param>
        /// <param name="cellSize">セルの大きさ</param>
        /// <returns></returns>
        private int GetSizeOrPositionValue(JToken mapEvent, string valueName, int cellSize)
        {
            int value = Mathf.FloorToInt(mapEvent[valueName].Value<float>()) / cellSize;
            return value;
        }

        /// <summary>
        /// イベントを追加する
        /// </summary>
        /// <param name="property">プロパティ</param>
        /// <returns></returns>
        private void AddEventOrExtraTrigger(JToken property)
        {
            string propertyType = property?["propertytype"]?.ToString();
            JToken values = property?["value"];

            switch (propertyType)
            {
                case "Battle":
                    IGameEvent battle = CreateBattleEvent(values);
                    Events.Add(battle);
                    break;

                case "Scenario":
                    IGameEvent scenario = CreateScenarioEvent(values);
                    Events.Add(scenario);
                    break;

                case "Flags":
                    IGameEvent flags = CreateStoryEvent(values);
                    Events.Add(flags);
                    break;

                case "Travel":
                    IGameEvent travel = CreateTravelEvent(values);
                    Events.Add(travel);
                    break;

                case "StoryTrigger":
                    IAppearanceTrigger stroyTrigger = CreateStoryTrigger(values);
                    AppearanceTriggers.Add(stroyTrigger);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// バトルイベントを作る
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private IGameEvent CreateBattleEvent(JToken values)
        {
            BattleEvent battleEvent = new()
            {
                EnemyGroupId = values["id"]?.ToObject<int>() ?? 0,
                IsBossBattle = values["is_boss_battle"]?.ToObject<bool>() ?? false
            };

            return battleEvent;
        }

        /// <summary>
        /// シナリオイベントを作る
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private IGameEvent CreateScenarioEvent(JToken values)
        {
            ScenarioEvent scenarioEvent = new()
            {
                Id = values["id"]?.ToObject<int>() ?? 0
            };

            return scenarioEvent;
        }

        /// <summary>
        /// フラグイベントを作る
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private IGameEvent CreateStoryEvent(JToken values)
        {
            StoryEvent storyEvent = new();

            return storyEvent;
        }

        /// <summary>
        /// 場所移動イベントを作る
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private IGameEvent CreateTravelEvent(JToken values)
        {
            TravelEvent travelEvent = new()
            {
                MapId = values["map_id"]?.ToObject<int>() ?? -1
            };

            return travelEvent;
        }

        /// <summary>
        /// ストーリートリガーを作る
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private IAppearanceTrigger CreateStoryTrigger(JToken values)
        {
            int? id = values["id"]?.ToObject<int>();
            ProgressionTrigger.FormulaType? formula = values["Formula"]?.ToObject<ProgressionTrigger.FormulaType>();
            int? parameter = values["parameter"]?.ToObject<int>();

            StoryTrigger storyTrigger = new(id, formula, parameter);

            return storyTrigger;
        }
    }
}
