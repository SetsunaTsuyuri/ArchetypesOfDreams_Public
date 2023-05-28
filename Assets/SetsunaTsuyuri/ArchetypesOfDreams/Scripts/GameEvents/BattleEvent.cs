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
        /// BGMID
        /// </summary>
        [field: SerializeField]
        public int BgmId { get; set; } = 0;

        /// <summary>
        /// 味方が逃走可能である
        /// </summary>
        [field: SerializeField]
        public bool AlliesCanEscape { get; set; } = false;

        /// <summary>
        /// ボスとの戦闘である
        /// </summary>
        [field: SerializeField]
        public bool IsBossBattle { get; set; } = false;

        public BattleEvent(string[] columns)
        {
            EnemyGroupId = ToInt(columns, 1);
            BgmId = ToInt(columns, 2);
            AlliesCanEscape= ToBool(columns, 3);
            IsBossBattle = ToBool(columns, 4);
        }

        public override UniTask Resolve(CancellationToken token)
        {
            return GameEventsManager.ResolveBattleEvent(this, token);
        }
    }
}
