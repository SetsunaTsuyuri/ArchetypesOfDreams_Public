using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵スプライト
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemySprite : MonoBehaviour, IInitializable
    {
        SpriteRenderer _spriteRenderer = null;

        /// <summary>
        /// 初期色
        /// </summary>
        Color _defaultColor = Color.white;

        /// <summary>
        /// 初期の拡大倍率X
        /// </summary>
        float _defaultLocalScaleX = 0.0f;

        /// <summary>
        /// 初期の拡大倍率Y
        /// </summary>
        float _defaultLocalScaleY = 0.0f;

        /// <summary>
        /// 対象Tween
        /// </summary>
        Tween targetedTween = null;

        /// <summary>
        /// 撃破Tween
        /// </summary>
        Tween _knockedOutTween = null;

        /// <summary>
        /// 点滅シーケンス
        /// </summary>
        Sequence _blinkingSequence = null;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultColor = _spriteRenderer.color;
            _defaultLocalScaleX = transform.localScale.x;
            _defaultLocalScaleY = transform.localScale.y;
            _blinkingSequence = CreateBlinkingSequence();
        }

        public void Initialize()
        {
            _spriteRenderer.color = _defaultColor;
            Vector3 scale = new (_defaultLocalScaleX, _defaultLocalScaleY, 1.0f);
            transform.localScale = scale;
        }

        /// <summary>
        /// 戦闘者が設定されたときの処理
        /// </summary>
        /// <param name="combatant">戦闘者</param>
        public void OnComabtantSet(Combatant combatant)
        {
            if (combatant != null)
            {
                _spriteRenderer.sprite = combatant.Data.Sprite;
                Vector3 scale = transform.localScale;
                scale.x = _defaultLocalScaleX * combatant.Data.SpriteScale;
                scale.y = _defaultLocalScaleY * combatant.Data.SpriteScale;
                transform.localScale = scale;
                _spriteRenderer.enabled = true;
            }
            else
            {
                _spriteRenderer.enabled = false;
            }
        }

        /// <summary>
        /// 戦闘者の健康状態が変化したときの処理
        /// </summary>
        /// <param name="combatant">戦闘者</param>
        public void OnConditionSet(Combatant combatant)
        {
            switch (combatant.Condition)
            {
                case Attribute.Condition.Normal:
                    // スプライト表示
                    _knockedOutTween.Kill();
                    _spriteRenderer.enabled = true;
                    _spriteRenderer.color = _defaultColor;
                    break;

                case Attribute.Condition.Crush:
                    // スプライト変化
                    break;

                case Attribute.Condition.KnockedOut:
                    // スプライト非表示
                    _knockedOutTween = ChangeColor(
                        GameSettings.VisualEffects.DefeatedEnemyColor,
                        GameSettings.VisualEffects.EnemyFadeDuration);
                    break;
            }
        }

        /// <summary>
        /// 点滅シーケンスを作る
        /// </summary>
        private Sequence CreateBlinkingSequence()
        {
            return DOTween.Sequence()
                .AppendCallback(() => _spriteRenderer.material.SetFloat("_Blinking", 1.0f))
                .AppendInterval(0.075f)
                .AppendCallback(() => _spriteRenderer.material.SetFloat("_Blinking", 0.0f))
                .AppendInterval(0.075f)
                .SetLoops(2)
                .SetLink(gameObject)
                .SetAutoKill(false)
                .Pause();
        }

        /// <summary>
        /// 点滅する
        /// </summary>
        public void Blink()
        {
            _blinkingSequence.Restart();
        }

        /// <summary>
        /// 揺れる
        /// </summary>
        public void Shake()
        {
            float duration = GameSettings.Enemies.DamageShakeDuration;
            float strength = GameSettings.Enemies.DamageShakeStrength;
            int vibrato = GameSettings.Enemies.DamageShakeVibrato;
            transform
                .DOShakePosition(duration, strength, vibrato)
                .SetLink(gameObject);
        }

        /// <summary>
        /// スプライトの色を変える
        /// </summary>
        private Tween ChangeColor(Color color, float duration)
        {
            return _spriteRenderer.DOColor(color, duration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);
        }

        /// <summary>
        /// 浄化されたときの処理
        /// </summary>
        public void OnPurified()
        {
            // スプライト非表示
            _spriteRenderer.enabled = false;
        }

        /// <summary>
        /// 対象フラグが設定されたときの処理
        /// </summary>
        /// <param name="targeted">対象になった</param>
        public void OnTargetFlagSet(bool targeted)
        {
            if (targeted)
            {
                StartTargetedTween();
            }
            else
            {
                KillTargetedTween();
            }
        }

        /// <summary>
        /// 対象に選択されたときの色変化Tweenを開始する
        /// </summary>
        private void StartTargetedTween()
        {
            Color color = GameSettings.VisualEffects.TargetedEnemyColor;
            float duration = GameSettings.VisualEffects.TargetedEnemyColorChangeDuration;

            targetedTween = _spriteRenderer.DOColor(color, duration)
                .SetEase(Ease.OutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        /// <summary>
        /// 対象に選択されたときの色変化Tweenを終了する
        /// </summary>
        public void KillTargetedTween()
        {
            // Tweenを殺す
            if (targetedTween != null)
            {
                targetedTween.Kill();
            }

            // 元の色に戻す
            if (_spriteRenderer)
            {
                _spriteRenderer.color = _defaultColor;
            }
        }
    }
}
