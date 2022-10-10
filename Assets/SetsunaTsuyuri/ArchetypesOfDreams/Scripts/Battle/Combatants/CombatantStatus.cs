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
        int _experience = 0;

        /// <summary>
        /// 経験値
        /// </summary>
        public int Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                OnExperienceSet();
            }
        }

        /// <summary>
        /// 現在HP
        /// </summary>
        int _currentHP = 0;

        /// <summary>
        /// 現在HP
        /// </summary>
        public int CurrentHP
        {
            get => _currentHP;
            protected set
            {
                _currentHP = Mathf.Clamp(value, 0, MaxHP);
            }
        }

        /// <summary>
        /// 最大HP
        /// </summary>
        int _maxHP;

        /// <summary>
        /// 最大HP
        /// </summary>
        public int MaxHP
        {
            get => _maxHP;
            protected set
            {
                _maxHP = Mathf.Clamp(value, 0, GameSettings.Combatants.MaxHP);
            }
        }

        /// <summary>
        /// 現在DP
        /// </summary>
        int _currentDP = 0;

        /// <summary>
        /// 現在DP
        /// </summary>
        public int CurrentDP
        {
            get => _currentDP;
            protected set
            {
                _currentDP = Mathf.Clamp(value, 0, GameSettings.Combatants.MaxDP);
            }
        }

        /// <summary>
        /// 初期DP
        /// </summary>
        public int InitialDP { get; protected set; } = 0;

        /// <summary>
        /// 現在SP
        /// </summary>
        int _currentSP = 0;

        /// <summary>
        /// 現在SP
        /// </summary>
        public int CurrentSP
        {
            get => _currentSP;
            protected set
            {
                _currentSP = Mathf.Clamp(value, 0, MaxSP);
            }
        }

        /// <summary>
        /// 最大SP
        /// </summary>
        int _maxSP = 0;

        /// <summary>
        /// 最大SP
        /// </summary>
        public int MaxSP
        {
            get => _maxSP;
            protected set
            {
                _maxSP = Mathf.Clamp(value, 0, GameSettings.Combatants.MaxSP);
            }
        }

        /// <summary>
        /// 力
        /// </summary>
        public int Power { get; protected set; } = 0;

        /// <summary>
        /// 技
        /// </summary>
        public int Technique { get; protected set; } = 0;

        /// <summary>
        /// 素早さ
        /// </summary>
        public int Agility { get; protected set; } = 0;

        /// <summary>
        /// 感情属性
        /// </summary>
        public Attribute.Emotion Emotion { get; protected set; } = Attribute.Emotion.None;

        /// <summary>
        /// クラッシュ状態から復帰するまでの残りターン数
        /// </summary>
        public int RemainingCrushTurns { get; protected set; } = 0;

        /// <summary>
        /// 通常攻撃スキル
        /// </summary>
        public ActionModel NormalAttack { get; protected set; } = null;

        /// <summary>
        /// スキル配列
        /// </summary>
        public ActionModel[] Skills { get; protected set; } = null;

        /// <summary>
        /// STR依存スキルIDリスト
        /// </summary>
        List<int> _strengthSkillIdList = new();

        /// <summary>
        /// TEC依存スキルIDリスト
        /// </summary>
        List<int> _techniqueSkillIdList = new();

        /// <summary>
        /// 特殊スキルIDリスト
        /// </summary>
        List<int> _specialSkillIdList = new();

        /// <summary>
        /// ステータス効果リスト
        /// </summary>
        public readonly List<StatusEffect> StatusEffects = new();

        /// <summary>
        /// 命中
        /// </summary>
        public int Hit { get; protected set; } = 0;

        /// <summary>
        /// 回避
        /// </summary>
        public int Evasion { get; protected set; } = 0;

        /// <summary>
        /// クリティカル命中
        /// </summary>
        public int CriticalHit { get; protected set; } = 0;

        /// <summary>
        /// クリティカル回避
        /// </summary>
        public int CriticalEvasion { get; protected set; } = 0;

        /// <summary>
        /// HP増減率
        /// </summary>
        public int HPChangeRate { get; protected set; } = 0;

        /// <summary>
        /// DP増減値
        /// </summary>
        public int DPChangeValue { get; protected set; } = 0;

        /// <summary>
        /// SP増減値
        /// </summary>
        public int SPChangeValue { get; protected set; } = 0;

        /// <summary>
        /// 精神配列の索引
        /// </summary>
        public int CurrentSoulIndex { get; protected set; } = 0;

        /// <summary>
        /// ステータスを初期化する
        /// </summary>
        public void InitializeStatus()
        {
            RefreshStatus();

            Condition = Attribute.Condition.Normal;
            RecoverHp();
            RecoverSp();
            InitializeDp();
        }

        /// <summary>
        /// ステータスを更新する
        /// </summary>
        public void RefreshStatus()
        {
            HPChangeRate = 0;
            DPChangeValue = 0;
            SPChangeValue = 0;
            CriticalHit = 0;
            CriticalEvasion = 0;

            SetStatusBasedOnData();
            ApplyStatusCorrectionBasedOnLevel();
            ApplyStatusEffects();

            SetSkillsBasedOnIdList();
        }

        /// <summary>
        /// データに基づきステータスを設定する
        /// </summary>
        protected virtual void SetStatusBasedOnData()
        {
            if (Data is null)
            {
                return;
            }

            // 最大HP
            MaxHP = Data.Status.HP;

            // 初期DP
            InitialDP = Data.Status.DP;

            SoulData soul = GetCurrentSoulData();

            // 最大SP
            MaxSP = soul.Max;

            // 感情属性
            Emotion = soul.Emotion;

            // STR
            Power = Data.Status.Power;

            // TEC
            Technique = Data.Status.Technique;

            // AGI
            Agility = Data.Status.Speed;

            // STR依存武器スキルIDリスト
            _strengthSkillIdList = new List<int>(Data.StrengthSkills);

            // TEC依存武器スキルIDリスト
            _techniqueSkillIdList = new List<int>(Data.TechniqueSkills);

            // 特殊スキルIDリスト
            _specialSkillIdList = new List<int>(Data.SpecialSkills);
        }

        /// <summary>
        /// レベルに応じたステータス補正をかける
        /// </summary>
        protected virtual void ApplyStatusCorrectionBasedOnLevel()
        {
            // 倍率
            float multiplier = 1.0f + ((Level - 1) * GameSettings.Combatants.AmountOfIncreaseInStatusPerLevel);

            // 最大HP
            MaxHP = Mathf.FloorToInt(Data.Status.HP * multiplier);

            // STR
            Power = Mathf.FloorToInt(Data.Status.Power * multiplier);

            // TEC
            Technique = Mathf.FloorToInt(Data.Status.Technique * multiplier);
        }

        /// <summary>
        /// ステータス効果を適用する
        /// </summary>
        private void ApplyStatusEffects()
        {
            foreach (var statusEffect in StatusEffects)
            {
                ApplyStatusEffectData(statusEffect.Data);
            }
        }

        /// <summary>
        /// ステータス効果を適用する
        /// </summary>
        /// <param name="data">ステータス効果データ</param>
        private void ApplyStatusEffectData(StatusEffectData data)
        {
            // HP増減率
            HPChangeRate += data.HPChangeRate;

            // DP増減値
            DPChangeValue += data.DPChangeValue;

            // SP増減値
            SPChangeValue += data.SPChangeValue;

            // 命中
            Hit += data.Hit;

            // 回避
            Evasion += data.Evasion;

            // クリティカル命中
            CriticalHit += data.CriticalHit;

            // クリティカル回避
            CriticalEvasion += data.CriticalEvasion;
        }

        /// <summary>
        /// IDリストを基にスキルを設定する
        /// </summary>
        private void SetSkillsBasedOnIdList()
        {
            // 通常攻撃
            NormalAttack = new(MasterData.Skills.NormalAttack, Data.NormalAttack);

            // スキルリスト
            List<ActionModel> skillList = new();

            // スキルを追加する関数
            void AddSkills(List<int> idList, Attribute.Skill skillAttribute)
            {
                foreach (var id in idList)
                {
                    SkillData data = MasterData.Skills[id];
                    ActionModel skill = new(data, skillAttribute);

                    skillList.Add(skill);
                }
            }

            // 力依存スキルを加える
            AddSkills(_strengthSkillIdList, Attribute.Skill.PowerSkill);

            // 技依存スキルを加える
            AddSkills(_techniqueSkillIdList, Attribute.Skill.TechniqueSkill);

            // 特殊スキルを加える
            AddSkills(_specialSkillIdList, Attribute.Skill.Special);

            // 配列に入れる
            Skills = skillList.ToArray();
        }

        /// <summary>
        /// スキルを取得する
        /// </summary>
        /// <param name="attack">攻撃属性</param>
        /// <returns></returns>
        public ActionModel[] GetSkills(Attribute.Attack attack)
        {
            ActionModel[] result = Skills
                .Where(x => x.GetAttack() == attack)
                .ToArray();

            return result;
        }

        /// <summary>
        /// HPを全回復する
        /// </summary>
        public void RecoverHp()
        {
            CurrentHP = MaxHP;
        }

        /// <summary>
        /// SPを全回復する
        /// </summary>
        public void RecoverSp()
        {
            CurrentSP = MaxSP;
        }

        /// <summary>
        /// DPを初期値に設定する
        /// </summary>
        public void InitializeDp()
        {
            CurrentDP = InitialDP;
        }

        /// <summary>
        /// 現在の精神データを取得する
        /// </summary>
        /// <returns></returns>
        public SoulData GetCurrentSoulData()
        {
            SoulData soulData = Data.Souls[CurrentSoulIndex];
            return soulData;
        }

        /// <summary>
        /// 精神を初期化する
        /// </summary>
        public void InitializeSoul()
        {
            ChangeSoul(0);
            RecoverSp();
        }

        /// <summary>
        /// 精神を変更する
        /// </summary>
        public void ChangeSoul()
        {
            // 次のインデックスへ 最後のインデックスへ到達したら最初に戻る
            int nextIndex = (CurrentSoulIndex + 1) % Data.Souls.Length;
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
        /// 経験値が設定されたときの処理
        /// </summary>
        private void OnExperienceSet()
        {
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
            int level = GameSettings.Combatants.MinLevel;
            while (Experience >= ToMinExperience(level + 1))
            {
                level++;
            }

            return level;
        }

        /// <summary>
        /// そのレベルに至るために必要な最低経験値を求める
        /// </summary>
        /// <param name="level">レベル</param>
        /// <returns></returns>
        public int ToMinExperience(int level)
        {
            // 戦闘者の設定
            CombatantsSettings combatants = GameSettings.Combatants;

            // 最小レベル
            int minLevel = combatants.MinLevel;

            // 基本値
            int baseValue = combatants.ExperienceRequiredToLevelUp;

            // 増加倍率
            float rate = combatants.PercentageIncreaceInExperienceRequiredToLevelUp;

            // 合計値
            int sum = 0;

            for (int i = minLevel; i < level; i++)
            {
                // 実際の倍率
                float levelRate = rate * (i - 1) + 1.0f;

                // 経験値を合計値に加える
                int experience = Mathf.FloorToInt(baseValue * levelRate);
                sum += experience;
            }

            return sum;
        }

        /// <summary>
        /// 次のレベルに到達するまでに必要な経験値を取得する
        /// </summary>
        /// <returns></returns>
        public int GetNextLevelExperience()
        {
            // 次のレベルまでに必要な最低経験値
            int min = ToMinExperience(Level + 1);

            // 現在の経験値から最低経験値を引く
            int next = Experience - min;
            return next;
        }

        /// <summary>
        /// 敵として倒された場合に得られる経験値を取得する
        /// </summary>
        /// <returns></returns>
        public int GetRewardExperience()
        {
            int experience = Data.RewardExperience;
            float increace = GameSettings.Combatants.PercentageIncreaceInExperienceRequiredToLevelUp;
            float rate = (Level - 1) * increace + 1.0f;
            int result = Mathf.FloorToInt(experience * rate);
            return result;
        }
    }
}
