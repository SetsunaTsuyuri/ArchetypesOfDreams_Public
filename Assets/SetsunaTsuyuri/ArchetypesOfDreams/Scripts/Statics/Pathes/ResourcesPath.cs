using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// Resourcesフォルダに置いたアセットにアクセスするためのパス
    /// </summary>
    public class ResourcesPath : MonoBehaviour
    {
        /// <summary>
        /// 戦闘者の設定
        /// </summary>
        public static readonly string CombatantsSettings = "Settings/Combatants";

        /// <summary>
        /// マップの設定
        /// </summary>
        public static readonly string MapsSettings = "Settings/Maps";

        /// <summary>
        /// アイテムの設定
        /// </summary>
        public static readonly string ItemsSettings = "Settings/Items";

        /// <summary>
        /// 感情属性の設定
        /// </summary>
        public static readonly string EmotionsSettings = "Settings/Emotions";

        /// <summary>
        /// 有効性の設定
        /// </summary>
        public static readonly string EffectivenessSettings = "Settings/Effectiveness";

        /// <summary>
        /// 視覚効果の設定
        /// </summary>
        public static readonly string VisualEffectsSettings = "Settings/VisualEffects";

        /// <summary>
        /// 説明文の設定
        /// </summary>
        public static readonly string DescriptionSettings = "Settings/Description";

        /// <summary>
        /// ポップアップテキストの設定
        /// </summary>
        public static readonly string PopUpTextsSettings = "Settings/PopUpTexts";

        /// <summary>
        /// 浄化の設定
        /// </summary>
        public static readonly string PurificationSettings = "Settings/Purification";

        /// <summary>
        /// 用語の設定
        /// </summary>
        public static readonly string TermsSettings = "Settings/Terms";

        /// <summary>
        /// ダンジョンのデータ集
        /// </summary>
        public static readonly string Dungeons = "MasterData/Dungeons";

        /// <summary>
        /// 敵グループのデータ集
        /// </summary>
        public static readonly string EnemyGroups = "MasterData/EnemyGroups";

        /// <summary>
        /// 夢渡りのデータ集
        /// </summary>
        public static readonly string DreamWalkers = "MasterData/Combatants/DreamWalkers";

        /// <summary>
        /// ナイトメアのデータ集
        /// </summary>
        public static readonly string Nightmares = "MasterData/Combatants/Nightmares";

        /// <summary>
        /// 武器スキルのデータ集
        /// </summary>
        public static readonly string WeaponSkills = "MasterData/Skills/WeaponSkills";

        /// <summary>
        /// 特殊スキルのデータ集
        /// </summary>
        public static readonly string SpecialSkills = "MasterData/Skills/SpecialSkills";

        /// <summary>
        /// 汎用スキルのデータ集
        /// </summary>
        public static readonly string CommonSkills = "MasterData/Skills/CommonSkills";

        /// <summary>
        /// シナリオのデータ集
        /// </summary>
        public static readonly string Scenarios = "MasterData/Scenarios";

        /// <summary>
        /// ステータス効果のデータ集
        /// </summary>
        public static readonly string StatusEffects = "MasterData/StatusEffects";

        /// <summary>
        /// アイテムのデータ集
        /// </summary>
        public static readonly string Items = "MasterData/Items";

        /// <summary>
        /// リセットマネージャー
        /// </summary>
        public static readonly string Reset = "Prefabs/Reset";
    }
}
