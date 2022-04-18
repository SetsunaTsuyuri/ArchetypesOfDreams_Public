using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームコマンドを実行した結果
    /// </summary>
    public class GameCommandResult
    {
        /// <summary>
        /// プレイヤーが敗北した
        /// </summary>
        public bool PlayerHasBeenDefeated { get; set; } = false;
    }
}
