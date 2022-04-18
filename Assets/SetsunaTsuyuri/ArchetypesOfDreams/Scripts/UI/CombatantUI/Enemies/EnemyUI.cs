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
        public override void OnConditionSet(Attribute.Condition condition)
        {
            switch (condition)
            {
                case Attribute.Condition.KnockedOut:
                    DeactivateAndHide();
                    break;
            }
        }
    }
}
