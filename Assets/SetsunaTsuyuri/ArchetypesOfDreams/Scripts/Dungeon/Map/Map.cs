using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マップ
    /// </summary>
    public class Map : MonoBehaviour
    {
        /// <summary>
        /// セルの親トランスフォーム
        /// </summary>
        [SerializeField]
        Transform _cellsParent = null;

        /// <summary>
        /// オブジェクトの親トランスフォーム
        /// </summary>
        [SerializeField]
        Transform _objectsParent = null;

        /// <summary>
        /// 床のプレハブ
        /// </summary>
        [SerializeField]
        GameObject _floorPrefab = null;

        /// <summary>
        /// 壁のプレハブ
        /// </summary>
        [SerializeField]
        GameObject _wallPrefab = null;

        /// <summary>
        /// 上り階段
        /// </summary>
        [SerializeField]
        GameObject _stairsUp = null;

        /// <summary>
        /// 下り階段
        /// </summary>
        [SerializeField]
        GameObject _StairsDown = null;

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 幅
        /// </summary>
        public int Width { get; private set; } = 0;

        /// <summary>
        /// 高さ
        /// </summary>
        public int Height { get; private set; } = 0;

        /// <summary>
        /// セル配列
        /// </summary>
        public MapCell[] Cells { get; private set; } = { };

        /// <summary>
        /// イベントリスト
        /// </summary>
        public List<MapEventObject> Events { get; private set; } = new();

        /// <summary>
        /// セルの大きさ
        /// </summary>
        public int CellSize { get; private set; } = 0;

        /// <summary>
        /// 敵の基本レベル
        /// </summary>
        public int BasicEnemyLevel { get; private set; } = 0;

        /// <summary>
        /// 敵の最小ID
        /// </summary>
        public int MinEnemyId { get; private set; } = 0;

        /// <summary>
        /// 敵の最大ID
        /// </summary>
        public int MaxEnemyId { get; private set; } = 0;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="json"></param>
        public void SetUp(string json)
        {
            JObject jObject = JObject.Parse(json);

            // 幅
            Width = jObject["width"].Value<int>();

            // 高さ
            Height = jObject["height"].Value<int>();

            // セルの大きさ
            CellSize = jObject["tilewidth"].Value<int>();

            // マップ名
            Name = TiledMapUtility.GetPropertyValue<string>(jObject, "name");

            // 敵の平均レベル
            BasicEnemyLevel = TiledMapUtility.GetPropertyValue<int>(jObject, "basic_enemy_level");

            // 敵の最小ID
            MinEnemyId = TiledMapUtility.GetPropertyValue<int>(jObject, "min_enemy_id");

            // 敵の最大ID
            MaxEnemyId = TiledMapUtility.GetPropertyValue<int>(jObject, "max_enemy_id");

            // セルを作る
            CreateCells(jObject);
            CreateCellGameObjects();

            // マップイベントを作る
            CreateEvents(jObject);
        }

        /// <summary>
        /// セルを作る
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        private void CreateCells(JToken jToken)
        {
            MapCellType[][] cellTypesArray = CreateCellTypesArray(jToken);

            Cells = new MapCell[Width * Height];
            for (int i = 0; i < Cells.Length; i++)
            {
                // セルを作る
                MapCell cell = new()
                {
                    Position = ToPosition(i),
                    Types = new MapCellType[cellTypesArray.Length]
                };

                // セルのタイプを追加する
                for (int j = 0; j < cellTypesArray.Length; j++)
                {
                    cell.Types[j] = cellTypesArray[j][i];
                }

                // セル配列に入れる
                Cells[i] = cell;
            }
        }

        /// <summary>
        /// セルのゲームオブジェクトを作る
        /// </summary>
        private void CreateCellGameObjects()
        {
            foreach (var cell in Cells)
            {
                if (cell.Types.Any(x => x == MapCellType.Floor))
                {
                    Vector3Int position = ToWorldPosition(cell);
                    Instantiate(_floorPrefab, position, Quaternion.identity, _cellsParent);
                }
                else if (cell.IsNone())
                {
                    MapCell[] surroundingCells = GetSurroundingCells(cell);
                    if (surroundingCells.Any(x => !x.IsNone()))
                    {
                        Vector3Int position = ToWorldPosition(cell);
                        Instantiate(_wallPrefab, position, Quaternion.identity, _cellsParent);
                    }
                }

            }
        }

        /// <summary>
        /// セルタイプのジャグ配列を作る
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        private MapCellType[][] CreateCellTypesArray(JToken jToken)
        {
            IEnumerable<JToken> tileLayers = jToken["layers"]
                .Where(x => x["type"].Value<string>() == "tilelayer");

            MapCellType[][] tileCellsArray = new MapCellType[tileLayers.Count()][];
            for (int i = 0; i < tileCellsArray.Length; i++)
            {
                MapCellType[] cells = tileLayers.ElementAt(i)["data"]
                    .ToObject<MapCellType[]>();

                tileCellsArray[i] = cells;
            }

            return tileCellsArray;
        }

        /// <summary>
        /// オブジェクトを作る
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="dungeon"></param>
        private void CreateEvents(JToken jToken)
        {
            IEnumerable<JToken> objects = jToken["layers"]
                .First(x => x["type"].Value<string>() == "objectgroup")
                ["objects"];

            foreach (var obj in objects)
            {
                MapEventObject mapEvent = new GameObject().AddComponent<MapEventObject>();

                mapEvent.SetUp(obj, this);

                Events.Add(mapEvent);
                mapEvent.transform.SetParent(_objectsParent);
            }
        }

        /// <summary>
        /// 範囲外を指定している
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsOutOfRange(int x, int y)
        {
            bool result = false;

            if (x < 0 || x >= Width ||
                y < 0 || y >= Height)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// マップイベントオブジェクトを取得する
        /// </summary>
        /// <param name="position">位置</param>
        /// <returns></returns>
        public MapEventObject GetMapEventObject(Vector2Int position)
        {
            return Events.FirstOrDefault(x => x.Position == position && x.Exists());
        }

        /// <summary>
        /// マップイベントオブジェクトを取得する
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="trigger">トリガー</param>
        /// <returns></returns>
        public MapEventObject GetMapEventObject(Vector2Int position, MapEventTriggerType trigger)
        {
            return Events
                .Where(x => x.Trigger == trigger)
                .FirstOrDefault(x => x.Position == position && x.Exists());
        }

        /// <summary>
        /// マップイベントオブジェクトの取得を試みる
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="trigger">トリガー</param>
        /// <param name="mapEventObject">マップイベントオブジェクト</param>
        /// <returns></returns>
        public bool TryGetMapEventObject(Vector2Int position, MapEventTriggerType trigger, out MapEventObject mapEventObject)
        {
            mapEventObject = GetMapEventObject(position, trigger);
            return mapEventObject;
        }

        /// <summary>
        /// マップイベントオブジェクトを取得する
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="trigger">トリガー</param>
        /// <param name="canCollide">他のオブジェクトと衝突可能か</param>
        /// <returns></returns>
        public MapEventObject GetMapEventObject(Vector2Int position, MapEventTriggerType trigger, bool canCollide)
        {
            return Events
                .Where(x => x.Trigger == trigger)
                .FirstOrDefault(x => x.Position == position && x.Exists() && x.CanCollide == canCollide);
        }

        /// <summary>
        /// マップイベントオブジェクトを取得する
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="trigger"><トリガー/param>
        /// <param name="canCollide">他のオブジェクトと衝突可能か</param>
        /// <param name="mapEventObject">マップイベントオブジェクト</param>
        /// <returns></returns>
        public bool TryGetMapEventObject(Vector2Int position, MapEventTriggerType trigger, bool canCollide, out MapEventObject mapEventObject)
        {
            mapEventObject = GetMapEventObject(position, trigger, canCollide);
            return mapEventObject;
        }

        /// <summary>
        /// プレイヤーの開始位置と方向を取得する
        /// </summary>
        /// <returns></returns>
        public (Vector2Int position, Vector2Int direction) GetPlayerStartPositionAndDirection()
        {
            Vector2Int position = Vector2Int.zero;
            Vector2Int direcition = Vector2Int.up;

            MapEventType mapEventObjectType = VariableData.PlayerInitialPosition switch
            {
                PlayerInitialPositionType.Start => MapEventType.Start,
                PlayerInitialPositionType.Goal => MapEventType.Goal,
                _ => MapEventType.Start
            };

            MapEventObject start = Events.FirstOrDefault(x => x.Type == mapEventObjectType);
            if (start)
            {
                position = start.Position + start.Direction;
                direcition = start.Direction;
            }

            return (position, direcition);
        }

        /// <summary>
        /// セルを取得する
        /// </summary>
        /// <param name="position">位置</param>
        /// <returns></returns>
        public MapCell GetCell(Vector2Int position)
        {
            return GetCell(position.x, position.y);
        }

        /// <summary>
        /// セルを取得する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public MapCell GetCell(int x, int y)
        {
            MapCell cell = null;

            if (!IsOutOfRange(x, y))
            {
                cell = Cells[y * Height + x];
            }

            return cell;
        }

        /// <summary>
        /// 指定したセルの周囲にあるセルを配列にして取得する
        /// </summary>
        /// <param name="cell">セル</param>
        /// <returns></returns>
        public MapCell[] GetSurroundingCells(MapCell cell)
        {
            return GetSurroundingCells(cell.Position.x, cell.Position.y);
        }


        /// <summary>
        /// 指定した位置の周囲にあるセルを配列にして取得する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public MapCell[] GetSurroundingCells(int x, int y)
        {
            List<MapCell> cellList = new();

            for (int row = y - 1; row <= y + 1; row++)
            {
                for (int column = x - 1; column <= x + 1; column++)
                {
                    // 自身または配列範囲外は取得しない
                    if (column == x && row == y || IsOutOfRange(column, row))
                    {
                        continue;
                    }

                    MapCell value = GetCell(column, row);
                    cellList.Add(value);
                }
            }

            MapCell[] cellArray = cellList.ToArray();
            return cellArray;
        }

        /// <summary>
        /// 索引を座標に変える
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector2Int ToPosition(int index)
        {
            int x = index % Width;
            int y = index / Width;
            Vector2Int positon = new(x, y);

            return positon;
        }

        /// <summary>
        /// 索引から位置を求める
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3Int ToWorldPosition(int index)
        {
            int x = index % Width;
            int z = -index / Width;
            Vector3Int positon = new(x, 0, z);

            return positon;
        }

        /// <summary>
        /// セルから位置を求める
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public Vector3Int ToWorldPosition(MapCell cell)
        {
            int x = cell.Position.x;
            int z = -cell.Position.y;
            Vector3Int positon = new(x, 0, z);

            return positon;
        }

        /// <summary>
        /// マップイベントオブジェクトのプレハブを取得する
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject GetMapEventObjectPrefab(MapEventObjectType type)
        {
            return type switch
            {
                MapEventObjectType.StairsUp => _stairsUp,
                MapEventObjectType.StairsDown => _StairsDown,
                _ => null
            };
        }
    }
}
