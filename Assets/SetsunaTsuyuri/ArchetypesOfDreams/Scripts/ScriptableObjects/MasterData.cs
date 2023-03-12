using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マスターデータ
    /// </summary>
    [CreateAssetMenu(fileName = nameof(MasterData), menuName = nameof(MasterData))]
    public class MasterData : SingletonScriptableObject<MasterData>
    {
#if UNITY_EDITOR
        /// <summary>
        /// Google Apps Script URL
        /// </summary>
        [field: SerializeField]
        public string GasUrl { get; set; } = string.Empty;

#endif
        /// <summary>
        /// 夢渡り
        /// </summary>
        public DreamWalkerData[]  DreamWalkers = { };

        /// <summary>
        /// ナイトメア
        /// </summary>
        public NightmareData[] Nightmares  = { };

        /// <summary>
        /// スキル
        /// </summary>
        public SkillData[] Skills = { };

        /// <summary>
        /// アイテム
        /// </summary>
        public ItemData[] Items = { };

        /// <summary>
        /// ステータス効果
        /// </summary>
        public StatusEffectData[] StatusEffects = { };

        /// <summary>
        /// ダンジョン
        /// </summary>
        public DungeonData[] Dungeons = { };

        /// <summary>
        /// 敵グループ
        /// </summary>
        public EnemyGroupData[] EnemyGroups= { };

        /// <summary>
        /// シナリオ
        /// </summary>
        public ScenarioData[] Scenarios= { };

        /// <summary>
        /// 夢渡りデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DreamWalkerData GetDreamWalkerData(int id)
        {
            return Instance.DreamWalkers.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// ナイトメアデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NightmareData GetNightmareData(int id)
        {
            return Instance.Nightmares.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// スキルデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SkillData GetSkillData(int id)
        {
            return Instance.Skills.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// 基本スキルデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SkillData GetSkillData(BasicSkillId id)
        {
            return GetSkillData((int)id);
        }

        /// <summary>
        /// アイテムデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ItemData GetItemData(int id)
        {
            return Instance.Items.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// 基本ステータス効果データを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static StatusEffectData GetStatusEffectData(StatusEffectId id)
        {
            return GetStatusEffectData((int)id);
        }

        /// <summary>
        /// ステータス効果データを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static StatusEffectData GetStatusEffectData(int id)
        {
            return Instance.StatusEffects.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// ダンジョンデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DungeonData GetDungeonData(int id)
        {
            return Instance.Dungeons.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// 敵グループデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static EnemyGroupData GetEnemyGroupData(int id)
        {
            return Instance.EnemyGroups.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// シナリオデータを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ScenarioData GetScenarioData(int id)
        {
            return Instance.Scenarios.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// アイテムデータの数を取得する
        /// </summary>
        /// <returns></returns>
        public static int CountItems()
        {
            return Instance.Items.Length;
        }

        /// <summary>
        /// ダンジョンデータの数を取得する
        /// </summary>
        /// <returns></returns>
        public static int CountDungeons()
        {
            return Instance.Dungeons.Length;
        }

        /// <summary>
        /// 選択可能なダンジョンデータ配列を取得する
        /// </summary>
        /// <returns></returns>
        public static DungeonData[] GetSelectableDungeons()
        {
            return Instance.Dungeons
                .Where(x => !x.CannotBeSelected)
                .ToArray();
        }
    }
}
