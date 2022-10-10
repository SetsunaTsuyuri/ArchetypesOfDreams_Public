using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
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

        static readonly int s_blockSize = 128;
        static readonly int s_keySize = 128;
        static readonly string s_iv = "1K9hf715zU8sm59H";
        static readonly string s_key = "R18FaEzPyxdv0WwW";

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
                AutoSaveData = Import(autoSaveDataPath);
            }

            // セーブデータ
            string[] saveDataPathes = GetSaveDataJsonPathes();
            foreach (var path in saveDataPathes)
            {
                SaveData save = Import(path);
                Saves[save.Id] = save;
            }
        }

        /// <summary>
        /// json形式のセーブデータを復元する
        /// </summary>
        /// <param name="path">セーブデータのパス</param>
        /// <returns></returns>
        private static SaveData Import(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            // 複合する
            bytes = Decrypt(bytes);

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
            if (Saves[id] is null)
            {
                Saves[id] = new();
            }

            SaveData saveData = Saves[id];
            saveData.Save(id);

            string path = GetSaveDataJsonPath(id);
            Export(saveData, path);
        }

        /// <summary>
        /// オートセーブする
        /// </summary>
        public static void SaveAuto()
        {
            AutoSaveData.Save();

            string path = GetAutoSaveDataJsonPath();
            Export(AutoSaveData, path);
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
        /// セーブデータをJson形式で書き出す
        /// </summary>
        /// <param name="saveData">セーブデータ</param>
        /// <param name="path">json形式で書き出すパス</param>
        private static void Export(SaveData saveData, string path)
        {
            string json = JsonUtility.ToJson(saveData);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            // 暗号化する
            bytes = Encrypt(bytes);

            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// 全てのセーブデータ(json)のパスを取得する
        /// </summary>
        /// <returns></returns>
        private static string[] GetSaveDataJsonPathes()
        {
            return Directory.GetFiles(SaveDataPath, $"{SaveDataName}_*.dat");
        }

        /// <summary>
        /// セーブデータ(json)のパスを取得する
        /// </summary>
        /// <param name="id">セーブデータID</param>
        /// <returns></returns>
        private static string GetSaveDataJsonPath(int id)
        {
            return $"{SaveDataPath}/{SaveDataName}_{id}.dat";
        }

        /// <summary>
        /// オートセーブデータ(json)のパスを取得する
        /// </summary>
        /// <returns></returns>
        private static string GetAutoSaveDataJsonPath()
        {
            return $"{SaveDataPath}/{AutoSaveDataName}.dat";
        }

        /// <summary>
        /// 暗号化する
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static byte[] Encrypt(byte[] bytes)
        {
            AesManaged managed = CreateAesManaged();
            ICryptoTransform encryptor = managed.CreateEncryptor();
            byte[] encrypted = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return encrypted;
        }

        /// <summary>
        /// 複合する
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static byte[] Decrypt(byte[] bytes)
        {
            AesManaged managed = CreateAesManaged();
            ICryptoTransform decryptor = managed.CreateDecryptor();
            byte[] decrypted = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return decrypted;
        }

        /// <summary>
        /// AesManagedを作る
        /// </summary>
        /// <returns></returns>
        private static AesManaged CreateAesManaged()
        {
            AesManaged managed = new()
            {
                BlockSize = s_blockSize,
                KeySize = s_keySize,
                IV = Encoding.UTF8.GetBytes(s_iv),
                Key = Encoding.UTF8.GetBytes(s_key),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            return managed;
        }
    }
}
