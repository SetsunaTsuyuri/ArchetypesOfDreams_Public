using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Battle
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
        private class TargetSelection : StateMachine<Battle>.State
        {
            public override void Enter(Battle context)
            {
                if (context.ActorAction is null)
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

            public override void Exit(Battle context)
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
            Targetables = GetTargetables(ActorAction.Effect);
        }

        /// <summary>
        /// 各戦闘者コンテナの対象フラグを更新する
        /// </summary>
        private void UpdateTargetFlags()
        {
            // スキルが設定されていないか、
            // 対象にできるコンテナが存在しないなら中止する
            if (ActorAction is null ||
                !Targetables.Any())
            {
                return;
            }

            // 単体を対象としていない場合
            if (ActorAction.Effect.TargetSelection != TargetSelectionType.Single)
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
                if (Actor.ContainsPlayerControlled())
                {
                    // 0番目を対象とする
                    targetIndex = 0;
                }
                else
                {
                    // AIが自動的に対象を決定する
                    targetIndex = Actor.Combatant.DecideTargetIndex(Targetables);
                }

                Targetables[targetIndex].IsTargeted = true;
            }
        }

        /// <summary>
        /// 対象選択時に必要な行動結果を作る
        /// </summary>
        private void MakeActionResultOnTargetSelection()
        {
            Actor.Combatant.MakeActionResultOnTargetSelection(ActorAction, Targetables);
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
                TargetPosition.Enemies => GetActorsTargetableEnemies(effect.TargetCondition),

                // 味方
                TargetPosition.Allies => GetActorsTargetableAllies(effect.TargetCondition),

                // 敵味方両方
                TargetPosition.Both => Enemies.GetTargetables(effect.TargetCondition)
                                        .Concat(Allies.GetTargetables(effect.TargetCondition))
                                        .ToArray(),
                // 自分
                TargetPosition.Oneself => new CombatantContainer[1] { Actor },

                // 自分以外の味方
                TargetPosition.AlliesOtherThanOneself => GetActorsTargetableAllies(effect.TargetCondition)
                                        .Where(x => x != Actor)
                                        .ToArray(),
                // 自分以外の敵味方
                TargetPosition.OtherThanOneself => Enemies.GetTargetables(effect.TargetCondition)
                                        .Concat(Allies.GetTargetables(effect.TargetCondition))
                                        .Where(x => x != Actor)
                                        .ToArray(),
                // 控え
                TargetPosition.Reserves => Allies.ReserveMembers
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
        public bool ExistsTargetableOrTargetPositionIsNone(EffectData effect)
        {
            return GetTargetables(effect).Any()
                || effect.TargetPosition == TargetPosition.None;
        }

        /// <summary>
        /// 行動者の敵で対象にできる戦闘者コンテナ配列を取得する
        /// </summary>
        /// <param name="condition">対象にできる戦闘者の状態</param>
        /// <returns></returns>
        private CombatantContainer[] GetActorsTargetableEnemies(TargetCondition condition)
        {
            if (Actor is AllyContainer)
            {
                return Enemies.GetTargetables(condition);
            }
            else
            {
                return Allies.GetTargetables(condition);
            }
        }

        /// <summary>
        /// 行動者の味方で対象にできる戦闘者コンテナ配列を取得する
        /// </summary>
        /// <param name="condition">対象にできる戦闘者の状態</param>
        /// <returns></returns>
        private CombatantContainer[] GetActorsTargetableAllies(TargetCondition condition)
        {
            if (Actor is AllyContainer)
            {
                return Allies.GetTargetables(condition);
            }
            else
            {
                return Enemies.GetTargetables(condition);
            }
        }
    }
}
