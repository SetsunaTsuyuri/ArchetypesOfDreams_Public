using System.Collections.Generic;
using System.Threading;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ステータス効果イベント
    /// </summary>
    [System.Serializable]
    public class StatusEffectEvent : GameEventBase
    {
        /// <summary>
        /// 対象となる夢魔ID
        /// </summary>
        [field: SerializeField]
        public int DreamWalkerId { get; set; } = 0;

        /// <summary>
        /// ステータス効果ID
        /// </summary>
        [field: SerializeField]
        public int StatusEffectId { get; set; } = 0;

        /// <summary>
        /// 解除する
        /// </summary>
        [field: SerializeField]
        public bool IsRemoval { get; set; } = false;

        /// <summary>
        /// ステータス効果付与データ
        /// </summary>
        [field: SerializeField]
        public EffectData.StatusEffect StatusEffect { get; set; } = new();

        public StatusEffectEvent(string[] columns)
        {
            DreamWalkerId = ToInt(columns, 1);

            EffectData.StatusEffect effect = new()
            {
                StatusEffectId = ToInt(columns, 2),
                IsRemoval = ToBool(columns, 3),
                IsPermanent = true,
                IgnoresRegistance = true
            };

            StatusEffect = effect;
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

            // 対象を探す
            Combatant target = null;
            if (DreamWalkerId > 0)
            {
                target = allies.GetNonEmptyContainers()
                    .Where(x => x.Combatant is DreamWalker)
                    .Select(x => x.Combatant)
                    .FirstOrDefault(x => x.DataId == DreamWalkerId);
            }
            if (target is null)
            {
                return;
            }

            // 効果を付与する
            target.AddStatusEffect(StatusEffect);
        }
    }
}
