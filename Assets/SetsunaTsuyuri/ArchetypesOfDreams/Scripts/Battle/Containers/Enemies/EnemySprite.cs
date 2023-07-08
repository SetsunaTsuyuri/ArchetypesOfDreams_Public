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
        /// <summary>
        /// スプライトレンダラー
        /// </summary>
        SpriteRenderer _spriteRenderer = null;

        /// <summary>
        /// 初期色
        /// </summary>
        Color _defaultColor = Color.white;

        /// <summary>
        /// 初期拡大倍率X
        /// </summary>
        float _defaultLocalScaleX = 0.0f;

        /// <summary>
        /// 初期拡大倍率Y
        /// </summary>
        float _defaultLocalScaleY = 0.0f;

        /// <summary>
        /// 対象Tween
        /// </summary>
        Tween targetedTween = null;

        /// <summary>
        /// 点滅Tween
        /// </summary>
        Tween _blinkingTween = null;

        /// <summary>
        /// 崩壊Tween
        /// </summary>
        Tween _collapseTween = null;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultColor = _spriteRenderer.color;
            _defaultLocalScaleX = transform.localScale.x;
            _defaultLocalScaleY = transform.localScale.y;
            _blinkingTween = CreateBlinking();
            _collapseTween = CreateCollapse();
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
            if (combatant is null)
            {
                _spriteRenderer.sprite = null;
                return;
            }

            Vector3 scale = transform.localScale;
            scale.x = _defaultLocalScaleX * combatant.Data.SpriteScale;
            scale.y = _defaultLocalScaleY * combatant.Data.SpriteScale;
            transform.localScale = scale;
        }

        /// <summary>
        /// スプライトを更新する
        /// </summary>
        /// <param name="combatant">戦闘者</param>
        public void UpdateSprite(Combatant combatant)
        {
            if (combatant is null)
            {
                _spriteRenderer.enabled = false;
                return;
            }

            _spriteRenderer.sprite = combatant.Sprite;
            _spriteRenderer.enabled = true;
        }

        /// <summary>
        /// 崩壊Tweenを作る
        /// </summary>
        /// <returns></returns>
        private Tween CreateCollapse()
        {
            Color color = GameSettings.VisualEffects.DefeatedEnemyColor;
            float duration = GameSettings.VisualEffects.EnemyFadeDuration;

            Tween tween = ChangeColor(color, duration)
                .SetLink(gameObject)
                .SetAutoKill(false)
                .Pause();

            return tween;
        }

        /// <summary>
        /// 点滅Tweenを作る
        /// </summary>
        private Tween CreateBlinking()
        {
            Tween tween = DOTween.Sequence()
                .AppendCallback(() => _spriteRenderer.material.SetFloat("_Blinking", 1.0f))
                .AppendInterval(0.075f)
                .AppendCallback(() => _spriteRenderer.material.SetFloat("_Blinking", 0.0f))
                .AppendInterval(0.075f)
                .SetLoops(2)
                .SetLink(gameObject)
                .SetAutoKill(false)
                .Pause();

            return tween;
        }

        /// <summary>
        /// 点滅する
        /// </summary>
        public void Blink()
        {
            _blinkingTween.Restart();
        }

        /// <summary>
        /// 崩壊する
        /// </summary>
        public void Collapse()
        {
            _collapseTween.Restart();
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
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        /// <summary>
        /// 対象に選択されたときの色変化Tweenを終了する
        /// </summary>
        public void KillTargetedTween()
        {
            if (targetedTween != null)
            {
                targetedTween.Kill();
            }

            _spriteRenderer.color = _defaultColor;
        }
    }
}
