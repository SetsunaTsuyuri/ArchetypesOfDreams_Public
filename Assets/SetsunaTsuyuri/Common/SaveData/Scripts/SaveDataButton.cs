using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
        SaveDataCommandType _commandType = SaveDataCommandType.Save;

        /// <summary>
        /// セーブデータID
        /// </summary>
        int _id = 0;

        /// <summary>
        /// オートセーブスロットである
        /// </summary>
        bool _isAutoSave = false;

        /// <summary>
        /// IDテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _idText = null;

        /// <summary>
        /// 日付・時間テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _dateTimeText = null;

        /// <summary>
        /// フッターテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _footer = null;

        /// <summary>
        /// キャラクターイメージの親トランスフォーム
        /// </summary>
        [SerializeField]
        RectTransform _characterImagesRoot = null;

        /// <summary>
        /// キャラクターイメージ配列
        /// </summary>
        Image[] _characterImages = { };

        protected override void Awake()
        {
            base.Awake();

            AddPressedListener(() =>
            {
                switch (_commandType)
                {
                    // セーブ
                    case SaveDataCommandType.Save:
                        SaveDataManager.Save(_id);
                        UpdateTexts(SaveDataManager.SaveDataDictionary[_id]);
                        UpdateImages(SaveDataManager.SaveDataDictionary[_id]);
                        break;

                    // ロード
                    case SaveDataCommandType.Load:

                        if (_isAutoSave)
                        {
                            SaveDataManager.LoadAutoSaveData();
                        }
                        else
                        {
                            SaveDataManager.Load(_id);
                        }
                        SceneChangeManager.StartChange(SceneId.MyRoom);
                        break;
                }
            });
        }

        /// <summary>
        /// キャラクターイメージを作る
        /// </summary>
        /// <param name="prefab">プレハブ</param>
        /// <param name="number">作る数</param>
        public void GenerateCharacterImages(Image prefab, int number)
        {
            _characterImages = new Image[number];
            for (int i = 0; i < number; i++)
            {
                _characterImages[i] = Instantiate(prefab, _characterImagesRoot);
            }
        }

        /// <summary>
        /// 更新する
        /// </summary>
        /// <param name="command">セーブデータコマンド</param>
        /// <param name="id">セーブデータID</param>
        /// <param name="isAutoSave">オートセーブ</param>
        public void UpdateButton(SaveDataCommandType command, int id, bool isAutoSave)
        {
            _commandType = command;
            _id = id;
            _isAutoSave = isAutoSave;

            // セーブデータ
            SaveData saveData = null;
            if (_isAutoSave)
            {
                saveData = SaveDataManager.AutoSaveData;
            }
            else if (SaveDataManager.SaveDataDictionary.TryGetValue(id, out SaveData value))
            {
                saveData = value;
            }

            // テキストを更新する
            UpdateTexts(saveData, isAutoSave);

            // イメージを更新する
            UpdateImages(saveData);

            // 空きデータはロードできないようにする
            if (saveData is null && _commandType == SaveDataCommandType.Load)
            {
                SetInteractable(false);
            }

            // オートセーブ枠はセーブできないようにする
            if (_commandType == SaveDataCommandType.Save
                && _isAutoSave)
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
            _idText.text = string.Empty;
            _footer.text = string.Empty;

            if (isAutoSave)
            {
                _idText.text += "Auto Save ";
            }
            else
            {
                _idText.text += $"No.{_id}";
            }

            if (save != null)
            {
                System.DateTime dateTime = System.DateTime.Parse(save.DateTime);
                _dateTimeText.text = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                _footer.text = "No Data";
            }
        }

        /// <summary>
        /// イメージを更新する
        /// </summary>
        /// <param name="saveData">セーブデータ</param>
        private void UpdateImages(SaveData saveData)
        {
            foreach (var image in _characterImages)
            {
                image.enabled = false;
            }

            // セーブデータが存在する場合
            if (saveData != null)
            {
                var combatants = saveData.Allies
                    .Concat(saveData.ReserveAllies)
                    .ToArray();

                // スプライトロード
                foreach (var combatant in combatants)
                {
                    combatant.LoadSprites();
                }

                Sprite[] sprites = combatants
                    .Select(x => x.GetFaceSpriteOrSprite())
                    .ToArray();

                // スプライトが存在する場合のみ表示する
                for (int i = 0; i < _characterImages.Length && i < sprites.Length; i++)
                {
                    Image image = _characterImages[i];
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
