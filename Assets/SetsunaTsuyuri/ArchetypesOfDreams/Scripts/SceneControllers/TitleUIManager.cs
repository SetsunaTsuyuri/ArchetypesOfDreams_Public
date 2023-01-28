using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// タイトルシーンUIの管理者
    /// </summary>
    public class TitleUIManager : UIManager
    {
        /// <summary>
        /// タイトルメニュー
        /// </summary>
        public TitleMenu TitleMenu { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            TitleMenu = GetComponentInChildren<TitleMenu>();
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        public void SetUp()
        {
            // セーブデータメニュー
            SaveDataMenu saveDataMenu = GetComponentInChildren<SaveDataMenu>();
            saveDataMenu.SetUp();

            // タイトルメニュー
            TitleMenu.SetUp(saveDataMenu);
        }
    }
}
