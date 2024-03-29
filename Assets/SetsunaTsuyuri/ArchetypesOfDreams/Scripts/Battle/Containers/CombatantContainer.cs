﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者を格納するコンテナ
    /// </summary>
    public abstract class CombatantContainer : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// 味方
        /// </summary>
        CombatantsPartyBase _allies = null;

        /// <summary>
        /// 味方
        /// </summary>
        public CombatantsPartyBase Allies
        {
            // TODO: 状態異常等により敵味方の判断が逆転する
            get => _allies;
        }

        /// <summary>
        /// 敵
        /// </summary>
        CombatantsPartyBase _enemies = null;

        /// <summary>
        /// 敵
        /// </summary>
        public CombatantsPartyBase Enemies
        {
            // TODO: 状態異常等により敵味方の判断が逆転する
            get => _enemies;
        }

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// 戦闘者
        /// </summary>
        Combatant _combatant = null;

        /// <summary>
        /// 戦闘者
        /// </summary>
        public virtual Combatant Combatant
        {
            get => _combatant;
            set
            {
                if (_combatant is not null)
                {
                    _combatant.ReleaseSprites();
                }

                _combatant = value;

                if (_combatant is not null)
                {
                    _combatant.Container = this;
                    _combatant.RefreshStatus();
                }

                MessageBrokersManager.CombatantSet.Publish(this);
            }
        }

        /// <summary>
        /// 対象に含まれている
        /// </summary>
        bool _isTargeted = false;

        /// <summary>
        /// 対象に含まれている
        /// </summary>
        public virtual bool IsTargeted
        {
            get => _isTargeted;
            set
            {
                _isTargeted = value;
                MessageBrokersManager.TargetFlagSet.Publish(this);
            }
        }

        /// <summary>
        /// ダメージを受けたときのイベント
        /// </summary>
        public event UnityAction<CombatantContainer> Damaged = null;

        private void OnDestroy()
        {
            Combatant?.ReleaseSprites();
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="allies">味方</param>
        /// <param name="enemies">敵</param>
        public void SetUp(CombatantsPartyBase allies, CombatantsPartyBase enemies)
        {
            _allies = allies;
            _enemies = enemies;
        }

        public virtual void Initialize()
        {
            if (Combatant is not null)
            {
                Combatant.ReleaseSprites();
                Combatant.Container = null;
            }

            Combatant = null;
        }

        /// <summary>
        /// 失敗
        /// </summary>
        public void OnMiss()
        {
            MessageBrokersManager.Miss.Publish(this);
        }

        /// <summary>
        /// ダメージ
        /// </summary>
        public virtual void OnDamage()
        {
            MessageBrokersManager.Damage.Publish(this);
            Damaged?.Invoke(this);
        }

        /// <summary>
        /// 回復
        /// </summary>
        public void OnHealing()
        {
            MessageBrokersManager.Healing.Publish(this);
        }

        /// <summary>
        /// 戦闘不能
        /// </summary>
        public virtual void OnKnockedOut()
        {
            MessageBrokersManager.KnockedOut.Publish(this);
        }

        /// <summary>
        /// ステータス効果付与
        /// </summary>
        /// <param name="effect"></param>
        public void OnStatusEffectAdded(EffectData.StatusEffect effect)
        {
            StatusEffectData data = MasterData.GetStatusEffectData(effect.StatusEffectId);
            AddedStatusEffectResult result = new(this, data);
            MessageBrokersManager.StatusEffectAdded.Publish(result);
        }

        /// <summary>
        /// ステータス効果解除
        /// </summary>
        /// <param name="effects"></param>
        public void OnStatusEffectsRemoved(StatusEffectData[] effects)
        {
            StatusEffectsResult result = new(this, effects);
            MessageBrokersManager.StatusEffectsRemoved.Publish(result);
        }

        /// <summary>
        /// 行動実行
        /// </summary>
        /// <param name="action"></param>
        public virtual void OnActionExecution(ActionInfo action)
        {
            MessageBrokersManager.ActionExecution.Publish(action);
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
        /// ターン開始時の処理
        /// </summary>
        /// <param name="token"></param>
        public async UniTask OnTurnStart(CancellationToken token)
        {
            if (!ContainsCombatant)
            {
                return;
            }

            await Combatant.OnTurnStart(token);
        }

        /// <summary>
        /// ターン終了時の処理
        /// </summary>
        /// <param name="token"></param>
        public async UniTask OnTurnEnd(CancellationToken token)
        {
            if (!ContainsCombatant)
            {
                return;
            }

            await Combatant.OnTurnEnd(token);
        }

        /// <summary>
        /// 行動する
        /// </summary>
        /// <param name="action"></param>
        /// <param name="targets"></param>
        /// <param name="onCompleted"></param>
        public void Act(ActionInfo action, CombatantContainer[] targets, UnityAction onCompleted)
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            Combatant.Act(action, targets, token, onCompleted).Forget();
        }

        /// <summary>
        /// 行動する
        /// </summary>
        /// <param name="action"></param>
        /// <param name="targets"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask Act(ActionInfo action, CombatantContainer[] targets, CancellationToken token)
        {
            await Combatant.Act(action, targets, token);
        }

        /// <summary>
        /// 逃げる
        /// </summary>
        public virtual void Escape()
        {
            AudioManager.PlaySE(SEId.Escape);
        }

        /// <summary>
        /// 浄化された戦闘者を別のコンテナに入れる
        /// </summary>
        /// <param name="target"></param>
        public void InjectPurified(CombatantContainer target)
        {
            target.Combatant = Combatant;
            target.Combatant.HasActed = false;
            target.Combatant.InitializeStatus();

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
        public bool ContainsCombatant => Combatant is not null;

        /// <summary>
        /// 行動の対象にできる戦闘者を格納している
        /// </summary>
        /// <param name="condition">対象にできる対象の状態</param>
        /// <returns></returns>
        public bool ContainsTargetable(TargetCondition condition)
        {
            return condition switch
            {
                TargetCondition.Living => ContainsFightable,
                TargetCondition.KnockedOut => ContainsKnockedOut,
                TargetCondition.LivingOrKnockedOut => ContainsCombatant,
                _ => false
            };
        }

        /// <summary>
        /// 戦闘可能な戦闘者を格納している
        /// </summary>
        public bool ContainsFightable
        {
            get => ContainsCombatant && !Combatant.IsKnockedOut;
        }

        /// <summary>
        /// 戦闘不能な戦闘者を格納している
        /// </summary>
        public bool ContainsKnockedOut
        {
            get => ContainsCombatant && Combatant.IsKnockedOut;
        }

        /// <summary>
        /// 行動可能な戦闘者を格納している
        /// </summary>
        public bool ContainsActionable
        {
            get => ContainsCombatant && Combatant.CanAct();
        }

        /// <summary>
        /// 浄化可能な戦闘者を格納している
        /// </summary>
        public bool ContainsPurificatable
        {
            get => ContainsCombatant && !Combatant.HasBossResistance;
        }

        /// <summary>
        /// 解放可能な戦闘者を格納している
        /// </summary>
        public bool ContainsReleasable
        {
            get => ContainsCombatant && Combatant.CanBeReleased();
        }

        /// <summary>
        /// 交代可能な戦闘者を格納している
        /// </summary>
        public bool ContainsChangeable
        {
            get => ContainsCombatant && Combatant is Nightmare;
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
        /// ブレイク状態の戦闘者を格納している
        /// </summary>
        /// <returns></returns>
        public bool ContainsBroken()
        {
            return ContainsCombatant && Combatant.IsBroken;
        }

        /// <summary>
        /// 何らかのスキルを持っている
        /// </summary>
        /// <returns></returns>
        public bool HasAnySkill => Combatant.HasAnySkill;

        /// <summary>
        /// 何らかのスキルを使用できる
        /// </summary>
        /// <returns></returns>
        public bool CanUseAnySkill()
        {
            var ids = Combatant.GetAcquisitionSkillIds();
            bool result = ids.Any(x => CanUseSkill(x));
            return result;
        }

        /// <summary>
        /// 何らかのアイテムを使用できる
        /// </summary>
        /// <returns></returns>
        public bool CanUseAnyItem()
        {
            var ids = VariableData.Items.OwnedItemIds;
            bool result = ids.Any(x => CanUseItem(x));
            return result;
        }

        /// <summary>
        /// スキルを使用できる
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CanUseSkill(int id)
        {
            SkillData skill = MasterData.GetSkillData(id);
            bool isSealed = Combatant.SkillIsSealed(id);
            bool canConsumeDP = Combatant.CurrentDP >= skill.Cost;
            return !isSealed && canConsumeDP && CanUse(skill);
        }

        /// <summary>
        /// アイテムを使用できる
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CanUseItem(int id)
        {
            ItemData item = MasterData.GetItemData(id);
            return CanUse(item);
        }

        /// <summary>
        /// 使用できる
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool CanUse(ActionInfo action)
        {
            bool canConsumeDP = Combatant.CurrentDP >= action.ConsumptionDP;
            return canConsumeDP && CanUse(action.Effect);
        }

        /// <summary>
        /// 効果を使用できる
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        private bool CanUse(EffectData effect)
        {
            bool result = effect.TargetPosition switch
            {
                TargetPosition.Enemies => ExistsTargetables(effect, Enemies),
                TargetPosition.Allies => ExistsTargetables(effect, Allies),
                TargetPosition.Both => ExistsTargetables(effect, Allies, Enemies),
                TargetPosition.Oneself => true,
                TargetPosition.AlliesOtherThanOneself => ExistsTargetables(effect, Allies),
                TargetPosition.Reserves => ExistsTargetables(effect, Allies),
                _ => false
            }; ;

            return result;
        }

        /// <summary>
        /// 対象にできるコンテナが存在する
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="party"></param>
        /// <returns></returns>
        private bool ExistsTargetables(EffectData effect, CombatantsPartyBase party)
        {
            bool result = false;

            if (party)
            {
                result = party.ContainsTargetables(this, effect);
            }

            return result;
        }

        /// <summary>
        /// 対象にできるコンテナが存在する
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="allies"></param>
        /// <param name="enemies"></param>
        /// <returns></returns>
        private bool ExistsTargetables(EffectData effect, CombatantsPartyBase allies, CombatantsPartyBase enemies)
        {
            return ExistsTargetables(effect, allies) || ExistsTargetables(effect, enemies);
        }

        /// <summary>
        /// 対象にできるコンテナを取得する
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public List<CombatantContainer> GetTargetables(EffectData effect)
        {
            List<CombatantContainer> targetables = new();
            switch (effect.TargetPosition)
            {
                case TargetPosition.Enemies:
                    AddTargetables(targetables, effect, Enemies);
                    break;

                case TargetPosition.Allies:
                case TargetPosition.AlliesOtherThanOneself:
                    AddTargetables(targetables, effect, Allies);
                    break;

                case TargetPosition.Both:
                    AddTargetables(targetables, effect, Allies);
                    AddTargetables(targetables, effect, Enemies);
                    break;

                case TargetPosition.Oneself:
                    targetables.Add(this);
                    break;

                case TargetPosition.Reserves:
                    AddTargetables(targetables, effect, Allies);
                    break;

                default:
                    break;
            }

            return targetables;
        }

        /// <summary>
        /// 対象にできるコンテナを追加する
        /// </summary>
        /// <param name="targetables"></param>
        /// <param name="effect"></param>
        /// <param name="party"></param>
        private void AddTargetables(List<CombatantContainer> targetables, EffectData effect, CombatantsPartyBase party)
        {
            if (!party)
            {
                return;
            }

            var added = party.GetTargetables(this, effect);
            targetables.AddRange(added);
        }

        /// <summary>
        /// 中身を入れ替える
        /// </summary>
        /// <param name="target"></param>
        public void Swap(CombatantContainer target)
        {
            (target.Combatant, Combatant) = (Combatant, target.Combatant);
        }
    }
}
