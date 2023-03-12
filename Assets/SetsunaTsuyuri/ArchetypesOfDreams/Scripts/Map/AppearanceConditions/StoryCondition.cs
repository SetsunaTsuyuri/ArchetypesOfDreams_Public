using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 物語の進行度による起動条件
    /// </summary>
    [System.Serializable]
    public class StoryCondition : ProgressionCondition
    {
        public StoryCondition(string[] columns) : base(columns) { }

        public override int GetValue()
        {
            return VariableData.StoryProgressions[Id];
        }
    }
}
