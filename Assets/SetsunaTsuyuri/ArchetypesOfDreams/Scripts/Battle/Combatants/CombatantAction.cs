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
        /// 行動開始時の処理
        /// </summary>
        public void OnActionStart()
        {
            // 防御状態を解除する
            IsDefending = false;
        }

        /// <summary>
        /// 行動終了後の処理
        /// </summary>
        public void OnActionEnd()
        {
        }

        /// <summary>
        /// ターン終了時の処理
        /// </summary>
        public void OnTurnEnd()
        {
            // クラッシュ状態の場合
            if (IsCrushed())
            {
                // カウントダウン
                RemainingCrushTurns--;
                if (RemainingCrushTurns == 0)
                {
                    // クラッシュ状態から復帰する
                    Condition = Attribute.Condition.Normal;
                    ChangeSoul();
                    RecoverSoulFully();
                }
            }

            // 夢想力増加
            Dream += GameSettings.Combatants.AmoutOfIncreaseInDreamPerTurn;

            // 行動済みフラグOFF
            Acted = false;
        }

        /// <summary>
        /// ターン終了時の処理(控えにいるとき)
        /// </summary>
        public void OnTurnEndReserve()
        {
            // 精神力回復
            Soul += GameSettings.Combatants.AmoutOfSoulRecoverdPerTurn;
        }

        /// <summary>
        /// 戦闘終了時の処理
        /// </summary>
        public void OnBattleEnd()
        {
            // 行動済みフラグOFF
            Acted = false;

            // 精神初期化
            InitializeSoul();
        }

        /// <summary>
        /// 対象選択時に必要な行動結果を作る
        /// </summary>
        /// <param name="skill">スキル</param>
        /// <param name="targetables">対象にできる戦闘者コンテナ</param>
        public void MakeActionResultOnTargetSelection(Skill skill, CombatantContainer[] targetables)
        {
            foreach (var targetable in targetables)
            {
                // 影響を与える者として自身を設定する
                targetable.Combatant.Result.Affecter = this;

                // 攻撃的なスキルの場合
                if (skill.Data.Effect.IsOffensive)
                {
                    // 感情属性の有効性
                    targetable.Combatant.Result.Effectiveness = targetable.Combatant.GetEffectiveness(skill.GetEmotion());
                }

                // 浄化スキルの場合
                if (skill.Data.Effect.IsPurification)
                {
                    // 浄化成功率
                    DreamWalker dreamWalker = this as DreamWalker;
                    Nightmare nightmare = targetable.Combatant as Nightmare;
                    if (dreamWalker != null && nightmare != null)
                    {
                        nightmare.Result.PurificationSuccessRate = nightmare.GetPurificationSuccessRate(dreamWalker);
                    }
                }
            }
        }

        /// <summary>
        /// スキルを使う
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <param name="token">トークン</param>
        public async UniTask UseSkill(BattleManager battle, CancellationToken token)
        {
            // コンテナがなければ中止する
            if (!Container)
            {
                Debug.LogError("Container is null");
                return;
            }

            // 対象
            CombatantContainer[] targets = battle.Targets;

            // スキル
            Skill skill = battle.SkillToBeUsed;

            // 夢想力を消費する
            Dream -= skill.Data.Cost;

            // ★使用者が誰か分かる演出
            // 敵スプライトが光る カメラが寄る等 絆スキルカットイン

            // スキル使用イベント
            Container.OnSkillUsed(skill);

            // ★暫定処理
            await UniTask.Delay(200);

            // 実行回数分繰り返す
            for (int i = 0; i < skill.Data.Effect.Executions; i++)
            {
                // 対象の戦闘者を取り出す
                Combatant[] targetCombatants = GetTargetCombatants(targets, skill.Data.Effect.TargetSelection);

                // 対象が存在する
                if (targetCombatants.Any())
                {
                    // 行動の結果を作る
                    MakeActionResults(skill, targetCombatants);

                    // 結果を反映させる
                    foreach (var targetCombatant in targetCombatants)
                    {
                        await targetCombatant.ApplyActionResult(battle, skill.Data.Effect, token);

                        // ★暫定処理
                        await UniTask.Delay(400);
                    }
                }

            }

            // 交代以外は行動済みフラグON
            if (!skill.Data.Effect.IsChange)
            {
                Acted = true;
            }
        }

        /// <summary>
        /// 行動の結果を反映させる
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        /// <param name="effect">効果データ</param>
        /// <param name="token">トークン</param>
        private async UniTask ApplyActionResult(BattleManager battle, EffectData effect, CancellationToken token)
        {
            // コンテナがなければ中止する
            if (!Container)
            {
                Debug.LogError("Container is null");

                Result.Initialize();
                return;
            }

            // 命中しなかった
            if (!Result.Hit)
            {
                if (Container)
                {
                    Container.OnMiss();
                }
                Result.Initialize();
                return;
            }

            // クリティカル
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
                // 生命力
                if (Result.LifeDamage > 0)
                {
                    Life -= (int)Result.LifeDamage;
                }

                // 夢想力
                if (Result.DreamDamage > 0)
                {
                    Dream -= (int)Result.DreamDamage;
                }

                // 精神力
                if (Result.SoulDamage > 0)
                {
                    Soul -= (int)Result.SoulDamage;
                }

                // 1以上のダメージがある場合
                if (Result.IsOneOrOverDamage())
                {
                    // 敵スプライト・UIが揺れる Container.OnOneOrOverDamager()を作りAllyContainerとEnemyContainerでそれぞれオーバーライド
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
                if (Result.LifeRecovery > 0)
                {
                    Life += (int)Result.LifeRecovery;
                }

                // 夢想力
                if (Result.DreamRecovery > 0)
                {
                    Dream += (int)Result.DreamRecovery;
                }

                // 精神力
                if (Result.SoulRecovery > 0)
                {
                    Soul += (int)Result.SoulRecovery;
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

            // 防御
            if (Result.IsDefending)
            {
                IsDefending = true;
            }

            // 浄化失敗
            if (effect.IsPurification && !Result.Purified)
            {
                if (Container)
                {
                    Container.OnMiss();
                }
            }

            // 浄化成功
            if (Result.Purified)
            {
                await BePurified(battle, token);
            }
            else if (!IsKnockedOut() && Life == 0) // 死亡
            {
                BeKnockedOut(token);
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
        private void BeKnockedOut(CancellationToken token)
        {
            Condition = Attribute.Condition.KnockedOut;

            // コンテナの処理
            if (Container)
            {
                // 敵コンテナに格納されている場合
                EnemyContainer enemyContainer = Container as EnemyContainer;
                if (enemyContainer)
                {
                    enemyContainer.OnKnockedOut();
                }
            }
        }

        /// <summary>
        /// クラッシュするs
        /// </summary>
        private void BeCrushed()
        {
            Condition = Attribute.Condition.Crush;
            RemainingCrushTurns = GameSettings.Combatants.CrushTurns;
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
        /// <param name="containers">コンテナ配列</param>
        /// <param name="selection">対象の選択方法</param>
        /// <returns></returns>
        private Combatant[] GetTargetCombatants(CombatantContainer[] containers, Effect.TargetSelection selection)
        {
            // 対象にできる戦闘者配列
            Combatant[] targets = containers
                .Where(c => c.ContainsFightable())
                .Select(c => c.Combatant)
                .ToArray();

            // ランダムに対象を選ぶ場合、配列内から1体のみをランダムに選ぶ
            if (selection == Effect.TargetSelection.Random && targets.Length > 1)
            {
                int index = Random.Range(0, targets.Length);
                targets = new Combatant[] { targets[index] };
            }
            return targets;
        }

        /// <summary>
        /// 行動結果を作る(行動実行時)
        /// </summary>
        /// <param name="skill">スキル</param>
        /// <param name="targets">対象配列</param>
        private void MakeActionResults(Skill skill, Combatant[] targets)
        {
            // 効果データ
            EffectData effect = skill.Data.Effect;

            foreach (var target in targets)
            {
                // 影響を与える者として自身を設定する
                target.Result.Affecter = this;

                // 命中判定
                target.Result.Hit = JudgeTheHit(target, effect);

                // 命中しなかったら次の対象へ
                if (!target.Result.Hit)
                {
                    continue;
                }

                // 生命力・夢想力・精神力への影響がある場合
                if (effect.AffectStatus())
                {
                    // 攻撃的な効果の場合
                    if (effect.IsOffensive)
                    {
                        // クリティカルの有無
                        target.Result.Critical = JudgeTheCritical(target, effect);

                        // 感情属性の有効性
                        target.Result.Effectiveness = target.GetEffectiveness(skill.GetEmotion());
                    }

                    // ダメージ・回復量を決定する
                    foreach (var status in effect.EffectsOnStatus)
                    {
                        DecideDamageOrRecovery(skill, target, effect, status);
                    }

                    // 追加精神ダメージを決定する
                    if (!target.IsCrushed() && target.Result.LifeDamage > 0)
                    {
                        int damage = DecideExtraSoulDamage(target, effect);
                        if (damage > 0)
                        {
                            target.Result.AddSoulDamage(damage);
                        }
                    }
                }

                // クラッシュ判定
                if (!target.IsCrushed() &&
                    target.Result.SoulDamage.HasValue &&
                    target.Soul - target.Result.SoulDamage <= 0)
                {
                    target.Result.Crushed = true;
                }

                // 防御スキルの場合
                if (effect.IsDefending)
                {
                    target.Result.IsDefending = true;
                }

                // 浄化スキルの場合
                if (effect.IsPurification)
                {
                    DreamWalker dreamWalker = this as DreamWalker;
                    Nightmare nightmare = target as Nightmare;
                    if (dreamWalker != null && nightmare != null)
                    {
                        // 浄化成功率
                        nightmare.Result.PurificationSuccessRate = nightmare.GetPurificationSuccessRate(dreamWalker);

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
        /// <param name="skill">スキル</param>
        /// <param name="target">対象</param>
        /// <param name="effect">効果データ</param>
        /// <param name="status">ステータスへの影響データ</param>
        private void DecideDamageOrRecovery(Skill skill, Combatant target, EffectData effect, DataOfEffectOnStatus status)
        {
            // 基本値
            float valueFloat = DecideBaseValue(skill.GetAttack(), status, target);

            // 威力補正
            valueFloat *= status.Parameter;

            // 攻撃的な効果の場合
            if (effect.IsOffensive)
            {
                // 感情属性補正
                valueFloat *= GameSettings.Effectiveness.Rates.GetValueOrDefault(target.GetEffectiveness(skill.GetEmotion()));

                // クリティカル補正
                if (target.Result.Critical)
                {
                    valueFloat *= GameSettings.Combatants.CriticalDamageCorrection;
                }

                // 防御補正
                if (target.IsDefending && status.IsDamage())
                {
                    valueFloat *= GameSettings.Combatants.DefendingDamageCorrection;
                }

                // 相手のクラッシュ補正
                if (target.IsCrushed() && status.IsDamage())
                {
                    valueFloat *= GameSettings.Combatants.GivingDamageCorrectionWhenCrush;
                }

                // 自分のクラッシュ補正
                if (IsCrushed())
                {
                    valueFloat *= GameSettings.Combatants.TakingDamageAndRecoveryCorrectionWhenCrush;
                }
            }

            // 値をint型にする
            int valueInt = Mathf.FloorToInt(valueFloat);

            // 固定値を加算する
            valueInt += status.Fixed;

            // 固定乱数値を加算する
            valueInt += Random.Range(0, status.Random);

            // 影響するステータスに応じて結果に値を加算する
            switch (status.Affected)
            {
                // 生命力
                case Effect.Affected.Life:

                    // ダメージ
                    if (status.IsDamage())
                    {
                        // ゼロにしない場合
                        if (status.DontToZero && valueInt >= target.Life)
                        {
                            valueInt = target.Life - 1;
                        }

                        target.Result.AddLifeDamage(valueInt);
                    }
                    else // 回復
                    {
                        target.Result.AddLifeRecovery(valueInt);
                    }
                    break;

                // 夢想力
                case Effect.Affected.Dream:

                    // ダメージ
                    if (status.IsDamage())
                    {
                        // ゼロにしない場合
                        if (status.DontToZero && valueInt >= target.Dream)
                        {
                            valueInt = target.Dream - 1;
                        }

                        target.Result.AddDreamDamage(valueInt);
                    }
                    else //回復
                    {
                        target.Result.AddDreamRecovery(valueInt);
                    }
                    break;

                // 精神力
                case Effect.Affected.Soul:

                    // ダメージ
                    if (status.IsDamage())
                    {
                        // ゼロにしない場合
                        if (status.DontToZero && valueInt >= target.Soul)
                        {
                            valueInt = target.Soul - 1;
                        }

                        target.Result.AddSoulDamage(valueInt);
                    }
                    else // 回復
                    {
                        target.Result.AddSoulRecovery(valueInt);
                    }

                    break;
            }
        }

        /// <summary>
        /// ダメージor回復量の基本値を決定する
        /// </summary>
        /// <param name="attack">攻撃属性</param>
        /// <param name="status">ステータスへの影響データ</param>
        /// <param name="target">対象</param>
        /// <returns></returns>
        private float DecideBaseValue(Attribute.Attack attack, DataOfEffectOnStatus status, Combatant target)
        {
            float result = 0.0f;

            switch (status.Formula)
            {
                case Effect.Formula.AttackAndDefense:
                    result = DecideBaseValueByStatus(attack, status, target);
                    break;

                case Effect.Formula.Rate:
                    result = DecideBaseValueByRate(status.Affected, target);
                    break;
            }

            return result;
        }

        /// <summary>
        /// ステータスによるダメージor回復値の基本値を決定する
        /// </summary>
        /// <param name="attack">攻撃属性</param>
        /// <param name="data">ステータスへの影響データ</param>
        /// <param name="target">対象</param>
        /// <returns></returns>
        private float DecideBaseValueByStatus(Attribute.Attack attack, DataOfEffectOnStatus data, Combatant target)
        {
            // 攻撃力
            int offensivePower = 0;

            // 守備力
            int defenseivePower = 0;

            // 攻撃属性
            switch (attack)
            {
                // 近接武器
                case Attribute.Attack.Melee:
                    offensivePower = MeleeAttack;
                    if (data.IsDamage())
                    {
                        defenseivePower = target.MeleeDefense;
                    }
                    break;

                // 遠隔武器
                case Attribute.Attack.Ranged:
                    offensivePower = RangedAttack;
                    if (data.IsDamage())
                    {
                        defenseivePower = target.RangedDefense;
                    }
                    break;

                // 近接・遠隔混合
                case Attribute.Attack.Mix:
                    offensivePower = (MeleeAttack + RangedAttack) / 2;
                    if (data.IsDamage())
                    {
                        defenseivePower = (target.MeleeDefense + target.RangedDefense) / 2;
                    }
                    break;
            }

            // 攻撃力 - 守備力%
            float result = offensivePower * (1.0f - defenseivePower / 100.0f);

            // ゼロ未満にしない
            if (result < 0.0f)
            {
                result = 0.0f;
            }

            return result;
        }

        /// <summary>
        /// 割合ダメージor回復の基本値を決定する
        /// </summary>
        /// <param name="affected">影響するステータスの種類</param>
        /// <param name="target">対象</param>
        /// <returns></returns>
        private float DecideBaseValueByRate(Effect.Affected affected, Combatant target)
        {
            float result = 0.0f;

            // 影響を与えるステータス
            switch (affected)
            {
                case Effect.Affected.Life:
                    result = target.MaxLife;
                    break;

                case Effect.Affected.Dream:
                    result = GameSettings.Combatants.MaxSoul;
                    break;

                case Effect.Affected.Soul:
                    result = target.MaxSoul;
                    break;
            }

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
        /// 追加精神ダメージを決定する
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="effect">効果</param>
        /// <returns></returns>
        private int DecideExtraSoulDamage(Combatant target, EffectData effect)
        {
            int damage = 0;

            // 感情属性
            switch (target.Result.Effectiveness)
            {
                case Attribute.Effectiveness.Weakness:
                    damage += effect.SoulDamage;
                    break;

                case Attribute.Effectiveness.SuperWeakness:
                    damage += target.Soul;
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
