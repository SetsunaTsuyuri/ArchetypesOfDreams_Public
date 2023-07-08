using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者の能力
    /// </summary>
    public partial class Combatant
    {
        /// <summary>
        /// 力依存の攻撃倍率
        /// </summary>
        public float PowerAttackScale
        {
            get
            {
                float scale = StatusEffects
                    .Select(x => x.Data.PowerAttackRate)
                    .Sum();

                float result = Mathf.Max(0.0f, 1.0f + scale);
                return result;
            }
        }

        /// <summary>
        /// 技依存の攻撃倍率
        /// </summary>
        public float TechniqueAttackScale
        {
            get
            {
                float scale = StatusEffects
                    .Select(x => x.Data.TechniqueAttackRate)
                    .Sum();

                float result = Mathf.Max(0.0f, 1.0f + scale);
                return result;

            }
        }

        /// <summary>
        /// 力依存の被ダメージ倍率
        /// </summary>
        public float PowerDamageScale
        {
            get
            {
                float scale = StatusEffects
                    .Select(x => x.Data.PowerDefenseRate)
                    .Sum();

                float result = Mathf.Max(0.0f, 1.0f - scale);
                return result;
            }
        }

        /// <summary>
        /// 技依存の被ダメージ倍率
        /// </summary>
        public float TechniqueDamageScale
        {
            get
            {
                float scale = StatusEffects
                    .Select(x => x.Data.TechniqueDefenseRate)
                    .Sum();

                float result = Mathf.Max(0.0f, 1.0f - scale);
                return result;
            }
        }

        /// <summary>
        /// ステータス効果に対する耐性を取得する
        /// </summary>
        /// <param name="statusEffectId"></param>
        /// <returns></returns>
        public int GetStatusEffectResistance(int statusEffectId)
        {
            int registance = CreateAbilityList(AbilityType.StatusEffectResistance)
                .Where(x => x.ParameterA == statusEffectId)
                .Select(x => x.ParameterB)
                .Sum();

            int result = Mathf.Clamp(registance, -1000, 1000);
            return result;
        }

        /// <summary>
        /// 能力リストを作る
        /// </summary>
        /// <param name="abilityType"></param>
        /// <returns></returns>
        public List<AbilityData> CreateAbilityList(AbilityType abilityType)
        {
            List<AbilityData> abilityList = new();

            // TODO: 戦闘者本体

            // ステータス効果
            var statusEffectAbilitiesCollection = StatusEffects
                .Where(x => x.Data.Abilities.Any(x => x.Type == abilityType))
                .Select(x => x.Data.Abilities);

            foreach (var statusEffectAbilities in statusEffectAbilitiesCollection)
            {
                abilityList.AddRange(statusEffectAbilities);
            }

            return abilityList;
        }

        /// <summary>
        /// 致死ダメージをHP1で耐える判定を行う
        /// </summary>
        /// <returns></returns>
        public bool JudgeSurvivour()
        {
            List<AbilityData> abilityList = CreateAbilityList(AbilityType.Survivor);
            bool result = abilityList.Any(x => RandomUtility.Percent(x.ParameterA));
            return result;
        }

        /// <summary>
        /// スキルが封じられている
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SkillIsSealed(int id)
        {
            List<AbilityData> abilityList = CreateAbilityList(AbilityType.SkillSealing);
            bool result = abilityList.Any(x => x.ParameterA == id);
            return result;
        }
    }
}
