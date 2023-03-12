using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ダメージ・回復の結果
    /// </summary>
    public abstract class DamageOrHealingResult : ActionResult
    {
        /// <summary>
        /// HP
        /// </summary>
        public int? HP { get; protected set; } = null;

        /// <summary>
        /// DP
        /// </summary>
        public int? DP { get; protected set; } = null;

        /// <summary>
        /// GP
        /// </summary>
        public int? GP { get; protected set; } = null;

        public override void Initialize()
        {
            HP = null;
            DP = null;
            GP = null;
        }

        public override bool IsValid
        {
            get => HP.HasValue || DP.HasValue || GP.HasValue;
        }

        /// <summary>
        /// 1以上の変化がある
        /// </summary>
        public bool IsOneOrOver
        {
            get => HP > 0 || DP > 0 || GP > 0;
        }

        /// <summary>
        /// HPを加算する
        /// </summary>
        /// <param name="hp"></param>
        public void AddHP(int hp)
        {
            HP ??= 0;
            HP = Clamp(HP.Value + hp);
        }

        /// <summary>
        /// DPを加算する
        /// </summary>
        /// <param name="dp"></param>
        public void AddDP(int dp)
        {
            DP ??= 0;
            DP = Clamp(DP.Value + dp);
        }

        /// <summary>
        /// GPを加算する
        /// </summary>
        /// <param name="gp"></param>
        public void AddGP(int gp)
        {
            GP ??= 0;
            GP = Clamp(GP.Value + gp);
        }

        /// <summary>
        /// クランプする
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int Clamp(int value)
        {
            return Mathf.Clamp(value, 0, GameSettings.Combatants.MaxDamageAndHealing);
        }
    }
}
