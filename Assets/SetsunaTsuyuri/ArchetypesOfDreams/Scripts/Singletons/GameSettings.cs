using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲーム設定
    /// </summary>
    public class GameSettings : Singleton<GameSettings>, IInitializable
    {
        /// <summary>
        /// 戦闘者の設定
        /// </summary>
        CombatantsSettings combatants = null;

        /// <summary>
        /// 戦闘者の設定
        /// </summary>
        public static CombatantsSettings Combatants
        {
            get => Instance.combatants;
        }

        /// <summary>
        /// 感情属性の設定
        /// </summary>
        EmotionsSettings emotions = null;

        /// <summary>
        /// 感情属性の設定
        /// </summary>
        public static EmotionsSettings Emotions
        {
            get => Instance.emotions;
        }

        /// <summary>
        /// 有効性の倍率設定
        /// </summary>
        EffectivenessSettings effectiveness = null;

        /// <summary>
        /// 有効性の倍率設定
        /// </summary>
        public static EffectivenessSettings Effectiveness
        {
            get => Instance.effectiveness;
        }

        /// <summary>
        /// 視覚効果の設定
        /// </summary>
        VisualEffectsSettings visualEffects = null;

        /// <summary>
        /// 視覚効果の設定
        /// </summary>
        public static VisualEffectsSettings VisualEffects
        {
            get => Instance.visualEffects; 
        }

        /// <summary>
        /// 説明文の設定
        /// </summary>
        DescriptionSettings description = null;

        /// <summary>
        /// 説明文の設定
        /// </summary>
        public static DescriptionSettings Description
        {
            get => Instance.description;
        }

        /// <summary>
        /// ポップアップテキストの設定
        /// </summary>
        PopUpTextsSettings popUpTexts = null;

        /// <summary>
        /// ポップアップテキストの設定
        /// </summary>
        public static PopUpTextsSettings PopUpTexts
        {
            get => Instance.popUpTexts;
        }

        /// <summary>
        /// 浄化の設定
        /// </summary>
        PurificationSettings purification = null;

        /// <summary>
        /// 浄化の設定
        /// </summary>
        public static PurificationSettings Purification
        {
            get => Instance.purification;
        }

        /// <summary>
        /// 用語の設定
        /// </summary>
        TermsSettings terms = null;

        /// <summary>
        /// 用語の設定
        /// </summary>
        public static TermsSettings Terms
        {
            get => Instance.terms;
        }

        public override void Initialize()
        {
            combatants = Resources.Load<CombatantsSettings>(ResourcesPath.CombatantsSettings);
            emotions = Resources.Load<EmotionsSettings>(ResourcesPath.EmotionsSettings);
            effectiveness = Resources.Load<EffectivenessSettings>(ResourcesPath.EffectivenessSettings);
            visualEffects = Resources.Load<VisualEffectsSettings>(ResourcesPath.VisualEffectsSettings);
            description = Resources.Load<DescriptionSettings>(ResourcesPath.DescriptionSettings);
            popUpTexts = Resources.Load<PopUpTextsSettings>(ResourcesPath.PopUpTextsSettings);
            purification = Resources.Load<PurificationSettings>(ResourcesPath.PurificationSettings);
            terms = Resources.Load<TermsSettings>(ResourcesPath.TermsSettings);
        }

        /// <summary>
        /// 感情属性による有効性を取得する
        /// </summary>
        /// <param name="attackEmotion">攻撃側の感情属性</param>
        /// <param name="defenseEmotion">守備側の感情属性</param>
        /// <returns>見つからなければNormalを返す</returns>
        public static Attribute.Effectiveness GetEffectiveness(Attribute.Emotion attackEmotion, Attribute.Emotion defenseEmotion)
        {
            return Instance.emotions.GetEffectiveness(defenseEmotion, attackEmotion);
        }
    }
}
