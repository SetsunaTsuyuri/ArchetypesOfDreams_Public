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
    public enum MapEventType
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
    /// マップイベントオブジェクトの種類
    /// </summary>
    public enum MapEventObjectType
    {
        /// <summary>
        /// なし
        /// </summary>
        None = 0,

        /// <summary>
        /// 上り階段
        /// </summary>
        StairsUp = 1,

        /// <summary>
        /// 下り階段
        /// </summary>
        StairsDown = 2
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
        public MapEventType Type { get; private set; } = MapEventType.None;

        /// <summary>
        /// 見た目の種類
        /// </summary>
        [field: SerializeField]
        public MapEventObjectType ObjectType { get; private set; } = MapEventObjectType.None;

        /// <summary>
        /// イベントの起動条件
        /// </summary>
        [field: SerializeField]
        public MapEventTriggerType Trigger { get; private set; } = MapEventTriggerType.None;

        /// <summary>
        /// 出現条件リスト
        /// </summary>
        [field: SerializeReference]
        public List<IAppearanceCondition> AppearanceConditions { get; set; } = new();

        /// <summary>
        /// イベントID
        /// </summary>
        public string EventId { get; private set; } = string.Empty;

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
            if (!string.IsNullOrEmpty(EventId))
            {
                await GameEventsManager.Resolve(EventId, token);
            }

            foreach (var gameEvent in Events)
            {
                await gameEvent.Resolve(token);
            }
        }

        /// <summary>
        /// 存在する
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            bool result = true;

            foreach (var condition in AppearanceConditions)
            {
                if (!condition.Evaluate())
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
        /// <param name="jToken">マップイベントオブジェクトのJToken</param>
        /// <param name="map">マップ</param>
        public void SetUp(JToken jToken, Map map)
        {
            // 出現条件
            string appearanceConditions = TiledMapUtility.GetPropertyValue(jToken, "appearance_conditions", string.Empty);
            AddConditionsOrEvents(appearanceConditions, AddAppearanceCondition);

            //// 種類とゲームオブジェクトの名前
            string type = TiledMapUtility.GetPropertyValue(jToken, "type", string.Empty);
            if (Enum.TryParse(type, out MapEventType result))
            {
                Type = result;
                gameObject.name = type;
            }

            // 大きさ
            int width = GetSizeOrPositionValue(jToken, "width", map.CellSize);
            int height = GetSizeOrPositionValue(jToken, "height", map.CellSize);
            SizeExtension = new(width, height);

            // 位置
            int positionX = GetSizeOrPositionValue(jToken, "x", map.CellSize);
            int positionY = GetSizeOrPositionValue(jToken, "y", map.CellSize);
            Position = new(positionX, positionY);

            // 向き
            int directionX = TiledMapUtility.GetPropertyValue(jToken, "direction_x", 0);
            int directionY = TiledMapUtility.GetPropertyValue(jToken, "direction_y", 1);
            Direction = new(directionX, directionY);

            // オブジェクトタイプ
            ObjectType = TiledMapUtility.GetPropertyValue(jToken, "object_type", MapEventObjectType.None);

            // 起動条件
            Trigger = TiledMapUtility.GetPropertyValue(jToken, "trigger", MapEventTriggerType.None);

            // 他のオブジェクトと衝突するか
            CanCollide = TiledMapUtility.GetPropertyValue(jToken, "can_collide", true);

            // 進入可能なセルの種類
            //AccessibleCells = 

            // 移動パターン
            // 不動、プレイヤー追跡等

            // イベントID
            EventId = TiledMapUtility.GetPropertyValue(jToken, "event_id", string.Empty);

            // イベント
            string events = TiledMapUtility.GetPropertyValue(jToken, "events", string.Empty);
            AddConditionsOrEvents(events, AddEvent);

            // トランスフォーム
            UpdateTransform();

            // オブジェクト作成
            CreateObject(map);
        }

        /// <summary>
        /// 大きさまたは位置の値を取得する
        /// </summary>
        /// <param name="jToken">マップイベントのJToken</param>
        /// <param name="valueName">値の名前</param>
        /// <param name="cellSize">セルの大きさ</param>
        /// <returns></returns>
        private int GetSizeOrPositionValue(JToken jToken, string valueName, int cellSize)
        {
            int value = Mathf.FloorToInt(jToken[valueName].Value<float>()) / cellSize;
            return value;
        }

        /// <summary>
        /// 出現条件またはイベントを追加する
        /// </summary>
        /// <param name="script"></param>
        /// <param name="action"></param>
        private void AddConditionsOrEvents(string script, Action<string[]> action)
        {
            string[] rows = script.Split("\n");
            foreach (var row in rows)
            {
                string[] columns = row.Split(" ");
                action?.Invoke(columns);
            }
        }

        /// <summary>
        /// 出現条件を追加する
        /// </summary>
        /// <param name="columns"></param>
        private void AddAppearanceCondition(string[] columns)
        {
            IAppearanceCondition appearanceTrigger = columns[0] switch
            {
                "Story" => new StoryCondition(columns),
                _ => null
            };

            if (appearanceTrigger is not null)
            {
                AppearanceConditions.Add(appearanceTrigger);
            }
        }

        /// <summary>
        /// イベントを追加する
        /// </summary>
        /// <param name="columns"></param>
        private void AddEvent(string[] columns)
        {
            IGameEvent gameEvent = columns[0] switch
            {
                "Battle" => new BattleEvent(columns),
                "Scenario" => new IdScenarioEvent(columns),
                "Travel" => new TravelEvent(columns),
                "Story" => new StoryEvent(columns),
                _ => null
            };

            if (gameEvent is not null)
            {
                Events.Add(gameEvent);
            }
        }

        /// <summary>
        /// オブジェクトを作る
        /// </summary>
        /// <param name="map">マップ</param>
        private void CreateObject(Map map)
        {
            GameObject prefab = map.GetMapEventObjectPrefab(ObjectType);
            if (prefab != null)
            {
                Instantiate(prefab, transform);
            }
        }
    }
}
