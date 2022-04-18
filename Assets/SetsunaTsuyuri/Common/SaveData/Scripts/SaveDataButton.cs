using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using SetsunaTsuyuri.ArchetypesOfDreams;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// セーブデータボタン
    /// </summary>
    public class SaveDataButton : GameButton
    {
        /// <summary>
        /// セーブデータメニュー
        /// </summary>
        public SaveDataCommandType SaveDataMenu { get; set; } = SaveDataCommandType.Save;

        /// <summary>
        /// セーブデータID
        /// </summary>
        public int SaveDataId { get; set; } = 0;

        /// <summary>
        /// オートセーブスロットである
        /// </summary>
        public bool IsAutoSave { get; set; } = false;

        /// <summary>
        /// IDテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI idText = null;

        /// <summary>
        /// 日付・時間テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI dateTimeText = null;

        /// <summary>
        /// フッターテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI footer = null;

        /// <summary>
        /// 顔グラフィック配列
        /// </summary>
        [SerializeField]
        Image[] alliesFace = { };

        protected override void Awake()
        {
            base.Awake();

            AddOnClickListener(() =>
            {
                switch (SaveDataMenu)
                {
                    // セーブ
                    case SaveDataCommandType.Save:
                        SaveDataManager.Save(SaveDataId);
                        UpdateTexts(SaveDataManager.CurrentSaveData);
                        UpdateImages(SaveDataManager.CurrentSaveData);
                        break;

                    // ロード
                    case SaveDataCommandType.Load:

                        if (IsAutoSave)
                        {
                            SaveDataManager.LoadAuto();
                        }
                        else
                        {
                            SaveDataManager.Load(SaveDataId);
                        }
                        SceneChangeManager.ChangeScene(ScenesName.MyRoom);
                        break;
                }
            });
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="command">セーブデータコマンド</param>
        /// <param name="id">セーブデータID</param>
        /// <param name="isAutoSave">オートセーブ</param>
        public void SetUp(SaveDataCommandType command, int id = 0, bool isAutoSave = false)
        {
            SaveDataMenu = command;
            SaveDataId = id;
            IsAutoSave = isAutoSave;

            // セーブデータ
            SaveData save = isAutoSave switch
            {
                true => SaveDataManager.AutoSaveData,
                false => SaveDataManager.Saves[id]
            };

            // テキストを更新する
            UpdateTexts(save, isAutoSave);

            // イメージを更新する
            UpdateImages(save);

            // 空きデータはロードできないようにする
            if (save is null && SaveDataMenu == SaveDataCommandType.Load)
            {
                SetInteractable(false);
            }

            // オートセーブ枠はセーブできないようにする
            if (SaveDataMenu == SaveDataCommandType.Save &&
                IsAutoSave)
            {
                SetInteractable(false);
            }
        }

        /// <summary>
        /// テキストを更新する
        /// </summary>
        /// <param name="save">セーブデータ</param>
        /// <param name="isAutoSave">オートセーブ</param>
        private void UpdateTexts(SaveData save, bool isAutoSave = false)
        {
            idText.text = string.Empty;
            footer.text = string.Empty;

            if (isAutoSave)
            {
                idText.text += "Auto Save ";
            }
            else
            {
                idText.text += $"No.{SaveDataId + 1} ";
            }

            // セーブデータが存在する場合
            if (save != null)
            {
                System.DateTime dateTime = System.DateTime.Parse(save.DateTime);
                dateTimeText.text = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                footer.text = "No Data";
            }
        }

        /// <summary>
        /// イメージを更新する
        /// </summary>
        /// <param name="saveData">セーブデータ</param>
        private void UpdateImages(SaveData saveData)
        {
            // 各顔グラフィックを全て非表示にする
            foreach (var face in alliesFace)
            {
                face.enabled = false;
            }

            // セーブデータが存在する場合
            if (saveData != null)
            {
                // 味方全員のスプライト
                Sprite[] sprites = saveData.Allies
                    .Concat(saveData.ReserveAllies)
                    .Select((x) => x.GetData().GetFaceSpriteOrSprite())
                    .ToArray();

                // スプライトが存在する場合のみ表示する
                for (int i = 0; i < alliesFace.Length && i < sprites.Length; i++)
                {
                    Image image = alliesFace[i];
                    Sprite sprite = sprites[i];

                    image.sprite = sprites[i];
                    if (sprite)
                    {
                        image.enabled = true;
                    }
                }
            }
        }
    }
}
