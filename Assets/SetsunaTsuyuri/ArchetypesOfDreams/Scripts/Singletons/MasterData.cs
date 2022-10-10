using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 変化しないデータ
    /// </summary>
    public class MasterData : Singleton<MasterData>, IInitializable
    {
        /// <summary>
        /// ダンジョンのデータ集
        /// </summary>
        DungeonDataCollection dungeons = null;

        /// <summary>
        /// ダンジョンのデータ集
        /// </summary>
        public static DungeonDataCollection Dungeons
        {
            get => Instance.dungeons;
        }

        /// <summary>
        /// 敵グループのデータ集
        /// </summary>
        EnemyGroupDataCollection enemyGroups = null;

        /// <summary>
        /// 敵グループのデータ集
        /// </summary>
        public static EnemyGroupDataCollection EnemyGroups
        {
            get => Instance.enemyGroups;
        }

        /// <summary>
        /// 夢渡りのデータ集
        /// </summary>
        DreamWalkerDataCollection dreamWalkes = null;

        /// <summary>
        /// 夢渡りのデータ集
        /// </summary>
        public static DreamWalkerDataCollection DreamWalkers
        {
            get => Instance.dreamWalkes;
        }

        /// <summary>
        /// ナイトメアのデータ集
        /// </summary>
        NightmareDataCollection nightmares = null;

        /// <summary>
        /// ナイトメアのデータ集
        /// </summary>
        public static NightmareDataCollection Nightmares
        {
            get => Instance.nightmares;
        }

        /// <summary>
        /// スキルデータグループ
        /// </summary>
        SkillDataGroup _skills = null;

        /// <summary>
        /// スキルデータグループ
        /// </summary>
        public static SkillDataGroup Skills
        {
            get => Instance._skills;
        }

        /// <summary>
        /// 武器スキルのデータ集
        /// </summary>
        SkillDataGroup weaponSkills = null;

        /// <summary>
        /// 武器スキルのデータ集
        /// </summary>
        public static SkillDataGroup WeaponSkills
        {
            get => Instance.weaponSkills;
        }

        /// <summary>
        /// 特殊スキルのデータ集
        /// </summary>
        SkillDataGroup specialSkills = null;

        /// <summary>
        /// 特殊スキルのデータ集
        /// </summary>
        public static SkillDataGroup SpecialSkills
        {
            get => Instance.specialSkills;
        }

        /// <summary>
        /// 汎用スキルのデータ集
        /// </summary>
        CommonSkillDataCollection commonSkills = null;

        /// <summary>
        /// 汎用スキルのデータ集
        /// </summary>
        public static CommonSkillDataCollection CommonSkills
        {
            get => Instance.commonSkills;
        }

        /// <summary>
        /// ステータス効果のデータ集
        /// </summary>
        StatusEffectDataCollection statusEffects = null;

        /// <summary>
        /// ステータス効果のデータ集
        /// </summary>
        public static StatusEffectDataCollection StatusEffects
        {
            get => Instance.statusEffects;
        }

        /// <summary>
        /// アイテムのデータ集
        /// </summary>
        ItemDataCollection items = null;

        public static ItemDataCollection Items
        {
            get => Instance.items;
        }

        /// <summary>
        /// シナリオのデータ集
        /// </summary>
        ScenarioDataCollection scenarios = null;

        /// <summary>
        /// シナリオのデータ集
        /// </summary>
        public static ScenarioDataCollection Scenarios
        {
            get => Instance.scenarios;
        }

        public override void Initialize()
        {
            dungeons = Resources.Load<DungeonDataCollection>(ResourcesPath.Dungeons);
            enemyGroups = Resources.Load<EnemyGroupDataCollection>(ResourcesPath.EnemyGroups);
            dreamWalkes = Resources.Load<DreamWalkerDataCollection>(ResourcesPath.DreamWalkers);
            nightmares = Resources.Load<NightmareDataCollection>(ResourcesPath.Nightmares);
            _skills = Resources.Load<SkillDataGroup>("MasterData/Skills");
            statusEffects = Resources.Load<StatusEffectDataCollection>(ResourcesPath.StatusEffects);
            items = Resources.Load<ItemDataCollection>(ResourcesPath.Items);
            scenarios = Resources.Load<ScenarioDataCollection>(ResourcesPath.Scenarios);
        }
    }
}
