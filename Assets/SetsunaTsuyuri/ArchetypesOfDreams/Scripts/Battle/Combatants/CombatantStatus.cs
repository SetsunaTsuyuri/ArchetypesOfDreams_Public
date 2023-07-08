using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
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
            set
            {
                _currentHP = Mathf.Clamp(value, 0, MaxHP);
                OnCurrentHPSet();
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
            set => _maxHP = Mathf.Clamp(value, 1, GameSettings.Combatants.MaxHP);
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
            set => _currentDP = Mathf.Clamp(value, 0, MaxDP);
        }

        /// <summary>
        /// 最大DP
        /// </summary>
        int _maxDP = 0;

        /// <summary>
        /// 最大DP
        /// </summary>
        public int MaxDP
        {
            get => _maxDP;
            set => _maxDP = Mathf.Clamp(value, 1, GameSettings.Combatants.MaxDP);
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
            set
            {
                _currentGP = Mathf.Clamp(value, 0, MaxGP);
                OnCurrentGPSet();
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
            set => _maxGP = Mathf.Clamp(value, 1, GameSettings.Combatants.MaxGP);
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
        public GameAttribute.Emotion Emotion { get; protected set; } = GameAttribute.Emotion.None;

        /// <summary>
        /// 通常攻撃スキル
        /// </summary>
        public ActionInfo NormalAttack { get; protected set; } = null;

        /// <summary>
        /// スキル配列
        /// </summary>
        public ActionInfo[] Skills { get; protected set; } = null;

        /// <summary>
        /// ステータス効果リスト
        /// </summary>
        [field: SerializeField]
        public List<StatusEffect> StatusEffects { get; protected set; } = new();

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
        /// 待機時間
        /// </summary>
        public int WaitTime { get; set; } = 0;

        /// <summary>
        /// DP増加タイマー
        /// </summary>
        public int DPRegainingTimer { get; protected set; } = 0;

        /// <summary>
        /// 待機時間の基本値
        /// </summary>
        /// <returns></returns>
        public int BasicWaitTime => GameSettings.Combatants.MaxWaitTime / Speed;

        /// <summary>
        /// HPの割合
        /// </summary>
        /// <returns></returns>
        public float HPRate => (float)CurrentHP / MaxHP;

        /// <summary>
        /// HPの減少率
        /// </summary>
        /// <returns></returns>
        public float HPDecreaseRate => 1.0f - HPRate;

        /// <summary>
        /// DPの割合
        /// </summary>
        /// <returns></returns>
        public float DPRate => (float)CurrentDP / MaxDP;

        /// <summary>
        /// DPの減少率
        /// </summary>
        /// <returns></returns>
        public float DPDecreaseRate => 1.0f - DPRate;

        /// <summary>
        /// GPの割合
        /// </summary>
        /// <returns></returns>
        public float GPRate => (float)CurrentGP / MaxGP;

        /// <summary>
        /// GPの減少率
        /// </summary>
        /// <returns></returns>
        public float GPDecreaseRate => 1.0f - GPRate;

        /// <summary>
        /// HPが設定されたときの処理 
        /// </summary>
        public void OnCurrentHPSet()
        {
            EffectData.StatusEffect effect = GameSettings.Combatants.EffectHP0;

            if (CurrentHP == 0)
            {
                AddStatusEffect(effect);
            }
            else if (CurrentHP > 0)
            {
                RemoveStatusEffects(effect.StatusEffectId);
            }
        }

        /// <summary>
        /// GPが設定されたときの処理
        /// </summary>
        public void OnCurrentGPSet()
        {
            EffectData.StatusEffect effect = GameSettings.Combatants.EffectGP0;

            if (CurrentGP == 0)
            {
                AddStatusEffect(effect);
            }
            else if (CurrentGP > 0)
            {
                RemoveStatusEffects(effect.StatusEffectId);
            }
        }

        /// <summary>
        /// ステータスを初期化する
        /// </summary>
        public void InitializeStatus()
        {
            RefreshStatus();
            RecoverHP();
            RecoverDP();
            RecoverGP();
            WaitTime = 0;
            DPRegainingTimer = 0;
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
            ApplyStatusCorrectionBasedOnParty();
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
        private void ApplyStatusCorrectionBasedOnLevel()
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
        /// 敵味方の違いによるステータス補正をかける
        /// </summary>
        private void ApplyStatusCorrectionBasedOnParty()
        {
            if (Container is EnemyContainer)
            {
                ApplyEnemyStatusCorrection();
            }
        }

        /// <summary>
        /// 敵ステータス補正をかける
        /// </summary>
        private void ApplyEnemyStatusCorrection()
        {
            // ボス耐性持ちは補正なし
            if (HasBossResistance)
            {
                return;
            }

            MaxHP = Mathf.FloorToInt(MaxHP * GameSettings.Enemies.HPScale);
            MaxGP += GameSettings.Enemies.GPValue;
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
            NormalAttack = new(MasterData.GetSkillData(BasicSkillId.Attack));

            // スキルリスト
            List<ActionInfo> skillList = new();

            foreach (var skillAcquisition in Data.Skills)
            {
                if (skillAcquisition.AcquisitionLevel >= Level)
                {
                    SkillData skillData = MasterData.GetSkillData(skillAcquisition.SkillId);
                    ActionInfo skill = new(skillData);
                    skillList.Add(skill);
                }
            }

            // 配列に入れる
            Skills = skillList.ToArray();
        }

        /// <summary>
        /// 習得スキルを取得する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetAcquisitionSkillIds()
        {
            return Data.Skills
                .Select(x => x.SkillId);
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
        /// ステータスの現在値を取得する
        /// </summary>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public int GetCurrentStatus(AffectedStatusType statusType)
        {
            int result = statusType switch
            {
                AffectedStatusType.HP => CurrentHP,
                AffectedStatusType.DP => CurrentDP,
                AffectedStatusType.GP => CurrentGP,
                _ => 0
            };

            return result;
        }

        /// <summary>
        /// ステータスの最大値を取得する
        /// </summary>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public int GetMaxStatus(AffectedStatusType statusType)
        {
            int result = statusType switch
            {
                AffectedStatusType.HP => MaxHP,
                AffectedStatusType.DP => MaxDP,
                AffectedStatusType.GP => MaxGP,
                _ => 0
            };

            return result;
        }
    }
}
