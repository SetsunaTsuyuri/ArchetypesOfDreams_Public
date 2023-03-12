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
        /// <param name="defenseEmotion">守備側の感情属性</param>
        /// <param name="attackEmotion">攻撃側の感情属性</param>
        /// <returns>見つからなければNormalを返す</returns>
        public GameAttribute.Effectiveness GetEffectiveness(GameAttribute.Emotion defenseEmotion, GameAttribute.Emotion attackEmotion)
        {
            GameAttribute.Effectiveness result = GameAttribute.Effectiveness.Normal;

            var defense = Effectiveness.FirstOrDefault(x => x.Defense == defenseEmotion);
            if (defense != null)
            {
                var attack = defense.Effectiveness.FirstOrDefault(x => x.Key == attackEmotion);
                if (attack != null)
                {
                    result = attack.Value;
                }
            }

            return result;
        }
    }
}
