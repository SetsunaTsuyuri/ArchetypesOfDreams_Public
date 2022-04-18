using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Combatant 
    {
        /// <summary>
        /// 使用するスキルを決定する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public Skill DecideTheSkillToBeUsed(BattleManager battle)
        {
            Skill result = null;

            // 行動できないならnullを返す
            if (!CanAct())
            {
                return null;
            }

            // 現在使用可能なスキルを抽出し、ランダムに選ぶ
            Skill[] availables = GetAvailableSkills(battle);
            if (availables.Any())
            {
                int index = Random.Range(0, availables.Length);
                result = availables[index];
            }

            return result;
        }

        /// <summary>
        /// 現在使用可能なスキルを全て取得する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <returns></returns>
        private Skill[] GetAvailableSkills(BattleManager battle)
        {
            Skill[] result = Skills
                .Where(x => x.CanBeUsed(battle))
                .ToArray();

            return result;
        }

        /// <summary>
        /// 対象を決定する
        /// </summary>
        /// <param name="targetables">対象にできる戦闘者コンテナ配列</param>
        /// <returns></returns>
        public int DecideTargetIndex(CombatantContainer[] targetables)
        {
            // ランダムに選ぶ
            int result = Random.Range(0, targetables.Length);

            return result;
        }
    }
}
