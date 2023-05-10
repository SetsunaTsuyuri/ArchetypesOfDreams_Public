using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
        static readonly string s_path = "GameSettings/";

        CombatantsSettings _combatants = null;

        /// <summary>
        /// 戦闘者の設定
        /// </summary>
        public static CombatantsSettings Combatants
        {
            get => Instance._combatants;
        }

        AlliesSettings _allies = null;

        /// <summary>
        /// 味方の設定
        /// </summary>
        public static AlliesSettings Allies
        {
            get => Instance._allies;
        } 

        EnemiesSettings _enemies = null;

        /// <summary>
        /// 敵の設定
        /// </summary>
        public static EnemiesSettings Enemies
        {
            get => Instance._enemies;
        }

        MapObjectsSettings _mapObjects = null;

        /// <summary>
        /// マップオブジェクトの設定
        /// </summary>
        public static MapObjectsSettings MapObjects
        {
            get => Instance._mapObjects;
        }

        ItemsSettings _items = null;

        /// <summary>
        /// アイテムの設定
        /// </summary>
        public static ItemsSettings Items
        {
            get => Instance._items;
        }

        EmotionsSettings _emotions = null;

        /// <summary>
        /// 感情属性の設定
        /// </summary>
        public static EmotionsSettings Emotions
        {
            get => Instance._emotions;
        }

        EffectivenessSettings _effectiveness = null;

        /// <summary>
        /// 有効性の倍率設定
        /// </summary>
        public static EffectivenessSettings Effectiveness
        {
            get => Instance._effectiveness;
        }

        VisualEffectsSettings _visualEffects = null;

        /// <summary>
        /// 視覚効果の設定
        /// </summary>
        public static VisualEffectsSettings VisualEffects
        {
            get => Instance._visualEffects; 
        }

        DescriptionSettings _description = null;

        /// <summary>
        /// 説明文の設定
        /// </summary>
        public static DescriptionSettings Description
        {
            get => Instance._description;
        }

        PopUpTextsSettings _popUpTexts = null;

        /// <summary>
        /// ポップアップテキストの設定
        /// </summary>
        public static PopUpTextsSettings PopUpTexts
        {
            get => Instance._popUpTexts;
        }

        PurificationSettings _purification = null;

        /// <summary>
        /// 浄化の設定
        /// </summary>
        public static PurificationSettings Purification
        {
            get => Instance._purification;
        }

        CamerasSettings _cameras = null;

        /// <summary>
        /// カメラの設定
        /// </summary>
        public static CamerasSettings Cameras
        {
            get => Instance._cameras;
        }

        TermsSettings _terms = null;

        /// <summary>
        /// 用語の設定
        /// </summary>
        public static TermsSettings Terms
        {
            get => Instance._terms;
        }

        WaitTimeSettings _waitTime = null;

        /// <summary>
        /// 待機時間の設定
        /// </summary>
        public static WaitTimeSettings WaitTime
        {
            get => Instance._waitTime;
        }

        OtherSettings _other = null;

        /// <summary>
        /// その他の設定
        /// </summary>
        public static OtherSettings Other
        {
            get => Instance._other;
        }

        public override void Initialize()
        {
            _combatants = Resources.Load<CombatantsSettings>(s_path + "Combatants");
            _allies = Resources.Load<AlliesSettings>(s_path + "Allies");
            _enemies = Resources.Load<EnemiesSettings>(s_path + "Enemies");
            _mapObjects = Resources.Load<MapObjectsSettings>(s_path + "MapObjects");
            _items = Resources.Load<ItemsSettings>(s_path + "Items");
            _emotions = Resources.Load<EmotionsSettings>(s_path + "Emotions");
            _effectiveness = Resources.Load<EffectivenessSettings>(s_path + "Effectiveness");
            _visualEffects = Resources.Load<VisualEffectsSettings>(s_path + "VisualEffects");
            _description = Resources.Load<DescriptionSettings>(s_path + "Description");
            _popUpTexts = Resources.Load<PopUpTextsSettings>(s_path + "PopUpTexts");
            _purification = Resources.Load<PurificationSettings>(s_path + "Purification");
            _cameras = Resources.Load<CamerasSettings>(s_path + "Cameras");
            _terms = Resources.Load<TermsSettings>(s_path + "Terms");
            _waitTime = Resources.Load<WaitTimeSettings>(s_path + "WaitTime");
            _other = Resources.Load<OtherSettings>(s_path + "Other");
        }

        /// <summary>
        /// 感情属性による有効性を取得する
        /// </summary>
        /// <param name="defenseEmotion">守備側の感情属性</param>
        /// <param name="attackEmotion">攻撃側の感情属性</param>
        /// <returns>見つからなければNormalを返す</returns>
        public static GameAttribute.Effectiveness GetEffectiveness(GameAttribute.Emotion defenseEmotion, GameAttribute.Emotion attackEmotion)
        {
            return Instance._emotions.GetEffectiveness(defenseEmotion, attackEmotion);
        }
    }
}
