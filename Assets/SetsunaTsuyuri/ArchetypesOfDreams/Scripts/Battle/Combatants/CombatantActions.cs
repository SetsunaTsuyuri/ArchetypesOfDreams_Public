using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Combatant
    {
        /// <summary>
        /// 通常攻撃アニメーションIDに置き換えられる値
        /// </summary>
        static readonly int s_attackAnimationId = -1;

        /// <summary>
        /// 最後に取った行動
        /// </summary>
        ActionInfo _lastAction = null;

        /// <summary>
        /// 行動済みフラグ
        /// </summary>
        public bool HasActed { get; private set; } = false;

        /// <summary>
        /// 戦闘開始時の処理
        /// </summary>
        public void OnBattleStart()
        {
            WaitTime = BasicWaitTime;
            DPRegainingTimer = 0;
        }

        /// <summary>
        /// 戦闘開始時の処理(控えにいる場合)
        /// </summary>
        public void OnBattleStartReserve()
        {
            WaitTime = 0;
            DPRegainingTimer = 0;
        }

        /// <summary>
        /// 戦闘終了時の処理
        /// </summary>
        public void OnBattleEnd()
        {
            _lastAction = null;
            HasActed = false;

            // ステータス効果解除
            StatusEffects.RemoveAll(x => x.Data.IsRemovedOnBattleEnd);

            // ステータス更新
            RefreshStatus();

            // GP回復
            RecoverGP();
        }

        /// <summary>
        /// 勝利時の処理
        /// </summary>
        /// <param name="experience">経験値</param>
        public void OnWin(int experience)
        {
            // 経験値増加
            Experience += experience;

            // HP回復
            int reductionHP = MaxHP - CurrentHP;
            if (reductionHP > 0)
            {
                // 復活
                if (IsKnockedOut)
                {
                    BeRevived();
                }

                int healing = Mathf.CeilToInt(reductionHP * GameSettings.Combatants.HPHealingRateOnWin);
                CurrentHP += healing;
            }
        }

        /// <summary>
        /// 時間経過時の処理
        /// </summary>
        /// <param name="time">経過時間</param>
        /// <returns></returns>
        public void OnTimeElapsed(int elapsedTime)
        {
            WaitTime -= elapsedTime;

            UpdateDPRegaining(elapsedTime);
        }

        /// <summary>
        /// 時間経過によるDP回復の処理
        /// </summary>
        /// <param name="elapsedTime"></param>
        private void UpdateDPRegaining(int elapsedTime)
        {
            DPRegainingTimer += elapsedTime;

            int regaining = DPRegainingTimer / GameSettings.Combatants.DPRegainingInterval;
            if (regaining > 0)
            {
                CurrentDP += regaining;
                DPRegainingTimer %= GameSettings.Combatants.DPRegainingInterval;
            }
        }

        /// <summary>
        /// ターン開始時の処理
        /// </summary>
        public async UniTask OnTurnStart(CancellationToken token)
        {
            await UpdateStatusEffectsRemoval(RemovalTiming.TurnStart, token);

            _lastAction = null;
            HasActed = false;
        }

        /// <summary>
        /// ターン終了時の処理
        /// </summary>
        /// <param name="token"></param>
        public async UniTask OnTurnEnd(CancellationToken token)
        {
            await UpdateStatusChange(token);

            await UpdateStatusEffectsRemoval(RemovalTiming.TurnEnd, token);

            float delay = _lastAction is not null ? _lastAction.Effect.ActionTime : 1.0f;
            Delay(delay);
        }

        /// <summary>
        /// ステータス効果解除の更新処理
        /// </summary>
        /// <param name="timing"></param>
        /// <param name="token"></param>
        private async UniTask UpdateStatusEffectsRemoval(RemovalTiming timing, CancellationToken token)
        {
            bool gp0 = IsAffected(GameSettings.Combatants.EffectGP0.StatusEffectId);

            var effects = StatusEffects.Where(x => x.Data.RemovalTiming == timing);
            foreach (var effect in effects)
            {
                effect.RemainingTurns--;
            }

            var removalEffects = StatusEffects
                .Where(x => x.MustBeRemoved)
                .Select(x => x.Data)
                .ToArray();

            Container.OnStatusEffectsRemoved(removalEffects);

            StatusEffects.RemoveAll(effect => effect.MustBeRemoved);

            // GP0の効果が解除された場合、GPを全回復する
            if (gp0 && !IsAffected(GameSettings.Combatants.EffectGP0.StatusEffectId))
            {
                Results.Healing.AddGP(MaxGP);
                await ApplyHealing(token);
            }

            RefreshStatus();
        }

        /// <summary>
        /// ステータス変化の更新処理
        /// </summary>
        /// <param name="token"></param>
        private async UniTask UpdateStatusChange(CancellationToken token)
        {
            // HP
            if (HPChangeRate != 0)
            {
                int hp = CalculationUtility.Percent(MaxHP, HPChangeRate);
                if (HPChangeRate < 0)
                {
                    Results.Damage.AddHP(Mathf.Abs(hp));
                }
                else
                {
                    Results.Healing.AddHP(Mathf.Abs(hp));
                }
            }

            // DP
            if (DPChangeValue < 0)
            {
                Results.Damage.AddDP(Mathf.Abs(DPChangeValue));
            }
            else if (DPChangeValue > 0)
            {
                Results.Healing.AddDP(Mathf.Abs(DPChangeValue));
            }

            // GP
            if (GPChangeValue < 0)
            {
                Results.Damage.AddGP(Mathf.Abs(GPChangeValue)); ;
            }
            else if (GPChangeValue > 0)
            {
                Results.Healing.AddGP(Mathf.Abs(GPChangeValue));
            }

            await ApplyResults(token);
        }

        /// <summary>
        /// 行動を遅らせる
        /// </summary>
        /// <param name="scale">遅延時間倍率</param>
        private void Delay(float scale)
        {
            int time = Mathf.FloorToInt(BasicWaitTime * scale);
            WaitTime += time;
        }

        /// <summary>
        /// 対象選択時に必要な行動結果を作る
        /// </summary>
        /// <param name="action">行動内容</param>
        /// <param name="targetables">対象にできる戦闘者コンテナリスト</param>
        public void CreateActionResultsOnTargetSelection(ActionInfo action, List<CombatantContainer> targetables)
        {
            foreach (var targetable in targetables)
            {
                // 影響を与える者として自身を設定する
                targetable.Combatant.Results.Affecter = this;

                // 攻撃的なスキルの場合
                if (action.Effect.IsOffensive)
                {
                    // 感情属性の有効性
                    targetable.Combatant.Results.Effectiveness = targetable.Combatant.GetEffectiveness(action.GetEmotion(this));
                }

                // 浄化スキルの場合
                if (action.Effect.IsPurification)
                {
                    // 浄化成功率
                    targetable.Combatant.Results.PurificationSuccessRate = targetable.Combatant.CaluclatePurificationSuccessRate(this);
                }
            }
        }

        /// <summary>
        /// 浄化成功率を計算する
        /// </summary>
        /// <param name="purifier">浄化する者</param>
        /// <returns></returns>
        public abstract int CaluclatePurificationSuccessRate(Combatant purifier);

        /// <summary>
        /// 行動する
        /// </summary>
        /// <param name="action"></param>
        /// <param name="targets"></param>
        /// <param name="token"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public async UniTask Act(ActionInfo action, CombatantContainer[] targets, CancellationToken token, UnityAction onCompleted = null)
        {
            // ターゲットフラグ解除
            foreach (var target in targets)
            {
                target.IsTargeted = false;
            }

            // DPを消費する
            CurrentDP -= action.ConsumptionDP;

            // アイテムを消費する
            if (action.ConsumptionItemdId.HasValue)
            {
                VariableData.Items.Decrease(action.ConsumptionItemdId.Value);
            }

            // 行動時イベント
            Container.OnAction(action);

            // ★暫定処理
            await UniTask.Delay(200, cancellationToken: token);

            // 実行回数分繰り返す
            for (int i = 0; i < action.Effect.ExtraExecutions; i++)
            {
                // 対象の戦闘者を取り出す
                Combatant[] targetCombatants = GetTargetCombatants(
                    targets,
                    action.Effect.TargetCondition,
                    action.Effect.TargetSelection);

                // 対象が存在する
                if (targetCombatants.Any())
                {
                    // 行動の結果を作る
                    CreateActionResults(action, targetCombatants);

                    // 結果を反映させる
                    foreach (var targetCombatant in targetCombatants)
                    {
                        if (action.Effect.AnimationId != 0)
                        {
                            await PlayEffectAnimation(action, targetCombatant, token);
                        }

                        await targetCombatant.ApplyResults(token);
                    }
                }
            }

            // 戦闘中の場合
            if (Battle.IsRunning)
            {
                // 最後の行動
                _lastAction = action;

                // 行動済みフラグ
                if (!action.Effect.CanActAgain)
                {
                    HasActed = true;
                }
            }

            await TimeUtility.Wait(GameSettings.WaitTime.ActionExecuted, token);

            // 完了時のコールバック呼び出し
            onCompleted?.Invoke();
        }

        /// <summary>
        /// エフェクトアニメーションを再生する
        /// </summary>
        /// <param name="action"></param>
        /// <param name="target"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask PlayEffectAnimation(ActionInfo action, Combatant target, CancellationToken token)
        {
            int animationId = action.Effect.AnimationId;
            if (animationId == s_attackAnimationId)
            {
                animationId = Data.AttackAnimationId;
            }

            EffectAnimationData animation = MasterData.GetEffectAnimationData(animationId);
            if (animation is null)
            {
                return;
            }

            Queue<EffectAnimationElementData> queue = new(animation.Elements.OrderBy(x => x.Timing));
            EffectAnimationElementData element = null;
            if (queue.TryDequeue(out EffectAnimationElementData firstElement))
            {
                element = firstElement;
            }

            float timer = 0.0f;
            while (timer <= animation.Duration)
            {
                if (element is not null
                    && timer >= element.Timing)
                {
                    // エフェクト再生位置
                    Vector3 position = element.EffectPosition switch
                    {
                        EffectAnimationPositionType.Targets => target.Container.transform.position,
                        EffectAnimationPositionType.User => Container.transform.position,
                        _ => Vector3.zero
                    };

                    // エフェクト再生
                    if (element.EffectId > 0)
                    {
                        EffectsManager.Play(element.EffectId, position);
                    }

                    // SE再生
                    if (element.SEId > 0)
                    {
                        AudioManager.PlaySE(element.SEId, element.SEVolume);
                    }

                    // 次の要素を取り出す
                    if (queue.TryDequeue(out EffectAnimationElementData nextElement))
                    {
                        element = nextElement;
                    }
                    else
                    {
                        element = null;
                    }
                }

                timer += Time.deltaTime;
                await UniTask.Yield(token);
            }
        }

        /// <summary>
        /// 行動結果を反映する
        /// </summary>
        /// <param name="token">トークン</param>
        private async UniTask ApplyResults(CancellationToken token)
        {
            if (!Container)
            {
                Debug.LogError("Container is null");
                Results.Initialize();
                return;
            }

            // ミス
            if (Results.Miss)
            {
                Container.OnMiss();
            }

            // ダメージ
            if (Results.Damage.IsValid)
            {
                await ApplyDamage(token);
            }

            // 回復
            if (Results.Healing.IsValid)
            {
                await ApplyHealing(token);
            }

            // ステータス効果付与
            if (Results.AddedStatusEffects.Any())
            {
                await ApplyAddedStatusEffects(Results.AddedStatusEffects, token);
            }

            // ステータス効果解除
            if (Results.RemovedStatusEffects.Any())
            {
                foreach (var status in Results.RemovedStatusEffects)
                {
                    StatusEffects.Remove(status);
                }

                // 解除されたステータス効果
                var removedEffects = Results.RemovedStatusEffects
                    .Select(x => x.Data)
                    .ToArray();

                Container.OnStatusEffectsRemoved(removedEffects);

                RefreshStatus();
            }

            // 浄化失敗
            if (Results.Purified == false)
            {
                Container.OnMiss();
            }

            // 浄化成功
            if (Results.Purified == true)
            {
                await BePurified(token);
            }
            // 戦闘不能
            else if (!IsKnockedOut && CurrentHP == 0)
            {
                BeKnockedOut();
            }
            // 復活
            else if (IsKnockedOut && CurrentHP > 0)
            {
                BeRevived();
            }

            // 交代
            if (Results.IsChangeTarget)
            {
                Swap(Results.Affecter.Container);
            }

            // 行動結果を初期化する
            Results.Initialize();
        }

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="token"></param>
        private async UniTask ApplyDamage(CancellationToken token)
        {
            bool crush = IsAffected(StatusEffectId.Crush);

            DamageResult damage = Results.Damage;

            int damageHP = damage.HP.GetValueOrDefault();
            int damageDP = damage.DP.GetValueOrDefault();
            int damageGP = damage.GP.GetValueOrDefault();

            // 致死ダメージ耐久判定
            if (damage.HP >= CurrentHP
                && JudgeSurvivour())
            {
                damageHP = CurrentHP - 1;
            }

            CurrentHP -= damageHP;
            CurrentDP -= damageDP;
            CurrentGP -= damageGP;

            if (!crush)
            {
                damage.IsCrush = IsAffected(StatusEffectId.Crush);
            }

            if (damage.IsOneOrOver)
            {
                AudioManager.PlaySE(SEId.Damage);
            }

            Container.OnDamage();
            damage.Initialize();
            await TimeUtility.Wait(GameSettings.WaitTime.Damage, token);
        }

        /// <summary>
        /// 回復する
        /// </summary>
        /// <param name="token"></param>
        private async UniTask ApplyHealing(CancellationToken token)
        {
            HealingResult healing = Results.Healing;
            CurrentHP += healing.HP.GetValueOrDefault();
            CurrentDP += healing.DP.GetValueOrDefault();
            CurrentGP += healing.GP.GetValueOrDefault();

            if (healing.IsOneOrOver)
            {
                AudioManager.PlaySE(SEId.Healing);
            }

            Container.OnHealing();
            healing.Initialize();
            await TimeUtility.Wait(GameSettings.WaitTime.Healing, token);
        }

        /// <summary>
        /// ステータス効果を追加する
        /// </summary>
        /// <param name="effects"></param>
        /// <param name="token"></param>
        private async UniTask ApplyAddedStatusEffects(List<EffectData.StatusEffect> effects, CancellationToken token)
        {
            foreach (var effect in effects)
            {
                bool success = AddStatusEffect(effect);
                if (success)
                {
                    Container.OnStatusEffectAdded(effect);
                    await TimeUtility.Wait(GameSettings.WaitTime.StatusEffectAdded, token);
                }
            }
        }

        /// <summary>
        /// 浄化される
        /// </summary>
        /// <param name="token"></param>
        private async UniTask BePurified(CancellationToken token)
        {
            if (Container is EnemyContainer enemyContainer)
            {
                await enemyContainer.OnPurified(token);
            }
        }

        /// <summary>
        /// 戦闘不能になる
        /// </summary>
        private void BeKnockedOut()
        {
            Condition = GameAttribute.Condition.KnockedOut;

            if (Container && Container is EnemyContainer enemyContainer)
            {
                enemyContainer.OnKnockedOut();
            }
        }

        /// <summary>
        /// 復活する
        /// </summary>
        private void BeRevived()
        {
            Condition = GameAttribute.Condition.Normal;
            WaitTime = BasicWaitTime;
            RecoverGP();
        }

        /// <summary>
        /// 入れ替わる
        /// </summary>
        /// <param name="target">対象のコンテナ</param>
        public void Swap(CombatantContainer target)
        {
            Container.Swap(target);
        }

        /// <summary>
        /// 対象のコンテナから戦闘者を取り出す
        /// </summary>
        /// <param name="targetContainers">対象コンテナ配列</param>
        /// <param name="selection">対象の選択方法</param>
        /// <returns></returns>
        private Combatant[] GetTargetCombatants(CombatantContainer[] targetContainers, TargetCondition condition, TargetSelectionType selection)
        {
            // 対象にできる戦闘者配列
            Combatant[] targets = targetContainers
                .Where(x => x.ContainsTargetable(condition))
                .Select(x => x.Combatant)
                .ToArray();

            // ランダムに対象を選ぶ場合、配列内から1体のみをランダムに選ぶ
            if (selection == TargetSelectionType.Random && targets.Length > 1)
            {
                int index = Random.Range(0, targets.Length);
                targets = new Combatant[] { targets[index] };
            }
            return targets;
        }

        /// <summary>
        /// 行動結果を作る(行動実行時)
        /// </summary>
        /// <param name="action">行動内容</param>
        /// <param name="targets">対象</param>
        private void CreateActionResults(ActionInfo action, Combatant[] targets)
        {
            foreach (var target in targets)
            {
                CreateActionResult(action, target);
            }
        }

        /// <summary>
        /// 行動結果を作る
        /// </summary>
        /// <param name="action"></param>
        /// <param name="target"></param>
        private void CreateActionResult(ActionInfo action, Combatant target)
        {
            EffectData effect = action.Effect;

            // 影響を与える者として自身を設定する
            target.Results.Affecter = this;

            // 命中判定
            target.Results.Miss = !JudgeHit(target, effect);
            if (target.Results.Miss)
            {
                return;
            }

            // ダメージ効果
            if (effect.HasDamageEffects)
            {
                // 攻撃的な効果の場合
                if (effect.IsOffensive)
                {
                    // クリティカルの有無
                    target.Results.Damage.IsCritical = JudgeCritical(target, effect);

                    // 感情属性の有効性
                    target.Results.Effectiveness = target.GetEffectiveness(action.GetEmotion(this));
                }

                // ダメージ・回復を決定する
                foreach (var status in effect.DamageEffects)
                {
                    CreateDamageOrHealingResult(action, target, status);
                }

                // 追加GPダメージを決定する
                if (!target.IsCrushed && target.Results.Damage.HP > 0)
                {
                    int damage = CalculateExtraGPDamage(target, effect);
                    if (damage > 0)
                    {
                        target.Results.Damage.AddGP(damage);
                    }
                }
            }

            // ステータス効果
            foreach (var data in effect.StatusEffects)
            {
                if (CanBeAffected(data.StatusEffectId)
                    && !data.IsRemoval
                    && RandomUtility.Percent(data.Probability))
                {
                    // ステータス効果を追加する
                    target.Results.AddedStatusEffects.Add(data);
                }
                else if (data.IsRemoval)
                {
                    // ID指定
                    var removalEffectsById = target.StatusEffects
                        .Where(x => x.Data.Id == data.StatusEffectId);

                    // カテゴリ指定
                    var removalEffectsByCategory = target.StatusEffects
                        .Where(x => x.Data.Category == data.RemovalCategory);

                    // ステータス効果を解除する
                    var removalEffects = removalEffectsById.Concat(removalEffectsByCategory);
                    target.Results.RemovedStatusEffects.AddRange(removalEffects);
                }
            }

            // 浄化
            if (effect.IsPurification)
            {
                // 浄化成功率
                target.Results.PurificationSuccessRate = target.CaluclatePurificationSuccessRate(this);

                // 判定
                if (RandomUtility.Percent(target.Results.PurificationSuccessRate))
                {
                    // 成功
                    target.Results.Purified = true;
                }
                else
                {
                    // 失敗
                    target.Results.Purified = false;
                }
            }

            // 交代
            if (effect.IsChange)
            {
                target.Results.IsChangeTarget = true;
            }
        }

        /// <summary>
        /// 命中判定を行う
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="effect">効果</param>
        /// <returns></returns>
        private bool JudgeHit(Combatant target, EffectData effect)
        {
            if (!effect.IsOffensive)
            {
                return true;
            }

            int probability = Accuracy + effect.Hit - target.Evasion;
            bool result = RandomUtility.Percent(probability);
            return result;
        }

        /// <summary>
        /// クリティカル判定を行う
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="effect">効果</param>
        /// <returns></returns>
        private bool JudgeCritical(Combatant target, EffectData effect)
        {
            if (!effect.IsOffensive)
            {
                return false;
            }

            int probability = CriticalHit + effect.Critical - target.CriticalEvasion;
            bool result = RandomUtility.Percent(probability);
            return result;
        }

        /// <summary>
        /// 属性に対する有効性を取得する
        /// </summary>
        /// <param name="emotion">属性</param>
        /// <returns></returns>
        public GameAttribute.Effectiveness GetEffectiveness(GameAttribute.Emotion emotion)
        {
            return GameSettings.GetEffectiveness(Emotion, emotion);
        }

        /// <summary>
        /// ダメージまたは回復の結果を作る
        /// </summary>
        /// <param name="action">行動内容</param>
        /// <param name="target">対象</param>
        /// <param name="damageEffect">ダメージ効果</param>
        private void CreateDamageOrHealingResult(ActionInfo action, Combatant target, EffectData.DamageEffect damageEffect)
        {
            // 基本値
            float value = CalculateBasicValue(damageEffect, target);

            // 攻撃的な効果の場合
            if (action.Effect.IsOffensive)
            {
                // 感情属性補正
                value *= GameSettings.Effectiveness.Rates.GetValueOrDefault(target.GetEffectiveness(action.GetEmotion(this)));

                // クリティカル補正
                if (target.Results.Damage.IsCritical)
                {
                    value *= GameSettings.Combatants.CriticalDamageScale;
                }

                // 攻撃倍率
                foreach (var statusEffect in StatusEffects)
                {
                    value *= 1.0f + statusEffect.Data.PowerAttackRate * damageEffect.Power;
                    value *= 1.0f + statusEffect.Data.TechniqueAttackRate * damageEffect.Technique;
                }

                // 防御倍率
                foreach (var statusEffect in target.StatusEffects)
                {
                    value *= 1.0f - statusEffect.Data.PowerDefenseRate * damageEffect.Power;
                    value *= 1.0f - statusEffect.Data.TechniqueDefenseRate * damageEffect.Technique;
                }
            }

            // 固定値
            value += damageEffect.Fixed;

            // 固定乱数値
            value += Random.Range(0, damageEffect.Random);

            // 影響するステータスに応じて結果に値を加算する
            int valueInt = Mathf.FloorToInt(value);
            AddDamageOrHealingResult(target, valueInt, damageEffect);
        }

        /// <summary>
        /// 対象にダメージまたは回復の結果を追加する
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="damageEffect"></param>
        private void AddDamageOrHealingResult(Combatant target, int value, EffectData.DamageEffect damageEffect)
        {
            switch (damageEffect.AffectedStatusType)
            {
                // HP
                case AffectedStatusType.HP:

                    // ダメージ
                    if (damageEffect.DamageType == DamageType.Damage)
                    {
                        // ゼロにしない場合
                        if (damageEffect.DontKill && value >= target.CurrentHP)
                        {
                            value = target.CurrentHP - 1;
                        }

                        target.Results.Damage.AddHP(value);
                    }
                    else // 回復
                    {
                        target.Results.Healing.AddHP(value);
                    }
                    break;

                // DP
                case AffectedStatusType.DP:

                    // ダメージ
                    if (damageEffect.DamageType == DamageType.Damage)
                    {
                        // ゼロにしない場合
                        if (damageEffect.DontKill && value >= target.CurrentDP)
                        {
                            value = target.CurrentDP - 1;
                        }

                        target.Results.Damage.AddDP(value);
                    }
                    else //回復
                    {
                        target.Results.Healing.AddDP(value);
                    }
                    break;

                // GP
                case AffectedStatusType.GP:

                    // ダメージ
                    if (damageEffect.DamageType == DamageType.Damage)
                    {
                        // ゼロにしない場合
                        if (damageEffect.DontKill && value >= target.CurrentGP)
                        {
                            value = target.CurrentGP - 1;
                        }

                        target.Results.Damage.AddGP(value);
                    }
                    else // 回復
                    {
                        target.Results.Healing.AddGP(value);
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ダメージor回復量の基本値を決定する
        /// </summary>
        /// <param name="damage">ダメージ効果</param>
        /// <param name="target">対象</param>
        /// <returns></returns>
        private float CalculateBasicValue(EffectData.DamageEffect damage, Combatant target)
        {
            float result = 0.0f;

            result += Power * damage.Power;
            result += Technique * damage.Technique;
            result += target.GetCurrentStatus(damage.AffectedStatusType) * damage.Current;
            result += target.GetMaxStatus(damage.AffectedStatusType) * damage.Max;

            return result;
        }

        /// <summary>
        /// 追加GPダメージを決定する
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="effect">効果</param>
        /// <returns></returns>
        private int CalculateExtraGPDamage(Combatant target, EffectData effect)
        {
            int damage = 0;

            // 弱点属性
            switch (target.Results.Effectiveness)
            {
                case GameAttribute.Effectiveness.Weakness:
                    damage++;
                    break;

                case GameAttribute.Effectiveness.SuperWeakness:
                    damage += target.CurrentGP;
                    break;
            }

            // クリティカル
            if (target.Results.Damage.IsCritical)
            {
                damage += GameSettings.Combatants.CriticalGPDamage;
            }

            return damage;
        }
    }
}
