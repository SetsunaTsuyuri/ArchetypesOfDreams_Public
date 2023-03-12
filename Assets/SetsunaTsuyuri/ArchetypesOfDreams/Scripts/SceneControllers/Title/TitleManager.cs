using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// タイトルシーンの管理者
    /// </summary>
    public class TitleManager : MonoBehaviour
    {
        /// <summary>
        /// UI
        /// </summary>
        [SerializeField]
        TitleUIManager _ui = null;

        private void Start()
        {
            _ui.SetUp();

            AudioManager.PlayBgm(BgmId.Title);
            _ui.TitleMenu.BeSelected();
        }
    }
}
