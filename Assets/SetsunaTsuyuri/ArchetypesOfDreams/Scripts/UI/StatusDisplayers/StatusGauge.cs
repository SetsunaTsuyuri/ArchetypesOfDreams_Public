using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲージによるInt型のステータス表示
    /// </summary>
    public abstract class StatusGauge : StatusDisplayer<Image, int>
    {
        /// <summary>
        /// 最大値
        /// </summary>
        protected int maxValue;

        /// <summary>
        /// 最後に表示された最大値
        /// </summary>
        protected int theLastMaxValue;

        /// <summary>
        /// ダメージゲージ
        /// </summary>
        [SerializeField]
        Image _damage = null;

        /// <summary>
        /// ダメージTween
        /// </summary>
        Tween _damageTween = null;

        /// <summary>
        /// ダメージTween開始を要求されている
        /// </summary>
        bool _isRequestedToStartDamageTween = false;

        float _damageTimer = 0.0f;

        protected virtual void Start()
        {
            // ダメージ
            MessageBrokersManager.Damage
                .Receive<CombatantContainer>()
                .Where(x => x == target && CanStartDamageTween(x.Combatant.Results.Damage))
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ =>
                {
                    // ダメージゲージ表示
                    _damage.enabled = true;
                    if (_damage.fillAmount < view.fillAmount)
                    {
                        _damage.fillAmount = view.fillAmount;
                    }

                    // 更新
                    UpdateValue();
                    UpdateView();

                    // ダメージゲージタイマー始動
                    _isRequestedToStartDamageTween = true;
                    _damageTimer = GameSettings.Combatants.DamageGaugeWaitDuration;
                });
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (_isRequestedToStartDamageTween)
            {
                if (_damageTimer <= 0.0f
                    && !_damageTween.IsActive())
                {
                    StartDamageTween();
                }
                else if (_damageTimer > 0.0f)
                {
                    _damageTimer -= Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// ダメージTweenを開始できる
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        protected abstract bool CanStartDamageTween(DamageResult damage);

        /// <summary>
        /// ダメージTweenを開始する
        /// </summary>
        protected void StartDamageTween()
        {
            _isRequestedToStartDamageTween = false;
            _damageTween.Kill();
            _damageTween = _damage.DOFillAmount(view.fillAmount, GameSettings.Combatants.DamageGaugeDuration)
                .SetEase(Ease.OutQuad)
                .SetLink(gameObject)
                .OnComplete(() => _damage.enabled = false);
        }

        protected override void UpdateView()
        {
            float amount = (float)value / maxValue;
            view.fillAmount = amount;
        }

        protected override void UpdateValue()
        {
            if (target.ContainsCombatant)
            {
                (int, int) result = GetValues(target.Combatant);
                value = result.Item1;
                maxValue = result.Item2;
            }
        }

        protected override void UpdateTheLastValue()
        {
            base.UpdateTheLastValue();

            theLastMaxValue = maxValue;
        }

        /// <summary>
        /// 値を取得する
        /// </summary>
        /// <returns></returns>
        protected abstract (int, int) GetValues(Combatant combatant);
    }
}