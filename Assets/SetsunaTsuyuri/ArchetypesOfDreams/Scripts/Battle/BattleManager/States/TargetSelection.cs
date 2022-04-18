using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class BattleManager
    {
        /// <summary>
        /// 対象選択開始時のイベント
        /// </summary>
        [SerializeField]
        GameEventWithBattleManager onTargetSelectionEnter = null;

        /// <summary>
        /// 対象選択終了時のイベント
        /// </summary>
        [SerializeField]
        GameEvent onTargetSelectionExit = null;

        /// <summary>
        /// 行動の対象にできる戦闘者コンテナ配列
        /// </summary>
        public CombatantContainer[] Targetables { get; private set; } = null;

        /// <summary>
        /// 行動の対象
        /// </summary>
        int targetIndex = 0;

        /// <summary>
        /// 対象が単体ではない
        /// </summary>
        bool targetIsNotSingle = false;

        /// <summary>
        /// ターゲット選択
        /// </summary>
        private class TargetSelection : FiniteStateMachine<BattleManager>.State
        {
            public override void Enter(BattleManager context)
            {
                if (context.SkillToBeUsed is null)
                {
                    Debug.LogError("SkillToBeUsed is null");
                    return;
                }

                // 対象候補コンテナ更新
                context.UpdateTargetables();

                // 対象選択時に必要な行動の結果を作る
                context.MakeActionResultOnTargetSelection();

                // 対象フラグ更新
                context.UpdateTargetFlags();

                // ゲームイベント呼び出し
                context.onTargetSelectionEnter.Invoke(context);
            }

            public override void Exit(BattleManager context)
            {
                // 対象候補コンテナの行動結果を初期化する
                foreach (var targetable in context.Targetables)
                {
                    targetable.Combatant.Result.Initialize();
                }

                // 敵スプライトの色変化を取り除く
                foreach (var enemy in context.Enemies.Members)
                {
                    enemy.EnemySprite.KillTargetedColorTween();
                }

                // ゲームイベント呼び出し
                context.onTargetSelectionExit.Invoke();
            }
        }

        /// <summary>
        /// 対象候補の戦闘者コンテナを更新する
        /// </summary>
        private void UpdateTargetables()
        {
            Targetables = GetTargetables(SkillToBeUsed.Data.Effect);
        }

        /// <summary>
        /// 各戦闘者コンテナの対象フラグを更新する
        /// </summary>
        private void UpdateTargetFlags()
        {
            // スキルが設定されていないか、
            // 対象にできるコンテナが存在しないなら中止する
            if (SkillToBeUsed is null ||
                !Targetables.Any())
            {
                return;
            }

            // 単体を対象としていない場合
            if (SkillToBeUsed.Data.Effect.TargetSelection != Effect.TargetSelection.Single)
            {
                // 非単体対象フラグON
                targetIsNotSingle = true;

                // 対象にできるコンテナ全てを対象とする
                foreach (var target in Targetables)
                {
                    target.IsTargeted = true;
                }
            }
            else
            {
                // 非単体対象フラグOFF
                targetIsNotSingle = false;

                // プレイヤーが操作可能な戦闘者の場合
                if (Performer.ContainsPlayerControlled())
                {
                    // 0番目を対象とする
                    targetIndex = 0;
                }
                else
                {
                    // AIが自動的に対象を決定する
                    targetIndex = Performer.Combatant.DecideTargetIndex(Targetables);
                }

                Targetables[targetIndex].IsTargeted = true;
            }
        }

        /// <summary>
        /// 対象選択時に必要な行動結果を作る
        /// </summary>
        private void MakeActionResultOnTargetSelection()
        {
            Performer.Combatant.MakeActionResultOnTargetSelection(SkillToBeUsed, Targetables);
        }

        /// <summary>
        /// 対象にできる戦闘者コンテナを取得する
        /// </summary>
        /// <param name="effect">効果データ</param>
        /// <returns></returns>
        private CombatantContainer[] GetTargetables(EffectData effect)
        {
            return effect.TargetPosition switch
            {
                // 敵
                Effect.TargetPosition.Enemies => GetPerformersTargetableEnemies(),

                // 味方
                Effect.TargetPosition.Allies => GetPerformersTargetableAllies(),

                // 敵味方両方
                Effect.TargetPosition.Both => Enemies.GetTargetables()
                                        .Concat(Allies.GetTargetables())
                                        .ToArray(),
                // 自分
                Effect.TargetPosition.Oneself => new CombatantContainer[1] { Performer },

                // 自分以外の味方
                Effect.TargetPosition.AlliesOtherThanOneself => GetPerformersTargetableAllies()
                                        .Where(x => x != Performer)
                                        .ToArray(),
                // 自分以外の敵味方
                Effect.TargetPosition.OtherThanOneself => Enemies.GetTargetables()
                                        .Concat(Allies.GetTargetables())
                                        .Where(x => x != Performer)
                                        .ToArray(),
                // 控え
                Effect.TargetPosition.Reserves => Allies.ReserveMembers
                                        .Where(x => x.ContainsChangeable())
                                        .ToArray(),

                // その他 空配列を返す
                _ => new CombatantContainer[0]
            };
        }

        /// <summary>
        /// 対象にできる戦闘者コンテナが存在するか、対象を指定しない効果である
        /// </summary>
        /// <param name="effect">効果データ</param>
        /// <returns></returns>
        public bool ExistsTargetableOrTargetOfEffectIsNone(EffectData effect)
        {
            return GetTargetables(effect).Any() || effect.TargetPosition == Effect.TargetPosition.None;
        }

        /// <summary>
        /// 行動者の敵で対象にできる戦闘者コンテナ配列を取得する
        /// </summary>
        /// <returns></returns>
        private CombatantContainer[] GetPerformersTargetableEnemies()
        {
            if (Performer is AllyContainer)
            {
                return Enemies.GetTargetables();
            }
            else
            {
                return Allies.GetTargetables();
            }
        }

        /// <summary>
        /// 行動者の味方で対象にできる戦闘者コンテナ配列を取得する
        /// </summary>
        /// <returns></returns>
        private CombatantContainer[] GetPerformersTargetableAllies()
        {
            if (Performer is AllyContainer)
            {
                return Allies.GetTargetables();
            }
            else
            {
                return Enemies.GetTargetables();
            }
        }
    }
}
