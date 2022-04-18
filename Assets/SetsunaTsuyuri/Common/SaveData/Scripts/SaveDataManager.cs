using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// セーブデータの管理者(シングルトン)
    /// </summary>
    public class SaveDataManager : Singleton<SaveDataManager>, IInitializable
    {
        /// <summary>
        /// オートセーブ可能な数
        /// </summary>
        public static readonly int AutoSaves = 1;

        /// <summary>
        /// セーブ可能な数
        /// </summary>
        public static readonly int MaxSaves = 3;

        /// <summary>
        /// セーブデータのパス
        /// </summary>
        public static readonly string SaveDataPath = Application.persistentDataPath;

        /// <summary>
        /// セーブデータの名前
        /// </summary>
        public static readonly string SaveDataName = "save";

        /// <summary>
        /// オートセーブデータの名前
        /// </summary>
        public static readonly string AutoSaveDataName = "autosave";

        /// <summary>
        /// セーブデータ配列
        /// </summary>
        readonly SaveData[] saves = new SaveData[MaxSaves];

        /// <summary>
        /// セーブデータ配列
        /// </summary>
        public static SaveData[] Saves
        {
            get => Instance.saves;
        }

        /// <summary>
        /// オートセーブデータ
        /// </summary>
        SaveData autoSaveData = null;

        /// <summary>
        /// オートセーブデータ
        /// </summary>
        public static SaveData AutoSaveData
        {
            get => Instance.autoSaveData;
            set => Instance.autoSaveData = value;
        }

        /// <summary>
        /// 現在のセーブデータ
        /// </summary>
        SaveData currentSaveData = new SaveData();

        /// <summary>
        /// 現在のセーブデータ
        /// </summary>
        public static SaveData CurrentSaveData
        {
            get => Instance.currentSaveData;
            private set => Instance.currentSaveData = value;
        }

        public override void Initialize()
        {
            ImportSaves();
        }

        /// <summary>
        /// json形式のセーブデータを取り込む
        /// </summary>
        private static void ImportSaves()
        {
            // オートセーブデータ
            string autoSaveDataPath = GetAutoSaveDataJsonPath();
            if (File.Exists(autoSaveDataPath))
            {
                AutoSaveData = ImportSave(autoSaveDataPath);
            }

            // セーブデータ
            string[] saveDataPathes = GetSaveDataJsonPathes();
            foreach (var path in saveDataPathes)
            {
                SaveData save = ImportSave(path);
                Saves[save.Id] = save;
            }
        }

        /// <summary>
        /// json形式のセーブデータを復元する
        /// </summary>
        /// <param name="path">セーブデータのパス</param>
        /// <returns></returns>
        private static SaveData ImportSave(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            string json = Encoding.UTF8.GetString(bytes);
            SaveData save = JsonUtility.FromJson<SaveData>(json);

            return save;
        }

        /// <summary>
        /// セーブする
        /// </summary>
        /// <param name="id">セーブデータID</param>
        public static void Save(int id)
        {
            CurrentSaveData.Save(id);

            string path = GetSaveDataJsonPath(id);
            SaveData copy = ExportAndCopySaveData(path);
            Saves[id] = copy;
        }

        /// <summary>
        /// オートセーブする
        /// </summary>
        public static void SaveAuto()
        {
            CurrentSaveData.Save();

            string path = GetAutoSaveDataJsonPath();
            SaveData copy = ExportAndCopySaveData(path);
            AutoSaveData = copy;
        }

        /// <summary>
        /// セーブデータをJson形式で書き出し、そのディープコピーを作る
        /// </summary>
        /// <param name="path">json形式で書き出す際のパス</param>
        /// <param name="id">ID</param>
        /// <returns>セーブデータ(ディープコピー)</returns>
        private static SaveData ExportAndCopySaveData(string path)
        {
            // セーブデータをjson形式で書き出す
            string json = JsonUtility.ToJson(CurrentSaveData);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            File.WriteAllBytes(path, bytes);

            // 書き出したjsonから新しくセーブデータを作る(ディープコピー)
            SaveData copy = JsonUtility.FromJson<SaveData>(json);
            return copy;
        }

        /// <summary>
        /// ロードする
        /// </summary>
        /// <param name="id">セーブデータID</param>
        public static void Load(int id)
        {
            CurrentSaveData = Saves[id];
        }

        /// <summary>
        /// オートセーブをロードする
        /// </summary>
        public static void LoadAuto()
        {
            CurrentSaveData = AutoSaveData;
        }

        /// <summary>
        /// 全てのセーブデータ(json)のパスを取得する
        /// </summary>
        /// <returns></returns>
        private static string[] GetSaveDataJsonPathes()
        {
            return Directory.GetFiles(SaveDataPath, $"{SaveDataName}_*.json");
        }

        /// <summary>
        /// セーブデータ(json)のパスを取得する
        /// </summary>
        /// <param name="id">セーブデータID</param>
        /// <returns></returns>
        private static string GetSaveDataJsonPath(int id)
        {
            return $"{SaveDataPath}/{SaveDataName}_{id}.json";
        }

        /// <summary>
        /// オートセーブデータ(json)のパスを取得する
        /// </summary>
        /// <returns></returns>
        private static string GetAutoSaveDataJsonPath()
        {
            return $"{SaveDataPath}/{AutoSaveDataName}.json";
        }
    }
}
