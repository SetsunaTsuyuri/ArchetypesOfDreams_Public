using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 物語の進行度による起動条件
    /// </summary>
    [System.Serializable]
    public class StoryTrigger : ProgressionTrigger
    {
        public StoryTrigger(int? id, FormulaType? formula, int? parameter) : base(id, formula, parameter) { }

        public override int GetValue()
        {
            return RuntimeData.StoryProgressions[Id];
        }
    }
}
