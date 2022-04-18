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
    public class CombatantContainer : MonoBehaviour, IInitializable
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
        /// スキルを使用したときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithSkill onSkillUsed = null;

        /// <summary>
        /// 攻撃を回避したときのゲームイベント
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
        /// スキルを使用した
        /// </summary>
        /// <param name="skill">スキル</param>
        public void OnSkillUsed(Skill skill)
        {
            if (onSkillUsed)
            {
                onSkillUsed.Invoke(skill);
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
        public void OnDamage()
        {
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
            return Combatant == null;
        }

        /// <summary>
        /// 戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsCombatant()
        {
            return Combatant != null;
        }

        /// <summary>
        /// 行動の対象にできる戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsTargetable()
        {
            return ContainsFightable();
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
    }
}
