using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 物語の進行度イベント
    /// </summary>
    [Serializable]
    public class StoryEvent : ProgressionEvent
    {
        public StoryEvent(string[] columns) : base(columns) { }

        public override int GetValue()
        {
            return VariableData.StoryProgressions[Id];
        }

        public override void SetValue(int value)
        {
            VariableData.StoryProgressions[Id] = value;
        }
    }
}
