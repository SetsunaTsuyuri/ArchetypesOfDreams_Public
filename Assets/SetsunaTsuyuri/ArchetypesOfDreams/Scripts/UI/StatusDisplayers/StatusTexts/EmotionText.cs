using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 感情属性の表示
    /// </summary>
    public class EmotionText : StatusText<GameAttribute.Emotion>
    {
        protected override void UpdateView()
        {
            view.text = GameSettings.Terms.EmotionIconNames.GetValueOrDefault(value);
        }

        protected override GameAttribute.Emotion GetValue(Combatant combatant)
        {
            return combatant.Emotion;
        }
    }
}
