using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵のUI
    /// </summary>
    public class EnemyUI : CombatantContainerUI
    {
        public override void OnConditionSet(GameAttribute.Condition condition)
        {
            switch (condition)
            {
                case GameAttribute.Condition.KnockedOut:
                    DeactivateAndHide();
                    break;
            }
        }
    }
}
