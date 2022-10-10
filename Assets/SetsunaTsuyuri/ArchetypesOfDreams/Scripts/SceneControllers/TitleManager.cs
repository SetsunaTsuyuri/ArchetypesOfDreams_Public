using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// タイトルの管理者
    /// </summary>
    public class TitleManager : MonoBehaviour
    {
        private void Start()
        {
            AudioManager.PlayBgm("自室");
        }
    }
}
