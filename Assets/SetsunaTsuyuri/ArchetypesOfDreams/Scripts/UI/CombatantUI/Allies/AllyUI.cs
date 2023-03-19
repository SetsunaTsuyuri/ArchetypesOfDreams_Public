using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 味方のUI
    /// </summary>
    public class AllyUI : CombatantUI
    {
        /// <summary>
        /// Imageの色を変えるもの
        /// </summary>
        public ImageColorChanger ColorChanger { get; private set; } = null;

        protected override void Awake()
        {
            base.Awake();

            ColorChanger = GetComponentInChildren<ImageColorChanger>(true);
        }

        /// <summary>
        /// ダメージを受けたときの処理
        /// </summary>
        /// <param name="_"></param>
        public void OnDamage(CombatantContainer _)
        {
            Vector3 punch = GameSettings.Allies.DamagePunch;
            float duration = GameSettings.Allies.DamagePunchDuration;
            int vibrato = GameSettings.Allies.DamagePunchVibrato;
            Punch(punch, duration, vibrato);
        }
    }
}
