using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 物語の進行度イベント
    /// </summary>
    public class StoryEvent : ProgressionEvent
    {
        public override int GetValue()
        {
            return RuntimeData.StoryProgressions[Id];
        }

        public override void SetValue(int value)
        {
            RuntimeData.StoryProgressions[Id] = value;
        }
    }
}
