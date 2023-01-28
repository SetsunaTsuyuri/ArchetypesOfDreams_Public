using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Combatant 
    {
        /// <summary>
        /// 行動を決定する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public ActionInfo DecideAction(Battle battle)
        {
            ActionInfo result = null;

            // 行動できないならnullを返す
            if (!CanAct())
            {
                return null;
            }

            // 使用可能な通常攻撃及びスキルを抽出し、ランダムに選ぶ
            ActionInfo[] availables = Skills
                .Append(NormalAttack)
                .Where(x => x.CanBeExecuted(battle))
                .ToArray();

            if (availables.Any())
            {
                int index = Random.Range(0, availables.Length);
                result = availables[index];
            }

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
