using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者のステータス
    /// </summary>
    public partial class Combatant
    {
        /// <summary>
        /// レベル
        /// </summary>
        [field: SerializeField]
        public int Level { get; set; } = 1;

        /// <summary>
        /// 経験値
        /// </summary>
        [SerializeField]
        int experience = 0;

        /// <summary>
        /// 経験値
        /// </summary>
        public int Experience
        {
            get => experience;
            set
            {
                experience = value;
                OnExperienceSet();
            }
        }

        /// <summary>
        /// 生命力
        /// </summary>
        int life = 0;

        /// <summary>
        /// 生命力
        /// </summary>
        public int Life
        {
            get => life;
            set
            {
                life = Mathf.Clamp(value, 0, MaxLife);
            }
        }

        /// <summary>
        /// 最大生命力
        /// </summary>
        int maxLife;

        /// <summary>
        /// 最大生命力
        /// </summary>
        public int MaxLife
        {
            get => maxLife;
            set
            {
                maxLife = Mathf.Clamp(value, 0, GameSettings.Combatants.MaxLife);
            }
        }

        /// <summary>
        /// 夢想力
        /// </summary>
        int dream = 0;

        /// <summary>
        /// 夢想力
        /// </summary>
        public int Dream
        {
            get => dream;
            set
            {
                dream = Mathf.Clamp(value, 0, GameSettings.Combatants.MaxDream);
            }
        }

        /// <summary>
        /// 初期夢想力
        /// </summary>
        public int InitialDream { get; set; } = 0;

        /// <summary>
        /// 精神力
        /// </summary>
        int soul = 0;

        /// <summary>
        /// 精神力
        /// </summary>
        public int Soul
        {
            get => soul;
            set
            {
                soul = Mathf.Clamp(value, 0, MaxSoul);
            }
        }

        /// <summary>
        /// 最大精神力
        /// </summary>
        int maxSoul = 0;

        /// <summary>
        /// 最大精神力
        /// </summary>
        public int MaxSoul
        {
            get => maxSoul;
            set
            {
                maxSoul = Mathf.Clamp(value, 0, GameSettings.Combatants.MaxSoul);
            }
        }

        /// <summary>
        /// 近接攻撃力
        /// </summary>
        public int MeleeAttack { get; set; } = 0;

        /// <summary>
        /// 近接守備力
        /// </summary>
        public int MeleeDefense { get; set; } = 0;

        /// <summary>
        /// 遠隔攻撃力
        /// </summary>
        public int RangedAttack { get; set; } = 0;

        /// <summary>
        /// 遠隔守備力
        /// </summary>
        public int RangedDefense { get; set; } = 0;

        /// <summary>
        /// 基本の素早さ
        /// </summary>
        public int BaseSpeed { get; set; } = 0;

        /// <summary>
        /// ランダムに変化する素早さ
        /// </summary>
        public float RandomSpeed { get; set; } = 0.0f;

        /// <summary>
        /// 感情属性
        /// </summary>
        public Attribute.Emotion Emotion { get; set; } = Attribute.Emotion.None;

        /// <summary>
        /// クラッシュ状態からの復帰するまでの残りターン数
        /// </summary>
        public int RemainingCrushTurns { get; set; } = 0;

        /// <summary>
        /// スキル配列
        /// </summary>
        public Skill[] Skills { get; private set; } = null;

        /// <summary>
        /// 近接武器スキルIDリスト
        /// </summary>
        List<int> meleeWeaponSkillIdList = new List<int>();

        /// <summary>
        /// 遠隔武器スキルIDリスト
        /// </summary>
        List<int> rangedWeaponSkillIdList = new List<int>();

        /// <summary>
        /// 特殊スキルIDリスト
        /// </summary>
        List<int> specialSkillIdList = new List<int>();

        /// <summary>
        /// 命中
        /// </summary>
        public int Hit { get; set; } = 0;

        /// <summary>
        /// 回避
        /// </summary>
        public int Evasion { get; set; } = 0;

        /// <summary>
        /// クリティカル命中率
        /// </summary>
        public int CriticalHit { get; set; } = 0;

        /// <summary>
        /// クリティカル回避率
        /// </summary>
        public int CriticalEvasion { get; set; } = 0;

        /// <summary>
        /// 行動優先度
        /// </summary>
        public int ActionPriority { get; set; } = 0;

        /// <summary>
        /// 精神設定配列の何番目を参照しているか
        /// </summary>
        public int CurrentSoulIndex { get; set; } = 0;

        /// <summary>
        /// ステータスを初期化する
        /// </summary>
        public void InitializeStatus()
        {
            RefreshStatus();

            Condition = Attribute.Condition.Normal;
            RecoverLifeFully();
            RecoverSoulFully();
            InitializeDream();
        }

        /// <summary>
        /// ステータスを更新する
        /// </summary>
        public void RefreshStatus()
        {
            SetStatusBasedOnData();
            ApplyStatusCorrectionBasedOnLevel();
        }

        /// <summary>
        /// データに基づきステータスを設定する
        /// </summary>
        protected virtual void SetStatusBasedOnData()
        {
            if (GetData() is null)
            {
                return;
            }

            // 生命力
            MaxLife = GetData().Status.Life;

            // 夢想力
            InitialDream = GetData().Status.Dream;

            // 精神
            SoulData soul = GetCurrentSoulData();
            MaxSoul = soul.Max;
            Emotion = soul.Emotion;

            // 攻撃力
            MeleeAttack = GetData().Status.MeleeAttack;
            RangedAttack = GetData().Status.RangedAttack;

            // 守備力
            MeleeDefense = GetData().Status.MeleeDefense;
            RangedDefense = GetData().Status.RangedDefense;

            // 素早さ
            BaseSpeed = GetData().Status.Speed;

            // 近接武器スキル
            meleeWeaponSkillIdList = new List<int>(GetData().MeleeWeaponSkills);

            // 遠隔武器スキル
            rangedWeaponSkillIdList = new List<int>(GetData().RangedWeaponSkills);

            // 特殊スキル
            specialSkillIdList = new List<int>(GetData().SpecialSkills);

            // スキル
            SetSkillsBasedOnData();
        }

        /// <summary>
        /// レベルに応じたステータス補正をかける
        /// </summary>
        protected virtual void ApplyStatusCorrectionBasedOnLevel()
        {
            // 倍率
            float multiplier = GetStatusCorrectionMultiplierBasedOnLevel();

            // 最大生命力
            MaxLife = Mathf.FloorToInt(GetData().Status.Life * multiplier);

            // 近接攻撃力
            MeleeAttack = Mathf.FloorToInt(GetData().Status.MeleeAttack * multiplier);

            // 遠隔攻撃力
            RangedAttack = Mathf.FloorToInt(GetData().Status.RangedAttack * multiplier);

            // 素早さ
            BaseSpeed = Mathf.FloorToInt(GetData().Status.Speed * multiplier);
        }

        /// <summary>
        /// レベルに応じたステータス補正倍率を取得する
        /// </summary>
        /// <returns></returns>
        protected float GetStatusCorrectionMultiplierBasedOnLevel()
        {
            return 1.0f + ((Level - 1) * GameSettings.Combatants.AmountOfIncreaseInStatusPerLevel);
        }

        /// <summary>
        /// データを基にスキルを設定する
        /// </summary>
        private void SetSkillsBasedOnData()
        {
            // スキルリスト
            List<Skill> skills = new List<Skill>();

            // スキルを追加する関数
            void AddSkills(List<int> idList, System.Func<int, SkillData> func, Attribute.Skill skillAttribute)
            {
                foreach (var id in idList)
                {
                    SkillData data = func.Invoke(id);
                    Skill skill = new Skill(this, data, skillAttribute);

                    skills.Add(skill);
                }
            }

            // 近接武器スキルを加える
            AddSkills(meleeWeaponSkillIdList, MasterData.WeaponSkills.GetValue, Attribute.Skill.MeleeWeapon);

            // 遠隔武器スキルを加える
            AddSkills(rangedWeaponSkillIdList, MasterData.WeaponSkills.GetValue, Attribute.Skill.RangedWeapon);

            // 特殊スキルを加える
            AddSkills(specialSkillIdList, MasterData.SpecialSkills.GetValue, Attribute.Skill.Special);

            // 配列にコピーする
            Skills = skills.ToArray();
        }

        /// <summary>
        /// スキルを取得する
        /// </summary>
        /// <param name="attack">攻撃属性</param>
        /// <returns></returns>
        public Skill[] GetSkills(Attribute.Attack attack)
        {
            Skill[] result = Skills
                .Where(x => x.GetAttack() == attack)
                .ToArray();

            return result;
        }

        /// <summary>
        /// 生命力を全回復する
        /// </summary>
        public void RecoverLifeFully()
        {
            Life = MaxLife;
        }

        /// <summary>
        /// 精神力を全回復する
        /// </summary>
        public void RecoverSoulFully()
        {
            Soul = MaxSoul;
        }

        /// <summary>
        /// 夢想力を初期値に設定する
        /// </summary>
        public void InitializeDream()
        {
            Dream = InitialDream;
        }

        /// <summary>
        /// 現在の精神データを取得する
        /// </summary>
        /// <returns></returns>
        public SoulData GetCurrentSoulData()
        {
            SoulData soulData = GetData().Souls[CurrentSoulIndex];
            return soulData;
        }

        /// <summary>
        /// 精神を初期化する
        /// </summary>
        public void InitializeSoul()
        {
            ChangeSoul(0);
            RecoverSoulFully();
        }

        /// <summary>
        /// 精神を変更する
        /// </summary>
        public void ChangeSoul()
        {
            // 次のインデックスへ 最後のインデックスへ到達したら最初に戻る
            int nextIndex = (CurrentSoulIndex + 1) % GetData().Souls.Length;
            ChangeSoul(nextIndex);
        }

        /// <summary>
        /// 精神を変更する
        /// </summary>
        /// <param name="index">索引</param>
        public void ChangeSoul(int index)
        {
            CurrentSoulIndex = index;

            // ステータス更新
            RefreshStatus();
        }

        /// <summary>
        /// ランダムに変化する素早さを更新する
        /// </summary>
        public void UpdateRandomSpeed()
        {
            float max = GameSettings.Combatants.RandomSpeedCorrection;
            RandomSpeed = BaseSpeed * Random.Range(0.0f, max);
        }

        /// <summary>
        /// 経験値が設定されたときの処理
        /// </summary>
        private void OnExperienceSet()
        {
            if (Level == GameSettings.Combatants.MaxLevel)
            {
                return;
            }

            int previousLevel = Level;

            Level = ExperienceToLevel();

            if (Level != previousLevel)
            {
                RefreshStatus();
            }
        }

        /// <summary>
        /// 経験値をレベルに変換する
        /// </summary>
        /// <returns></returns>
        private int ExperienceToLevel()
        {
            //int level = GameSettings.Combatants.MinLevel;
            //int experience = Experience;

            //// もらえる経験値の計算

            //// 同レベルの敵を倒した場合、レベルアップに必要な撃破数
            //int defeat = 5;

            //// レベルアップに必要な基本経験値
            //int baseE = 50;

            //// レベルによる補正
            //float levelCorrectionBase = 1.1f;

            //// 種族毎の補正
            //float raceCorrection = 1.0f;

            //// 基本
            //float resultF = baseE / (float)defeat;

            //// レベル補正
            //float levelCorrection = 1.0f + (levelCorrectionBase * (Level - 1));

            //// 種族補正
            //levelCorrection *= raceCorrection;

            int result = (Experience / 2) + 1;
            return result;
        }

        /// <summary>
        /// レベルをその値に至るために必要な最低経験値に変換する
        /// </summary>
        /// <returns></returns>
        private int LevelToExperience()
        {
            // 経験値テーブルを順番に調べていく 取り出した要素以下ならそのレベルになる
            // 経験値テーブルからレベル番目の最低経験値を取得する
            int result = (Level - 1) * 2;
            result = Mathf.Max(result, 0);
            return result;
        }
    }
}
