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
        /// パス
        /// </summary>
        static readonly string s_path = "Settings/";

        /// <summary>
        /// 戦闘者の設定
        /// </summary>
        CombatantsSettings _combatants = null;

        /// <summary>
        /// 戦闘者の設定
        /// </summary>
        public static CombatantsSettings Combatants
        {
            get => Instance._combatants;
        }

        /// <summary>
        /// 敵の設定
        /// </summary>
        EnemiesSettings _enemies = null;

        /// <summary>
        /// 敵の設定
        /// </summary>
        public static EnemiesSettings Enemies
        {
            get => Instance._enemies;
        }

        /// <summary>
        /// マップの設定
        /// </summary>
        MapsSettings _maps = null;

        /// <summary>
        /// マップの設定
        /// </summary>
        public static MapsSettings Maps
        {
            get => Instance._maps;
        }

        /// <summary>
        /// アイテムの設定
        /// </summary>
        ItemsSettings _items = null;

        /// <summary>
        /// アイテムの設定
        /// </summary>
        public static ItemsSettings Items
        {
            get => Instance._items;
        }

        /// <summary>
        /// 感情属性の設定
        /// </summary>
        EmotionsSettings _emotions = null;

        /// <summary>
        /// 感情属性の設定
        /// </summary>
        public static EmotionsSettings Emotions
        {
            get => Instance._emotions;
        }

        /// <summary>
        /// 有効性の倍率設定
        /// </summary>
        EffectivenessSettings _effectiveness = null;

        /// <summary>
        /// 有効性の倍率設定
        /// </summary>
        public static EffectivenessSettings Effectiveness
        {
            get => Instance._effectiveness;
        }

        /// <summary>
        /// 視覚効果の設定
        /// </summary>
        VisualEffectsSettings _visualEffects = null;

        /// <summary>
        /// 視覚効果の設定
        /// </summary>
        public static VisualEffectsSettings VisualEffects
        {
            get => Instance._visualEffects; 
        }

        /// <summary>
        /// 説明文の設定
        /// </summary>
        DescriptionSettings _description = null;

        /// <summary>
        /// 説明文の設定
        /// </summary>
        public static DescriptionSettings Description
        {
            get => Instance._description;
        }

        /// <summary>
        /// ポップアップテキストの設定
        /// </summary>
        PopUpTextsSettings _popUpTexts = null;

        /// <summary>
        /// ポップアップテキストの設定
        /// </summary>
        public static PopUpTextsSettings PopUpTexts
        {
            get => Instance._popUpTexts;
        }

        /// <summary>
        /// 浄化の設定
        /// </summary>
        PurificationSettings _purification = null;

        /// <summary>
        /// 浄化の設定
        /// </summary>
        public static PurificationSettings Purification
        {
            get => Instance._purification;
        }

        /// <summary>
        /// 用語の設定
        /// </summary>
        TermsSettings _terms = null;

        /// <summary>
        /// 用語の設定
        /// </summary>
        public static TermsSettings Terms
        {
            get => Instance._terms;
        }

        public override void Initialize()
        {
            _combatants = Resources.Load<CombatantsSettings>(s_path + "Combatants");
            _enemies = Resources.Load<EnemiesSettings>(s_path + "Enemies");
            _maps = Resources.Load<MapsSettings>(s_path + "Maps");
            _items = Resources.Load<ItemsSettings>(s_path + "Items");
            _emotions = Resources.Load<EmotionsSettings>(s_path + "Emotions");
            _effectiveness = Resources.Load<EffectivenessSettings>(s_path + "Effectiveness");
            _visualEffects = Resources.Load<VisualEffectsSettings>(s_path + "VisualEffects");
            _description = Resources.Load<DescriptionSettings>(s_path + "Description");
            _popUpTexts = Resources.Load<PopUpTextsSettings>(s_path + "PopUpTexts");
            _purification = Resources.Load<PurificationSettings>(s_path + "Purification");
            _terms = Resources.Load<TermsSettings>(s_path + "Terms");
        }

        /// <summary>
        /// 感情属性による有効性を取得する
        /// </summary>
        /// <param name="attackEmotion">攻撃側の感情属性</param>
        /// <param name="defenseEmotion">守備側の感情属性</param>
        /// <returns>見つからなければNormalを返す</returns>
        public static Attribute.Effectiveness GetEffectiveness(Attribute.Emotion attackEmotion, Attribute.Emotion defenseEmotion)
        {
            return Instance._emotions.GetEffectiveness(defenseEmotion, attackEmotion);
        }
    }
}
