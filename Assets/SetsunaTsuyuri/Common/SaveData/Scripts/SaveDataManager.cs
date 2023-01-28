using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// セーブデータの管理者
    /// </summary>
    public class SaveDataManager : Singleton<SaveDataManager>, IInitializable
    {
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
        readonly SaveData[] _saves = new SaveData[MaxSaves];

        /// <summary>
        /// セーブデータ配列
        /// </summary>
        public static SaveData[] Saves
        {
            get => Instance._saves;
        }

        /// <summary>
        /// オートセーブデータ
        /// </summary>
        SaveData _autoSaveData = null;

        /// <summary>
        /// オートセーブデータ
        /// </summary>
        public static SaveData AutoSaveData
        {
            get => Instance._autoSaveData;
            set => Instance._autoSaveData = value;
        }

        public override void Initialize()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // オートセーブデータ
            AutoSaveData = LoadFromPlayerPrefs(AutoSaveDataName);
            
            // セーブデータ
            for (int i = 0; i < Saves.Length; i++)
            {
                string key = GetSaveDataKey(i);
                SaveData saveData = LoadFromPlayerPrefs(key);
                Saves[i] = saveData;
            }
#else
            // オートセーブデータ
            string autoSaveDataPath = GetAutoSaveDataPath();
            AutoSaveData = Import(autoSaveDataPath);

            // セーブデータ
            string[] saveDataPathes = GetSaveDataPathes();
            foreach (var path in saveDataPathes)
            {
                SaveData saveData = Import(path);
                Saves[saveData.Id] = saveData;
            }
#endif
        }

        /// <summary>
        /// セーブする
        /// </summary>
        /// <param name="id">セーブデータID</param>
        public static void Save(int id)
        {
            if (Saves[id] is null)
            {
                Saves[id] = new();
            }

            SaveData saveData = Saves[id];
            saveData.Save(id);

#if UNITY_WEBGL && !UNITY_EDITOR
            string key = GetSaveDataKey(id);
            SaveInPlayerPrefs(saveData, key);
#else
            string path = GetSaveDataPath(id);
            Export(saveData, path);
#endif
        }

        /// <summary>
        /// オートセーブする
        /// </summary>
        public static void SaveAuto()
        {
            AutoSaveData.Save();

#if UNITY_WEBGL && !UNITY_EDITOR
            SaveInPlayerPrefs(AutoSaveData, AutoSaveDataName);
#else
            string path = GetAutoSaveDataPath();
            Export(AutoSaveData, path);
#endif
        }

        /// <summary>
        /// ロードする
        /// </summary>
        /// <param name="id">セーブデータID</param>
        public static void Load(int id)
        {
            Saves[id].Load();
        }

        /// <summary>
        /// オートセーブをロードする
        /// </summary>
        public static void LoadAuto()
        {
            AutoSaveData.Load();
        }

        /// <summary>
        /// セーブデータを出力する
        /// </summary>
        /// <param name="saveData">セーブデータ</param>
        /// <param name="path">出力先</param>
        private static void Export(SaveData saveData, string path)
        {
            // 暗号化
            byte[] encrypted = Encrypt(saveData);

            // 出力
            File.WriteAllBytes(path, encrypted);
        }

        /// <summary>
        /// セーブデータを暗号化する
        /// </summary>
        /// <param name="saveData">セーブデータ</param>
        /// <returns></returns>
        private static byte[] Encrypt(SaveData saveData)
        {
            string json = JsonUtility.ToJson(saveData);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            byte[] encrypted = CryptographyUtility.Encrypt(bytes);
            return encrypted;
        }

        /// <summary>
        /// セーブデータを取り込む
        /// </summary>
        /// <param name="path">セーブデータのパス</param>
        /// <returns></returns>
        private static SaveData Import(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            // 復号
            byte[] encrypted = File.ReadAllBytes(path);
            SaveData saveData = Decrypt(encrypted);
            return saveData;
        }

        /// <summary>
        /// セーブデータを復号する
        /// </summary>
        /// <param name="encrypted">暗号化されたセーブデータ</param>
        /// <returns></returns>
        private static SaveData Decrypt(byte[] encrypted)
        {
            // 復号
            byte[] decrypted = CryptographyUtility.Decrypt(encrypted);

            // セーブデータ
            string json = Encoding.UTF8.GetString(decrypted);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            return saveData;
        }

        /// <summary>
        /// 全てのセーブデータ(json)のパスを取得する
        /// </summary>
        /// <returns></returns>
        private static string[] GetSaveDataPathes()
        {
            return Directory.GetFiles(SaveDataPath, $"{SaveDataName}_*.dat");
        }

        /// <summary>
        /// セーブデータのパスを取得する
        /// </summary>
        /// <param name="id">セーブデータID</param>
        /// <returns></returns>
        private static string GetSaveDataPath(int id)
        {
            return $"{SaveDataPath}/{SaveDataName}_{id}.dat";
        }

        /// <summary>
        /// オートセーブデータのパスを取得する
        /// </summary>
        /// <returns></returns>
        private static string GetAutoSaveDataPath()
        {
            return $"{SaveDataPath}/{AutoSaveDataName}.dat";
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>
        /// セーブデータをPlayerPrefsに保存する
        /// </summary>
        /// <param name="saveData">セーブデータ</param>
        /// <param name="key">キー</param>
        private static void SaveInPlayerPrefs(SaveData saveData, string key)
        {
            // 暗号化
            byte[] encrypted = Encrypt(saveData);

            // 保存
            string value = Encoding.UTF8.GetString(encrypted);
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// PlayerPrefsからセーブデータを取り込む
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns></returns>
        private static SaveData LoadFromPlayerPrefs(string key)
        {
            SaveData saveData = null;

            string value = PlayerPrefs.GetString(key);
            if (value != string.Empty)
            {
                // 復号
                byte[] encrypted = Encoding.UTF8.GetBytes(value);
                saveData = Decrypt(encrypted);
            }

            return saveData;
        }

        /// <summary>
        /// セーブデータのPlayerPrefsキーを取得する
        /// </summary>
        /// <param name="id">セーブデータID</param>
        /// <returns></returns>
        private static string GetSaveDataKey(int id)
        {
            return $"{SaveDataName}_{id}";
        }
#endif
    }
}
