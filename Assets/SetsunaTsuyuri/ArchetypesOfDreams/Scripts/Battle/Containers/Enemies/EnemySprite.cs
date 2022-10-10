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
    public class EnemySprite : MonoBehaviour
    {
        SpriteRenderer spriteRenderer = null;

        /// <summary>
        /// 初期色
        /// </summary>
        Color defaultColor = Color.white;

        /// <summary>
        /// 初期の拡大倍率X
        /// </summary>
        float defaultLocalScaleX = 0.0f;

        /// <summary>
        /// 初期の拡大倍率Y
        /// </summary>
        float defaultLocalScaleY = 0.0f;

        /// <summary>
        /// 色変化Tween
        /// </summary>
        Tweener targetedColor = null;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            defaultColor = spriteRenderer.color;
            defaultLocalScaleX = transform.localScale.x;
            defaultLocalScaleY = transform.localScale.y;
        }

        /// <summary>
        /// 戦闘者が設定されたときの処理
        /// </summary>
        /// <param name="combatant">戦闘者</param>
        public void OnComabtantSet(Combatant combatant)
        {
            if (combatant != null)
            {
                spriteRenderer.sprite = combatant.Data.Sprite;
                Vector3 scale = transform.localScale;
                scale.x = defaultLocalScaleX * combatant.Data.SpriteScale;
                scale.y = defaultLocalScaleY * combatant.Data.SpriteScale;
                transform.localScale = scale;
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
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
                    spriteRenderer.enabled = true;
                    break;

                case Attribute.Condition.Crush:
                    // スプライト変化
                    break;

                case Attribute.Condition.KnockedOut:
                    // スプライト非表示
                    ChangeColor(
                        GameSettings.VisualEffects.DefeatedEnemyColor,
                        GameSettings.VisualEffects.EnemyFadeDuration);
                    break;
            }
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
        private void ChangeColor(Color color, float duration)
        {
            targetedColor = spriteRenderer.DOColor(color, duration)
                .SetEase(Ease.Linear);
        }

        /// <summary>
        /// 浄化されたときの処理
        /// </summary>
        public void OnPurified()
        {
            // スプライト非表示
            spriteRenderer.enabled = false;
        }

        /// <summary>
        /// 対象フラグが設定されたときの処理
        /// </summary>
        /// <param name="targeted">対象になった</param>
        public void OnTargetFlagSet(bool targeted)
        {
            if (targeted)
            {
                StartTargetedColorTween();
            }
            else
            {
                KillTargetedColorTween();
            }
        }

        /// <summary>
        /// 対象に選択されたときの色変化Tweenを開始する
        /// </summary>
        private void StartTargetedColorTween()
        {
            Color color = GameSettings.VisualEffects.TargetedEnemyColor;
            float duration = GameSettings.VisualEffects.TargetedEnemyColorChangeDuration;

            targetedColor = spriteRenderer.DOColor(color, duration)
                .SetEase(Ease.OutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        /// <summary>
        /// 対象に選択されたときの色変化Tweenを終了する
        /// </summary>
        public void KillTargetedColorTween()
        {
            // Tweenを殺す
            if (targetedColor != null)
            {
                targetedColor.Kill();
            }

            // 元の色に戻す
            if (spriteRenderer)
            {
                spriteRenderer.color = defaultColor;
            }
        }
    }
}
