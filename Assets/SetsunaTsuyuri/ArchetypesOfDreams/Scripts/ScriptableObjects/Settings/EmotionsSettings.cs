using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 感情属性の設定
    /// </summary>
    [CreateAssetMenu(fileName = "Emotions", menuName = "Settings/Emotions")]
    public class EmotionsSettings : ScriptableObject
    {
        // 属性毎の有効性配列
        [field: SerializeField]
        public EffectivenessDataOfEmotions[] Effectiveness { get; private set; }

        /// <summary>
        /// 有効性を取得する
        /// </summary>
        /// <param name="attackEmotion">攻撃側の感情属性</param>
        /// <param name="defenseEmotion">守備側の感情属性</param>
        /// <returns>見つからなければNormalを返す</returns>
        public Attribute.Effectiveness GetEffectiveness(Attribute.Emotion attackEmotion, Attribute.Emotion defenseEmotion)
        {
            Attribute.Effectiveness result = Attribute.Effectiveness.Normal;

            var offense = Effectiveness.FirstOrDefault(a => a.Attack == attackEmotion);
            if (offense != null)
            {
                var defense = offense.Effectiveness.FirstOrDefault(a => a.Key == defenseEmotion);
                if (defense != null)
                {
                    result = defense.Value;
                }
            }

            return result;
        }
    }
}
