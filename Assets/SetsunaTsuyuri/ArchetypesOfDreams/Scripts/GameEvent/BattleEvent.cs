using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘イベント
    /// </summary>
    public class BattleEvent : IGameEvent
    {
        /// <summary>
        /// 敵グループID
        /// </summary>
        [field: SerializeField]
        public int EnemyGroupId { get; set; } = 1;

        /// <summary>
        /// 勝利後、フェードインする
        /// </summary>
        [field: SerializeField]
        public bool RequestsFadeInAfterWin { get; set; } = true;

        /// <summary>
        /// ボスとの戦闘である
        /// </summary>
        [field: SerializeField]
        public bool IsBossBattle { get; set; } = false;

        public UniTask GetUniTask(CancellationToken token)
        {
            return GameEventsManager.ResolveBattleEvent(this, token);
        }
    }
}
