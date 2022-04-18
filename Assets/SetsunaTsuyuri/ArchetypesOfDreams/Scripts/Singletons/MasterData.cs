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
        /// 武器スキルのデータ集
        /// </summary>
        SkillDataCollection weaponSkills = null;

        /// <summary>
        /// 武器スキルのデータ集
        /// </summary>
        public static SkillDataCollection WeaponSkills
        {
            get => Instance.weaponSkills;
        }

        /// <summary>
        /// 特殊スキルのデータ集
        /// </summary>
        SkillDataCollection specialSkills = null;

        /// <summary>
        /// 特殊スキルのデータ集
        /// </summary>
        public static SkillDataCollection SpecialSkills
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
            weaponSkills = Resources.Load<SkillDataCollection>(ResourcesPath.WeaponSkills);
            specialSkills = Resources.Load<SkillDataCollection>(ResourcesPath.SpecialSkills);
            commonSkills = Resources.Load<CommonSkillDataCollection>(ResourcesPath.CommonSkills);
            scenarios = Resources.Load<ScenarioDataCollection>(ResourcesPath.Scenarios);
        }

        /// <summary>
        /// 武器スキルのデータを読み込む
        /// </summary>
        /// <param name="idList">スキルIDリスト</param>
        public static SkillData[] LoadWeaponSkillData(List<int> idList)
        {
            List<SkillData> skills = new List<SkillData>();

            foreach (var id in idList)
            {
                SkillData data = WeaponSkills.GetValue(id);
                skills.Add(data);
            }

            return skills.ToArray();
        }

        /// <summary>
        /// 特殊スキルのデータを読み込む
        /// </summary>
        /// <param name="idList">スキルIDリスト</param>
        public static SkillData[] LoadSpecialSkillData(List<int> idList)
        {
            List<SkillData> skills = new List<SkillData>();

            foreach (var id in idList)
            {
                SkillData data = SpecialSkills.GetValue(id);
                skills.Add(data);
            }

            return skills.ToArray();
        }
    }
}
