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
        /// ボスとの戦闘である
        /// </summary>
        [field: SerializeField]
        public bool IsBossBattle { get; set; } = false;

        public BattleEvent(string[] columns)
        {
            if (int.TryParse(columns[1], out int enemyGroupId))
            {
                EnemyGroupId = enemyGroupId;
            }

            if (columns.Length > 2
                && bool.TryParse(columns[2], out bool isBossBattle))
            {
                IsBossBattle = isBossBattle;
            }
        }

        public UniTask Resolve(CancellationToken token)
        {
            return GameEventsManager.ResolveBattleEvent(this, token);
        }
    }
}
