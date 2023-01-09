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
                _currentDP = Mathf.Clamp(value, 0, MaxDP);
            }
        }

        /// <summary>
        /// 最大DP
        /// </summary>
        int _maxDP = 0;

        public int MaxDP
        {
            get => _maxDP;
            protected set
            {
                _maxDP = Mathf.Clamp(value, 0, GameSettings.Combatants.MaxDP);
            }
        }

        /// <summary>
        /// 現在GP
        /// </summary>
        int _currentGP = 0;

        /// <summary>
        /// 現在GP
        /// </summary>
        public int CurrentGP
        {
            get => _currentGP;
            protected set
            {
                _currentGP = Mathf.Clamp(value, 0, MaxGP);
            }
        }

        /// <summary>
        /// 最大GP
        /// </summary>
        int _maxGP = 0;

        /// <summary>
        /// 最大GP
        /// </summary>
        public int MaxGP
        {
            get => _maxGP;
            protected set
            {
                _maxGP = Mathf.Clamp(value, 0, GameSettings.Combatants.MaxGP);
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
        public int Speed { get; protected set; } = 0;

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
        public int Accuracy { get; protected set; } = 0;

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
        /// GP増減値
        /// </summary>
        public int GPChangeValue { get; protected set; } = 0;

        /// <summary>
        /// ステータスを初期化する
        /// </summary>
        public void InitializeStatus()
        {
            RefreshStatus();

            Condition = Attribute.Condition.Normal;
            RecoverHP();
            RecoverDP();
            RecoverGP();
        }

        /// <summary>
        /// ステータスを更新する
        /// </summary>
        public void RefreshStatus()
        {
            HPChangeRate = 0;
            DPChangeValue = 0;
            GPChangeValue = 0;
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

            // 感情属性
            Emotion = Data.Emotion;

            // 最大HP
            MaxHP = Data.HP;

            // 最大DP
            MaxDP = Data.DP;

            // 最大GP
            MaxGP = Data.GP;

            // 力
            Power = Data.Power;

            // 技
            Technique = Data.Technique;

            // 素早さ
            Speed = Data.Speed;

            // 命中
            Accuracy = Data.Accuracy;

            // 回避
            Evasion = Data.Evasion;
        }

        /// <summary>
        /// レベルに応じたステータス補正をかける
        /// </summary>
        protected virtual void ApplyStatusCorrectionBasedOnLevel()
        {
            // 倍率
            float multiplier = 1.0f + ((Level - 1) * GameSettings.Combatants.AmountOfIncreaseInStatusPerLevel);

            // 最大HP
            MaxHP = Mathf.FloorToInt(Data.HP * multiplier);

            // 力
            Power = Mathf.FloorToInt(Data.Power * multiplier);

            // 技
            Technique = Mathf.FloorToInt(Data.Technique * multiplier);
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

            // GP増減値
            GPChangeValue += data.GPChangeValue;

            // 命中
            Accuracy += data.Accuracy;

            // 回避
            Evasion += data.Evasion;

            // クリティカル命中
            CriticalHit += data.CriticalAccuracy;

            // クリティカル回避
            CriticalEvasion += data.CriticalEvasion;
        }

        /// <summary>
        /// IDリストを基にスキルを設定する
        /// </summary>
        private void SetSkillsBasedOnIdList()
        {
            // 通常攻撃
            NormalAttack = new(MasterData.GetSkillData(BasicSkillType.Attack), Data.NormalAttack);

            // スキルリスト
            List<ActionModel> skillList = new();

            foreach (var skillAcquisition in Data.Skills)
            {
                if (skillAcquisition.AcquisitionLevel >= Level)
                {
                    SkillData skillData = MasterData.GetSkillData(skillAcquisition.SkillId);
                    ActionModel skill = new(skillData, Attribute.Skill.PowerSkill);
                    skillList.Add(skill);
                }
            }

            // 配列に入れる
            Skills = skillList.ToArray();
        }

        /// <summary>
        /// HPを全回復する
        /// </summary>
        public void RecoverHP()
        {
            CurrentHP = MaxHP;
        }

        /// <summary>
        /// DPを全回復する
        /// </summary>
        public void RecoverDP()
        {
            CurrentDP = MaxDP;
        }

        /// <summary>
        /// GPを全回復する
        /// </summary>
        public void RecoverGP()
        {
            CurrentGP = MaxGP;
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
