using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者の行動
    /// </summary>
    public partial class Combatant
    {
        /// <summary>
        /// 待機時間
        /// </summary>
        public int WaitTime { get; set; } = 0;

        /// <summary>
        /// 戦闘開始時の処理
        /// </summary>
        public void OnBattleStart()
        {
            // 基本待機時間を設定する
            WaitTime = GetBasicWaitTime();
        }

        /// <summary>
        /// 戦闘開始時の処理
        /// </summary>
        public void OnBattleStartReserve()
        {
            // 待機時間をゼロにする
            WaitTime = 0;
        }

        /// <summary>
        /// 時間が経過したときの処理
        /// </summary>
        /// <param name="time">経過時間</param>
        /// <returns></returns>
        public void OnTimeElapsed(int elapsedTime)
        {
            // 経過時間分、待機時間を減らす
            WaitTime -= elapsedTime;
        }

        /// <summary>
        /// 行動開始時の処理
        /// </summary>
        public void OnActionStart()
        {
            // クラッシュ状態の更新
            UpdateCrush();
        }

        /// <summary>
        /// 行動終了後の処理
        /// </summary>
        /// <param name="battle">戦闘の内容</param>
        public void OnActionEnd(BattleManager battle)
        {
            // ステータス変化の更新
            UpdateStatusChange(battle);

            // ステータス効果の更新
            UpdateStatusEffects();

            // 夢想力増加
            CurrentDP += GameSettings.Combatants.AmoutOfIncreaseInDreamPerTurn;

            // 行動内容
            ActionModel action = battle.ActorAction;

            // 行動していた場合
            if (action is not null)
            {
                // 行動負荷に応じた待機時間を設定する
                SetWaitTime(action);
            }
            else
            {
                // 基本待機時間を設定する
                WaitTime = GetBasicWaitTime();
            }
        }

        /// <summary>
        /// 戦闘終了時の処理
        /// </summary>
        public void OnBattleEnd()
        {
            // ステータス効果解除
            StatusEffects.RemoveAll(x => x.Data.IsRemovedOnBattleEnd);

            // ステータス更新
            RefreshStatus();

            // 精神力初期化
            InitializeSoul();
        }

        /// <summary>
        /// クラッシュ状態の更新処理
        /// </summary>
        private void UpdateCrush()
        {
            if (!IsCrushed())
            {
                return;
            }

            // 有効ターン数を減らす
            RemainingCrushTurns--;
            if (RemainingCrushTurns <= 0)
            {
                // クラッシュ状態から復帰する
                Condition = Attribute.Condition.Normal;
                ChangeSoul();
                RecoverSp();
            }
        }

        /// <summary>
        /// ステータス効果の更新処理
        /// </summary>
        private void UpdateStatusEffects()
        {
            // 有効ターン数を減らす
            foreach (var statusEffect in StatusEffects)
            {
                statusEffect.RemainingTurns--;
            }

            // 残りターン数0のステータス効果を解除する
            StatusEffects.RemoveAll(x => x.RemainingTurns == 0);
        }

        /// <summary>
        /// ステータス変化の更新処理
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        private void UpdateStatusChange(BattleManager battle)
        {
            int hpChange = MathUtility.Percent(MaxHP, HPChangeRate);
            if (HPChangeRate < 0)
            {
                Result.AddHpDamage(Mathf.Abs(hpChange));
            }
            else if (HPChangeRate > 0)
            {
                Result.AddHpRecovery(Mathf.Abs(hpChange));
            }

            if (DPChangeValue < 0)
            {
                Result.AddDpDamage(Mathf.Abs(DPChangeValue));
            }
            else if (DPChangeValue > 0)
            {
                Result.AddDpRecovery(Mathf.Abs(DPChangeValue));
            }

            if (SPChangeValue < 0)
            {
                Result.AddSpDamage(Mathf.Abs(SPChangeValue));
            }
            else if (SPChangeValue > 0)
            {
                Result.AddHpRecovery(Mathf.Abs(SPChangeValue));
            }

            ApplyActionResult(battle, battle.GetCancellationTokenOnDestroy()).Forget();
        }

        /// <summary>
        /// 待機時間の基本値を取得する
        /// </summary>
        /// <returns></returns>
        public int GetBasicWaitTime()
        {
            return GameSettings.Combatants.MaxWaitTime / Agility;
        }

        /// <summary>
        /// 待機時間を設定する
        /// </summary>
        /// <param name="actionTime">行動内容</param>
        private void SetWaitTime(ActionModel action)
        {
            int basicWaitTime = GetBasicWaitTime();
            float actionTime = action.Effect.ActionTime;
            WaitTime = Mathf.FloorToInt(basicWaitTime * actionTime);
        }

        /// <summary>
        /// ステータス効果を追加する
        /// </summary>
        /// <param name="data">ステータス効果付与データ</param>
        private void AddStatusEffect(EffectData.StatusEffect data)
        {
            // 既に同じIDの効果が存在する場合
            StatusEffect sameIdStatusEffect = StatusEffects.FirstOrDefault(x => x.Data.Id == data.Id);
            if (sameIdStatusEffect is not null)
            {
                switch (sameIdStatusEffect.Data.Stack)
                {
                    // 延長
                    case Attribute.Stack.Prolong:
                        sameIdStatusEffect.RemainingTurns += data.Turns;
                        break;

                    // 上書き
                    case Attribute.Stack.OverWritten:
                        sameIdStatusEffect.RemainingTurns = data.Turns;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                // ステータス効果
                StatusEffect statusEffect = new(data.Id, data.Turns);

                // リストに追加する
                StatusEffects.Add(statusEffect);

                // ステータス更新
                RefreshStatus();
            }
        }

        /// <summary>
        /// 現在使用可能なスキルを全て取得する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <returns></returns>
        private ActionModel[] GetAvailableSkills(BattleManager battle)
        {
            ActionModel[] result = Skills
                .Where(x => x.CanBeExecuted(battle))
                .ToArray();

            return result;
        }

        /// <summary>
        /// 対象選択時に必要な行動結果を作る
        /// </summary>
        /// <param name="action">行動内容</param>
        /// <param name="targetables">対象にできる戦闘者コンテナ</param>
        public void MakeActionResultOnTargetSelection(ActionModel action, CombatantContainer[] targetables)
        {
            foreach (var targetable in targetables)
            {
                // 影響を与える者として自身を設定する
                targetable.Combatant.Result.Affecter = this;

                // 攻撃的なスキルの場合
                if (action.Effect.IsOffensive)
                {
                    // 感情属性の有効性
                    targetable.Combatant.Result.Effectiveness = targetable.Combatant.GetEffectiveness(action.GetEmotion(this));
                }

                // 浄化スキルの場合
                if (action.Effect.IsPurification)
                {
                    // 浄化成功率
                    targetable.Combatant.Result.PurificationSuccessRate = targetable.Combatant.GetPurificationSuccessRate(this);
                }
            }
        }

        /// <summary>
        /// 浄化成功率を取得する
        /// </summary>
        /// <param name="purifier">浄化する者</param>
        /// <returns></returns>
        public abstract int GetPurificationSuccessRate(Combatant purifier);

        /// <summary>
        /// 行動する
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <param name="token"></param>
        public async UniTask Act(BattleManager battle, CancellationToken token)
        {
            // コンテナがなければ中止する
            if (!Container)
            {
                Debug.LogError("Container is null");
                return;
            }

            // 対象
            CombatantContainer[] targets = battle.Targets;

            // 行動内容
            ActionModel action = battle.ActorAction;

            // DPを消費する
            CurrentDP -= action.ConsumptionDp;

            // アイテムを消費する
            if (action.ConsumptionItemdId.HasValue)
            {
                ItemUtility.ConsumeItem(action.ConsumptionItemdId.Value);
            }

            // ★使用者が誰か分かる演出

            // 行動時イベント
            Container.OnAction(action);

            // ★暫定処理
            await UniTask.Delay(200);

            // 実行回数分繰り返す
            for (int i = 0; i < action.Effect.Executions; i++)
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
                    MakeActionResults(action, targetCombatants);

                    // 結果を反映させる
                    foreach (var targetCombatant in targetCombatants)
                    {
                        if (action.Effect.EffectsAndSEs.Any())
                        {
                            await PlayBattleEffect(action, targetCombatant, battle, token);
                        }

                        await targetCombatant.ApplyActionResult(battle, token);

                        // ★暫定処理
                        await UniTask.Delay(400);
                    }
                }
            }
        }

        private async UniTask PlayBattleEffect(ActionModel battleAction, Combatant target, BattleManager battle, CancellationToken token)
        {
            Queue<EffectData.EffectAndSE> queue = new(battleAction.Effect.EffectsAndSEs);
            while (queue.Any())
            {
                float timer = 0.0f;
                EffectData.EffectAndSE effectAndSE = queue.Dequeue();

                if (timer >= effectAndSE.Timing)
                {
                    // エフェクト再生位置
                    Vector3 position = effectAndSE.EffectPlayPosition switch
                    {
                        EffectPlayPositionType.Targrt => target.Container.transform.position,
                        EffectPlayPositionType.Self => Container.transform.position,
                        _ => Vector3.zero
                    };

                    // エフェクトを再生する
                    EffectsManager.Play(effectAndSE.Effect, position, false);

                    await UniTask.Yield(token);
                }
            }
        }

        /// <summary>
        /// 行動の結果を反映させる
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <param name="token">トークン</param>
        private async UniTask ApplyActionResult(BattleManager battle, CancellationToken token)
        {
            // コンテナがなければ中止する
            if (!Container)
            {
                Debug.LogError("Container is null");
                Result.Initialize();
                return;
            }

            // 命中しなかった
            if (Result.Miss)
            {
                Container.OnMiss();
            }

            // クリティカル発生
            if (Result.Critical)
            {
                // ★ダメージにCRITICAL表示
            }

            // 弱点
            if (Result.Effectiveness == Attribute.Effectiveness.Weakness)
            {
                // ★ダメージにWEAKNESS表示
            }

            // 耐性
            if (Result.Effectiveness == Attribute.Effectiveness.Resistance)
            {
                // ★ダメージにREGISTANCE表示
            }

            // ダメージ
            if (Result.IsDamage())
            {
                // HP
                if (Result.HpDamage > 0)
                {
                    CurrentHP -= Result.HpDamage.Value;
                }

                // DP
                if (Result.DpDamage > 0)
                {
                    CurrentDP -= Result.DpDamage.Value;
                }

                // SP
                if (Result.SpDamage > 0)
                {
                    CurrentSP -= Result.SpDamage.Value;
                }

                // 1以上のダメージがある場合
                if (Result.IsOneOrOverDamage())
                {
                    AudioManager.PlaySE("ダメージ");
                }

                // ダメージイベント
                if (Container)
                {
                    Container.OnDamage();
                }
            }

            // 回復
            if (Result.IsRecovery())
            {
                // 生命力
                if (Result.HpRecovery > 0)
                {
                    CurrentHP += Result.HpRecovery.Value;
                }

                // 夢想力
                if (Result.DpRecovery > 0)
                {
                    CurrentDP += Result.DpRecovery.Value;
                }

                // 精神力
                if (Result.SpRecovery > 0)
                {
                    CurrentSP += Result.SpRecovery.Value;
                }

                // 1以上の回復がある場合
                if (Result.IsOneOrOverRecovery())
                {
                    AudioManager.PlaySE("回復");
                }

                // イベント
                if (Container)
                {
                    Container.OnRecovery();
                }
            }

            // ステータス効果追加
            foreach (var status in Result.AddedStatusEffects)
            {
                AddStatusEffect(status);
            }

            // ステータス効果削除
            foreach (var status in Result.RemovedStatusEffects)
            {
                StatusEffects.Remove(status);
            }

            // 浄化失敗
            if (Result.Purified == false)
            {
                if (Container)
                {
                    Container.OnMiss();
                }
            }

            // 浄化成功
            if (Result.Purified == true)
            {
                await BePurified(battle, token);
            }
            else if (!IsKnockedOut() && CurrentHP == 0) // 死亡
            {
                BeKnockedOut();
            }
            else if (IsKnockedOut() && CurrentHP > 0) // 復活
            {
                BeRevived();
            }
            else if (Result.Crushed) // クラッシュ
            {
                BeCrushed();
            }

            // 交代
            if (Result.IsChangeTarget)
            {
                Swap(Result.Affecter.Container);
            }

            // 行動結果を初期化する
            Result.Initialize();
        }

        /// <summary>
        /// 浄化される
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <param name="token">トークン</param>
        private async UniTask BePurified(BattleManager battle, CancellationToken token)
        {
            AudioManager.PlaySE("浄化");
            // 初期化する
            Initialize();

            // コンテナの処理
            if (Container)
            {
                // 敵コンテナに格納されている場合
                EnemyContainer enemyContainer = Container as EnemyContainer;
                if (enemyContainer)
                {
                    await enemyContainer.OnPurified(battle, token);
                }
            }
        }

        /// <summary>
        /// 倒される
        /// </summary>
        private void BeKnockedOut()
        {
            Condition = Attribute.Condition.KnockedOut;

            // 敵コンテナに格納されている場合
            if (Container && Container is EnemyContainer enemyContainer)
            {
                enemyContainer.OnKnockedOut();
            }
        }

        /// <summary>
        /// クラッシュする
        /// </summary>
        private void BeCrushed()
        {
            Condition = Attribute.Condition.Crush;
            RemainingCrushTurns = GameSettings.Combatants.CrushTurns;
        }

        /// <summary>
        /// 復活する
        /// </summary>
        private void BeRevived()
        {
            Condition = Attribute.Condition.Normal;
            WaitTime = GetBasicWaitTime();
            InitializeSoul();
        }

        /// <summary>
        /// 入れ替わる
        /// </summary>
        /// <param name="target">対象のコンテナ</param>
        public void Swap(CombatantContainer target)
        {
            // コンテナが存在しなければ中止
            if (!Container || !target)
            {
                return;
            }
            CombatantContainer temporaryContainer = Container;
            Combatant temporaryCombatant = target.Combatant;

            target.Combatant = this;
            temporaryContainer.Combatant = temporaryCombatant;
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
        private void MakeActionResults(ActionModel action, Combatant[] targets)
        {
            // 効果データ
            EffectData effect = action.Effect;

            foreach (var target in targets)
            {
                // 影響を与える者として自身を設定する
                target.Result.Affecter = this;

                // 命中判定
                target.Result.Miss = !JudgeTheHit(target, effect);

                // 命中しなかったら次の対象へ
                if (Result.Miss)
                {
                    continue;
                }

                // HP・DP・SPへの影響がある場合
                if (effect.AffectStatus())
                {
                    // 攻撃的な効果の場合
                    if (effect.IsOffensive)
                    {
                        // クリティカルの有無
                        target.Result.Critical = JudgeTheCritical(target, effect);

                        // 感情属性の有効性
                        target.Result.Effectiveness = target.GetEffectiveness(action.GetEmotion(this));
                    }

                    // ダメージ・回復量を決定する
                    foreach (var status in effect.Damages)
                    {
                        DecideDamageOrRecovery(action, target, status);
                    }

                    // 追加SPダメージを決定する
                    if (!target.IsCrushed() && target.Result.HpDamage > 0)
                    {
                        int damage = DecideExtraSpDamage(target, effect);
                        if (damage > 0)
                        {
                            target.Result.AddSpDamage(damage);
                        }
                    }
                }

                // クラッシュ判定
                if (!target.IsCrushed() &&
                    target.Result.SpDamage.HasValue &&
                    target.CurrentSP - target.Result.SpDamage <= 0)
                {
                    target.Result.Crushed = true;
                }

                // ステータス効果を付与する場合
                if (effect.AddsStatusEffects())
                {
                    foreach (var data in effect.StatusEffects)
                    {
                        // 付与判定に成功した場合
                        if (RandomUtility.JudgeByPercentage(data.Rate))
                        {
                            // ステータス効果を追加する
                            target.Result.AddedStatusEffects.Add(data);
                        }
                    }
                }

                // 浄化スキルの場合
                if (effect.IsPurification)
                {
                    // 浄化成功率
                    target.Result.PurificationSuccessRate = target.GetPurificationSuccessRate(this);

                    // 判定
                    if (RandomUtility.JudgeByPercentage(target.Result.PurificationSuccessRate))
                    {
                        // 成功
                        target.Result.Purified = true;
                    }
                    else
                    {
                        // 失敗
                        target.Result.Purified = false;
                    }
                }

                // 交代スキルの場合
                if (effect.IsChange)
                {
                    target.Result.IsChangeTarget = true;
                }
            }
        }

        /// <summary>
        /// 命中判定を行う
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="effect">効果</param>
        /// <returns></returns>
        private bool JudgeTheHit(Combatant target, EffectData effect)
        {
            // 非攻撃的な行動は必ず命中する
            if (!effect.IsOffensive)
            {
                return true;
            }

            // 命中率
            int hit = Hit + effect.Hit - target.Evasion;

            // 判定
            bool result = RandomUtility.JudgeByPercentage(hit);
            return result;
        }

        /// <summary>
        /// クリティカル判定を行う
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="effect">効果</param>
        /// <returns></returns>
        private bool JudgeTheCritical(Combatant target, EffectData effect)
        {
            // 非攻撃的な行動はクリティカルにならない
            if (!effect.IsOffensive)
            {
                return false;
            }

            // クリティカル率
            int rate = CriticalHit + effect.Critical - target.CriticalEvasion;

            bool result = RandomUtility.JudgeByPercentage(rate);
            return result;
        }

        /// <summary>
        /// 属性に対する有効性を取得する
        /// </summary>
        /// <param name="emotion">属性</param>
        /// <returns></returns>
        public Attribute.Effectiveness GetEffectiveness(Attribute.Emotion emotion)
        {
            return GameSettings.GetEffectiveness(emotion, Emotion);
        }

        /// <summary>
        /// ダメージまたは回復量を決定する
        /// </summary>
        /// <param name="action">行動内容</param>
        /// <param name="target">対象</param>
        /// <param name="status">ダメージデータ</param>
        private void DecideDamageOrRecovery(ActionModel action, Combatant target, EffectData.Damage status)
        {
            // 値にステータス効果を適用する
            void ApplyStatusEffect(ref int value, float strength, float technique)
            {
                float rate = action.GetAttack() switch
                {
                    Attribute.Attack.Strength => strength,
                    Attribute.Attack.Technique => technique,
                    Attribute.Attack.Mix => strength + technique,
                    _ => 1.0f
                };
                MathUtility.Multiply(ref value, rate);
            }

            // 基本値
            int value = DecideBasicValue(action.GetAttack(), status, target);

            // 威力補正
            value = Mathf.FloorToInt(value * status.Parameter);

            // 攻撃的な効果の場合
            if (action.Effect.IsOffensive)
            {
                // 感情属性補正
                MathUtility.Multiply(ref value, GameSettings.Effectiveness.Rates.GetValueOrDefault(target.GetEffectiveness(action.GetEmotion(this))));

                // クリティカル補正
                if (target.Result.Critical)
                {
                    MathUtility.Multiply(ref value, GameSettings.Combatants.CriticalDamageCorrection);
                }

                // 相手のクラッシュ補正
                if (target.IsCrushed() && status.IsDamage())
                {
                    MathUtility.Multiply(ref value, GameSettings.Combatants.GivingDamageCorrectionWhenCrush);
                }

                // 自分のクラッシュ補正
                if (IsCrushed())
                {
                    MathUtility.Multiply(ref value, GameSettings.Combatants.TakingDamageAndRecoveryCorrectionWhenCrush);
                }

                // 攻撃倍率
                foreach (var statusEffect in StatusEffects)
                {
                    float str = statusEffect.Data.PowerAttackRate;
                    float tec = statusEffect.Data.TechniqueAttackRate;
                    ApplyStatusEffect(ref value, str, tec);
                }

                // 防御倍率
                foreach (var statusEffect in target.StatusEffects)
                {
                    float str = statusEffect.Data.PowerDefenseRate;
                    float tec = statusEffect.Data.TechniqueDefenseRate;
                    ApplyStatusEffect(ref value, str, tec);
                }
            }

            // 固定値を加算する
            value += status.Fixed;

            // 固定乱数値を加算する
            value += Random.Range(0, status.Random);

            // 影響するステータスに応じて結果に値を加算する
            switch (status.Affected)
            {
                // HP
                case Affected.Hp:

                    // ダメージ
                    if (status.IsDamage())
                    {
                        // ゼロにしない場合
                        if (status.DontToZero && value >= target.CurrentHP)
                        {
                            value = target.CurrentHP - 1;
                        }

                        target.Result.AddHpDamage(value);
                    }
                    else // 回復
                    {
                        target.Result.AddHpRecovery(value);
                    }
                    break;

                // DP
                case Affected.Dp:

                    // ダメージ
                    if (status.IsDamage())
                    {
                        // ゼロにしない場合
                        if (status.DontToZero && value >= target.CurrentDP)
                        {
                            value = target.CurrentDP - 1;
                        }

                        target.Result.AddDpDamage(value);
                    }
                    else //回復
                    {
                        target.Result.AddDpRecovery(value);
                    }
                    break;

                // SP
                case Affected.Sp:

                    // ダメージ
                    if (status.IsDamage())
                    {
                        // ゼロにしない場合
                        if (status.DontToZero && value >= target.CurrentSP)
                        {
                            value = target.CurrentSP - 1;
                        }

                        target.Result.AddSpDamage(value);
                    }
                    else // 回復
                    {
                        target.Result.AddSpRecovery(value);
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ダメージor回復量の基本値を決定する
        /// </summary>
        /// <param name="attack">攻撃属性</param>
        /// <param name="status">ダメージデータ</param>
        /// <param name="target">対象</param>
        /// <returns></returns>
        private int DecideBasicValue(Attribute.Attack attack, EffectData.Damage status, Combatant target)
        {
            int result = 0;

            switch (status.Formula)
            {
                case Formula.Status:
                    result = DecideBasicValueByStatus(attack);
                    break;

                case Formula.Rate:
                    result = DecideBasicValueByRate(status.Affected, target);
                    break;
            }

            return result;
        }

        /// <summary>
        /// ステータスによるダメージor回復値の基本値を決定する
        /// </summary>
        /// <param name="attack">攻撃属性</param>
        /// <returns></returns>
        private int DecideBasicValueByStatus(Attribute.Attack attack)
        {
            // 攻撃力
            int value = 0;

            // 攻撃属性
            switch (attack)
            {
                // STR依存
                case Attribute.Attack.Strength:
                    value = Power;
                    break;

                // TEC依存
                case Attribute.Attack.Technique:
                    value = Technique;
                    break;

                // STR・TEC混合
                case Attribute.Attack.Mix:
                    value = (Power + Technique) / 2;
                    break;
            }

            return value;
        }

        /// <summary>
        /// 割合ダメージor回復の基本値を決定する
        /// </summary>
        /// <param name="affected">影響するステータスの種類</param>
        /// <param name="target">対象</param>
        /// <returns></returns>
        private int DecideBasicValueByRate(Affected affected, Combatant target)
        {
            int result = 0;

            // 影響を与えるステータス
            switch (affected)
            {
                case Affected.Hp:
                    result = target.MaxHP;
                    break;

                case Affected.Dp:
                    result = GameSettings.Combatants.MaxDP;
                    break;

                case Affected.Sp:
                    result = target.MaxSP;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 追加SPダメージを決定する
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="effect">効果</param>
        /// <returns></returns>
        private int DecideExtraSpDamage(Combatant target, EffectData effect)
        {
            int damage = 0;

            // 弱点属性
            switch (target.Result.Effectiveness)
            {
                case Attribute.Effectiveness.Weakness:
                    damage += effect.SoulDamage;
                    break;

                case Attribute.Effectiveness.SuperWeakness:
                    damage += target.CurrentSP;
                    break;
            }

            // クリティカル
            if (target.Result.Critical)
            {
                damage += GameSettings.Combatants.CriticalSoulDamage;
            }

            return damage;
        }
    }
}
