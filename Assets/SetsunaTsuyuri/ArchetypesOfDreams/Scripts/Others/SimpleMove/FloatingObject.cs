using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 浮かぶもの
    /// </summary>
    public class FloatingObject : MonoBehaviour
    {
        [SerializeField]
        float move = 1.0f;

        [SerializeField]
        float duration = 1.0f;

        [SerializeField]
        Ease ease = Ease.Unset;

        private void Start()
        {
            Sequence sequence = DOTween.Sequence()
                .Append(transform.DOMoveY(move, duration).SetEase(ease).SetRelative())
                .Append(transform.DOMoveY(-move, duration).SetEase(ease).SetRelative())
                .SetLoops(-1)
                .SetLink(gameObject);
        }
    }
}
