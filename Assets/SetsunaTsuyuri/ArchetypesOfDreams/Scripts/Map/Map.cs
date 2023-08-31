using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Unity.Mathematics;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マップ
    /// </summary>
    public class Map : MonoBehaviour
    {
        /// <summary>
        /// エンカウントエリアIDオフセット
        /// </summary>
        static readonly int s_encounterAreaIdOffset = -30;

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
        /// ナイトメア
        /// </summary>
        [SerializeField]
        GameObject _nightmare = null;

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
        /// ランダムエンカウントエリアID配列
        /// </summary>
        public int[,] RandomEncounterAreas { get; private set; } = { };

        /// <summary>
        /// セル2次元配列
        /// </summary>
        public MapCell[,] Cells { get; private set; } = { };

        /// <summary>
        /// イベントオブジェクトリスト
        /// </summary>
        public List<MapEventObject> Events { get; private set; } = new();

        /// <summary>
        /// プレイヤー
        /// </summary>
        public Player Player { get; private set; } = null;

        /// <summary>
        /// セルの大きさ
        /// </summary>
        public int CellSize { get; private set; } = 0;

        private void Awake()
        {
            // プレイヤー
            Player = _objectsParent.GetComponentInChildren<Player>();
        }

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

            // エンカウントエリアを作る
            CreateEncounterAreas(jObject);

            // セルを作る
            CreateCells(jObject);
            CreateCellGameObjects();

            // マップイベントを作る
            CreateEvents(jObject);
        }

        /// <summary>
        /// エンカウントエリア配列を作る
        /// </summary>
        /// <param name="map"></param>
        private void CreateEncounterAreas(JToken map)
        {
            JToken ids = map["layers"].FirstOrDefault(x => x["name"].Value<string>() == "encounter");
            if (ids is null)
            {
                return;
            }

            RandomEncounterAreas = ids["data"].ToObject<int[]>()
                .Select(x => Mathf.Max(x + s_encounterAreaIdOffset, 0))
                .ToArray()
                .ToArray2D(Height, Width);
        }

        /// <summary>
        /// セルを作る
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        private void CreateCells(JToken jToken)
        {
            MapCellType[][,] cellTypesArray = CreateCellTypes(jToken);

            Cells = new MapCell[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    MapCell cell = new()
                    {
                        Position = new Vector2Int(j, i),
                        Types = new MapCellType[cellTypesArray.Length]
                    };

                    for (int k = 0; k < cellTypesArray.Length; k++)
                    {
                        cell.Types[k] = cellTypesArray[k][i, j];
                    }

                    Cells[i, j] = cell;
                }
            }
        }

        /// <summary>
        /// セルのゲームオブジェクトを作る
        /// </summary>
        private void CreateCellGameObjects()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    MapCell cell = Cells[i, j];
                    if (cell.IsNone)
                    {
                        continue;
                    }

                    GameObject prefab = cell switch
                    {
                        MapCell c when c.Types.Any(x => x == MapCellType.Floor) => _floorPrefab,
                        MapCell c when c.Types.Any(x => x == MapCellType.Wall) => _wallPrefab,
                        _ => null
                    };

                    if (prefab)
                    {
                        Vector3Int position = ToWorldPosition(cell);
                        Instantiate(prefab, position, Quaternion.identity, _cellsParent);
                    }
                }
            }
        }

        /// <summary>
        /// セルタイプ2次元配列の配列を作る
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        private MapCellType[][,] CreateCellTypes(JToken jToken)
        {
            IEnumerable<JToken> tileLayers = jToken["layers"]
                .Where(x => x["name"].Value<string>() == "cell")
                .Where(x => x["type"].Value<string>() == "tilelayer");

            MapCellType[][,] tileCellsArray = new MapCellType[tileLayers.Count()][,];
            for (int i = 0; i < tileCellsArray.Length; i++)
            {
                MapCellType[,] cells = tileLayers.ElementAt(i)["data"]
                    .ToObject<MapCellType[]>().ToArray2D(Height, Width);

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
                MapEventObject mapEvent = new GameObject()
                    .AddComponent<MapEventObject>();

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
        /// マップオブジェクトを取得する
        /// </summary>
        /// <param name="position">位置</param>
        /// <returns></returns>
        public MapObject GetMapObject(Vector2Int position)
        {
            // プレイヤー
            if (Player.Position == position)
            {
                return Player;
            }

            // イベント
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
                cell = Cells[y, x];
            }

            return cell;
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
                MapEventObjectType.Nightmare => _nightmare,
                _ => null
            };
        }
    }
}
