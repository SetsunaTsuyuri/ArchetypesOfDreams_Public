﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// HPの表示
    /// </summary>
    public class HpText : StatusText<int>
    {
        protected override void UpdateView()
        {
            if (value == 0)
            {
                view.color = Color.red;
            }
            else
            {
                view.color = Color.white;
            }

            view.text = value.ToString();
        }

        protected override int GetValue(Combatant combatant)
        {
            return combatant.CurrentHP;
        }
    }
}