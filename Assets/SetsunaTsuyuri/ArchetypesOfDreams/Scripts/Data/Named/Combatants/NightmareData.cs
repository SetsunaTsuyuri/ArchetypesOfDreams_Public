﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ナイトメアのデータ
    /// </summary>
    [System.Serializable]
    public class NightmareData : CombatantData
    {
        /// <summary>
        /// 浄化成功率
        /// </summary>
        [field: SerializeField]
        public int PurificationSuccessRate { get; private set; }
    }
}
