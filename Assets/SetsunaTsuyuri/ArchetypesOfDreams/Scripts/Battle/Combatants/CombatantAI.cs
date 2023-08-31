using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Combatant
    {
        /// <summary>
        /// ターンカウンター
        /// </summary>
        public int TurnCounter { get; protected set; } = 0;

        /// <summary>
        /// AIを初期化する
        /// </summary>
        public void InitializeAI()
        {
            TurnCounter = 0;
        }

        /// <summary>
        /// 行動を決定する
        /// </summary>
        public ActionInfo DecideAction()
        {
            ActionInfo result = null;
            if (!CanAct())
            {
                return result;
            }

            // AIデータで決める
            if (Data.AIElements.Any())
            {
                result = DecideActionByAIElements();
            }
            else
            {
                // 使用可能な通常攻撃及びスキルを抽出し、ランダムに選ぶ
                ActionInfo[] availables = Skills
                    .Append(NormalAttack)
                    .Where(x => Container.CanUse(x))
                    .ToArray();

                if (availables.Any())
                {
                    int index = Random.Range(0, availables.Length);
                    result = availables[index];
                }
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

        /// <summary>
        /// 行動を決定する
        /// </summary>
        /// <returns></returns>
        private ActionInfo DecideActionByAIElements()
        {
            // 条件を満たす行動内容リストを作る
            List<(int priority, ActionInfo action)> actions = new();
            var elements = Data.AIElements.Where(x => MatchCondition(x));
            foreach (var element in elements)
            {
                ActionInfo action = new(MasterData.GetSkillData(element.SkillId));
                actions.Add((element.Priority, action));
            }

            // 行動内容を1つ選ぶ
            ActionInfo result = actions
                .Where(x => Container.CanUse(x.action))
                .OrderBy(x => x.priority)
                .ThenByRandom()
                .Select(x => x.action)
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 条件に一致する
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool MatchCondition(CombatantAIElementData element)
        {
            bool result = element.Condition switch
            {
                AIConditionType.Unconditional => true,
                AIConditionType.Turn => MatchTurnCondition(element),
                _ => false
            };

            return result;
        }

        /// <summary>
        /// ターンによる条件に一致する
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool MatchTurnCondition(CombatantAIElementData element)
        {
            bool result = false;

            // ターン数 = ConditionA * n + ConditionBが成り立つならtrue
            if ((TurnCounter - element.ConditionParameterB) % element.ConditionParameterA == 0)
            {
                result = true;
            }

            return result;
        }
    }
}
