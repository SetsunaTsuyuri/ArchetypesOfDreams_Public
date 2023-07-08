using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵のUI
    /// </summary>
    public class EnemyUI : CombatantUI
    {
        protected override void Awake()
        {
            base.Awake();

            // 戦闘不能
            MessageBrokersManager.KnockedOut
                .Receive<CombatantContainer>()
                .Where(x => x == Target)
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => DeactivateAndHide());
        }
    }
}
