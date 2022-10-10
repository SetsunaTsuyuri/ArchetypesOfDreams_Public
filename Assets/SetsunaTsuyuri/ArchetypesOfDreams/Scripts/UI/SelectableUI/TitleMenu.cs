using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// タイトルメニューボタンの管理者
    /// </summary>
    public class TitleMenu : SelectableGameUI<GameButton>
    {
        /// <summary>
        /// セーブデータボタンの管理者
        /// </summary>
        [SerializeField]
        SaveDataMenu saveDataMenu = null;

        protected override void Awake()
        {
            base.Awake();
            saveDataMenu.Previous = this;
        }

        private void Start()
        {
            SetUp();
        }
    }
}
