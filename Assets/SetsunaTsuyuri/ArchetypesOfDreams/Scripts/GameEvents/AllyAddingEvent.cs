using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 味方追加イベント
    /// </summary>
    [System.Serializable]
    public class AllyAddingEvent : GameEventBase
    {
        /// <summary>
        /// ID
        /// </summary>
        [field: SerializeField]
        public int Id { get; set; } = 0;

        public AllyAddingEvent(string[] columns) 
        {
            Id = ToInt(columns, 1, 1);
        }

        public override async UniTask Resolve(CancellationToken token)
        {
            await UniTask.CompletedTask;
            token.ThrowIfCancellationRequested();

            AlliesParty allies = AlliesParty.InstanceInActiveScene;
            if (!allies)
            {
                return;
            }

            Combatant combatant = new DreamWalker()
            {
                DataId = Id,
                Level = 1
            };

            combatant.Initialize();
            combatant.Experience = allies.MaxDreamWalkerExperience;
            
            allies.TryAdd(combatant);
        }
    }
}
