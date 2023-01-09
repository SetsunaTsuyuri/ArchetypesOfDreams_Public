using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者を格納するコンテナ
    /// </summary>
    public abstract class CombatantContainer : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// 戦闘者
        /// </summary>
        Combatant combatant = null;

        /// <summary>
        /// 戦闘者
        /// </summary>
        public virtual Combatant Combatant
        {
            get => combatant;
            set
            {
                combatant = value;

                if (combatant != null)
                {
                    combatant.Container = this;
                }

                onCombatantSet.Invoke(this);
            }
        }

        /// <summary>
        /// 対象に含まれている
        /// </summary>
        bool isTargeted = false;

        /// <summary>
        /// 対象に含まれている
        /// </summary>
        public virtual bool IsTargeted
        {
            get => isTargeted;
            set
            {
                isTargeted = value;
                onTargetFlagSet.Invoke(this);
            }
        }

        public event Action<CombatantContainer> OnDamageEvent = null;

        /// <summary>
        /// 戦闘者が設定されたときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithCombatantContainer onCombatantSet = null;

        /// <summary>
        /// 戦闘者の健康状態が設定されたときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithCombatantContainer onConditionSet = null;

        /// <summary>
        /// 対象フラグが設定されたときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithCombatantContainer onTargetFlagSet = null;

        /// <summary>
        /// 行動したときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithSkill onAction = null;

        /// <summary>
        /// 回避したときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithCombatantContainer onMiss = null;

        /// <summary>
        /// ダメージが発生したときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithCombatantContainer onDamage = null;

        /// <summary>
        /// 回復が発生したときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithCombatantContainer onRecovery = null;

        public void Initialize()
        {
            Combatant = null;
        }

        /// <summary>
        /// 健康状態が設定された
        /// </summary>
        public virtual void OnConditionSet()
        {
            if (onConditionSet)
            {
                onConditionSet.Invoke(this);
            }
        }

        /// <summary>
        /// 行動した
        /// </summary>
        /// <param name="model">行動内容</param>
        public virtual void OnAction(ActionModel model)
        {
            if (onAction)
            {
                onAction.Invoke(model);
            }
        }

        /// <summary>
        /// 行動を回避した
        /// </summary>
        public void OnMiss()
        {
            if (onMiss)
            {
                onMiss.Invoke(this);
            }
        }

        /// <summary>
        /// ダメージ
        /// </summary>
        public virtual void OnDamage()
        {
            OnDamageEvent?.Invoke(this);

            if (onDamage)
            {
                onDamage.Invoke(this);
            }
        }

        /// <summary>
        /// 回復
        /// </summary>
        public void OnRecovery()
        {
            if (onRecovery)
            {
                onRecovery.Invoke(this);
            }
        }

        /// <summary>
        /// 戦闘者を解放する
        /// </summary>
        public void Release()
        {
            Combatant = null;
        }

        /// <summary>
        /// 戦闘開始時の処理
        /// </summary>
        public void OnBattleStart()
        {
            Combatant?.OnBattleStart();
        }

        /// <summary>
        /// 戦闘開始時の処理(控えにいる場合)
        /// </summary>
        public void OnBattleStartReserve()
        {
            Combatant?.OnBattleStartReserve();
        }

        /// <summary>
        /// 戦闘終了時の処理
        /// </summary>
        public void OnBattleEnd()
        {
            Combatant?.OnBattleEnd();
        }

        /// <summary>
        /// 時間が経過した時の処理
        /// </summary>
        /// <param name="elapsedTime">経過時間</param>
        public void OnTimeElapsed(int elapsedTime)
        {
            Combatant?.OnTimeElapsed(elapsedTime);
        }

        /// <summary>
        /// 自身が格納している戦闘者を別のコンテナに入れる
        /// </summary>
        /// <param name="target"></param>
        public void InjectCombatantInto(CombatantContainer target)
        {
            target.Combatant = Combatant;
            Release();
        }

        /// <summary>
        /// 利用可能である
        /// </summary>
        /// <returns></returns>
        public bool IsAvailable()
        {
            return Combatant is null;
        }

        /// <summary>
        /// 戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsCombatant()
        {
            return Combatant is not null;
        }

        /// <summary>
        /// 行動の対象にできる戦闘者を格納している
        /// </summary>
        /// <param name="condition">対象にできる対象の状態</param>
        /// <returns></returns>
        public bool ContainsTargetable(TargetCondition condition)
        {
            return condition switch
            {
                TargetCondition.Living => ContainsFightable(),
                TargetCondition.KnockedOut => ContainsKnockedOut(),
                TargetCondition.LivingAndKnockedOut => ContainsCombatant(),
                _ => false
            };
        }

        /// <summary>
        /// 戦闘可能な戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsFightable()
        {
            return ContainsCombatant() && !Combatant.IsKnockedOut();
        }

        /// <summary>
        /// 戦闘不能な戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsKnockedOut()
        {
            return ContainsCombatant() && Combatant.IsKnockedOut();
        }

        /// <summary>
        /// 行動可能な戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsActionable()
        {
            return ContainsCombatant() && Combatant.CanAct();
        }

        /// <summary>
        /// 浄化可能な戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsPurificatable()
        {
            return ContainsCombatant() && !Combatant.HasBossResistance;
        }

        /// <summary>
        /// 解放可能な戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsReleasable()
        {
            return ContainsCombatant() && Combatant.CanBeReleased();
        }

        /// <summary>
        /// 交代可能な戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsChangeable()
        {
            return ContainsCombatant();
        }

        /// <summary>
        /// プレイヤーが操作可能な戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public virtual bool ContainsPlayerControlled()
        {
            return false;
        }

        /// <summary>
        /// クラッシュ状態の戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsCrush()
        {
            return ContainsCombatant() && Combatant.IsCrushed();
        }
    }
}
