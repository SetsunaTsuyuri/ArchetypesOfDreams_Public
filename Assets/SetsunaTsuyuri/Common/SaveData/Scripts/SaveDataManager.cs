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
        static readonly int s_maxSaves = 3;

        /// <summary>
        /// セーブデータのパス
        /// </summary>
        static readonly string s_saveDataPath = Application.persistentDataPath;

        /// <summary>
        /// セーブデータの名前
        /// </summary>
        static readonly string s_saveDataName = "savedata";

        /// <summary>
        /// オートセーブデータの名前
        /// </summary>
        static readonly string s_autoSaveDataName = "autosavedata";

        /// <summary>
        /// システムデータの名前
        /// </summary>
        static readonly string s_systemDataName = "systemdata";

        /// <summary>
        /// セーブデータディクショナリー
        /// </summary>
        readonly Dictionary<int, SaveData> _saveDataArray = new();

        /// <summary>
        /// セーブデータディクショナリー
        /// </summary>
        public static Dictionary<int, SaveData> SaveDataDic
        {
            get => Instance._saveDataArray;
        }

        /// <summary>
        /// システムデータ
        /// </summary>
        SystemData _systemData = new();

        /// <summary>
        /// システムデータ
        /// </summary>
        public static SystemData SystemData
        {
            get => Instance._systemData;
            set => Instance._systemData = value;
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

        /// <summary>
        /// システムデータのパス
        /// </summary>
        public static string SystemDataPath => $"{s_saveDataPath}/{s_systemDataName}.dat";

        /// <summary>
        /// オートセーブデータのパス
        /// </summary>
        /// <returns></returns>
        public static string AutoSaveDataPath => $"{s_saveDataPath}/{s_autoSaveDataName}.dat";

        /// <summary>
        /// 全てのセーブデータのパス
        /// </summary>
        /// <returns></returns>
        public static string[] SaveDataPathes => Directory.GetFiles(s_saveDataPath, $"{s_saveDataName}_*.dat");

        public override void Initialize()
        {
            // システムデータ
            if (TryImport(SystemDataPath, out byte[] system))
            {
                SystemData = Decrypt<SystemData>(system);
            }

            // オートセーブデータ
            if (TryImport(AutoSaveDataPath, out byte[] autoSave))
            {
                AutoSaveData = Decrypt<SaveData>(autoSave);
            }

            // セーブデータ
            SaveDataDic.Clear();
            for (int i = 1; i <= s_maxSaves; i++)
            {
                SaveDataDic.Add(i, null);
            }

            foreach (var path in SaveDataPathes)
            {
                if (TryImport(path, out byte[] save))
                {
                    SaveData saveData = Decrypt<SaveData>(save);
                    SaveDataDic[saveData.Id] = saveData;
                }
            }

            SystemData.Load();
        }

        /// <summary>
        /// セーブデータのパスを取得する
        /// </summary>
        /// <param name="id">セーブデータID</param>
        /// <returns></returns>
        private static string GetSaveDataPath(int id)
        {
            return $"{s_saveDataPath}/{s_saveDataName}_{id}.dat";
        }

        /// <summary>
        /// セーブする
        /// </summary>
        /// <param name="id"></param>
        public static void Save(int id)
        {
            SaveDataDic[id] ??= new();

            SaveData saveData = SaveDataDic[id];
            saveData.Save(id);

            byte[] bytes = Encrypt(saveData);
            string path = GetSaveDataPath(id);
            Export(bytes, path);
        }

        /// <summary>
        /// オートセーブを行う
        /// </summary>
        public static void AutoSave()
        {
            AutoSaveData ??= new();

            AutoSaveData.Save();

            byte[] bytes = Encrypt(AutoSaveData);
            Export(bytes, AutoSaveDataPath);
        }

        /// <summary>
        /// システムデータをセーブする
        /// </summary>
        public static void SaveSystemData()
        {
            SystemData.Save();

            byte[] bytes = Encrypt(SystemData);
            Export(bytes, SystemDataPath);
        }

        /// <summary>
        /// ロードする
        /// </summary>
        /// <param name="id"></param>
        public static void Load(int id)
        {
            SaveDataDic[id].Load();
        }

        /// <summary>
        /// オートセーブをロードする
        /// </summary>
        /// <param name="id"></param>
        public static void LoadAutoSaveData()
        {
            AutoSaveData.Load();
        }

        /// <summary>
        /// インポートを試みる
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        private static bool TryImport(string path, out byte[] bytes)
        {
            if (!File.Exists(path))
            {
                bytes = null;
                return false;
            }

            bytes = File.ReadAllBytes(path);
            return true;
        }

        /// <summary>
        /// エクスポートする
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        private static void Export(byte[] bytes, string path)
        {
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// オブジェクトからバイト配列を作る
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] ToBytes(object data)
        {
            string json = JsonUtility.ToJson(data);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }

        /// <summary>
        /// バイト配列からオブジェクトを作る
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static T FromBytes<T>(byte[] bytes)
        {
            string json = Encoding.UTF8.GetString(bytes);
            T data = JsonUtility.FromJson<T>(json);
            return data;
        }

        /// <summary>
        /// 暗号化する
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] Encrypt(object data)
        {
            byte[] bytes = ToBytes(data);
            byte[] encrypted = CryptographyUtility.Encrypt(bytes);
            return encrypted;
        }

        /// <summary>
        /// 復号する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        private static T Decrypt<T>(byte[] encrypted)
        {
            byte[] bytes = CryptographyUtility.Decrypt(encrypted);
            T data = FromBytes<T>(bytes);
            return data;
        }
    }
}
