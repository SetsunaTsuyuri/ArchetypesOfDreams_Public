using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘イベント
    /// </summary>
    [System.Serializable]
    public class BattleEvent : GameEventBase
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
            EnemyGroupId = ToInt(columns, 1);
            IsBossBattle = ToBool(columns, 2);
        }

        public override UniTask Resolve(CancellationToken token)
        {
            return GameEventsManager.ResolveBattleEvent(this, token);
        }
    }
}
