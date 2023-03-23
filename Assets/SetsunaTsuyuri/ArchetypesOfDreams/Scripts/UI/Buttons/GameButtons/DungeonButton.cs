using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダンジョンボタン
    /// </summary>
    public class DungeonButton : GameButton
    {
        /// <summary>
        /// 名前の表示
        /// </summary>
        public TextMeshProUGUI NameText { get; private set; } = null;

        /// <summary>
        /// ダンジョンデータ
        /// </summary>
        public DungeonData Data { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            NameText = GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="dungeon">ダンジョンデータ</param>
        /// <param name="description">説明文UIの管理者</param>
        public void SetUp(DungeonData dungeon, DescriptionUI description)
        {
            // ダンジョンデータ
            Data = dungeon;

            // 名前の表示
            NameText.text = dungeon.Name;

            // 選択されたとき、説明文を設定する
            AddTrriger(EventTriggerType.Select, (_) => description.SetText(Data.Description));

            // キャンセルされたとき、自室シーンに移る
            AddTrriger(EventTriggerType.Cancel, (_) => SceneChangeManager.StartChange(SceneId.MyRoom));
            
            // 押されたとき、ダンジョンシーンに移行する
            AddPressedListener(() =>
            {
                VariableData.DungeonId = Data.Id;
                SceneChangeManager.StartChange(SceneId.Dungeon);
            });
        }
    }
}
