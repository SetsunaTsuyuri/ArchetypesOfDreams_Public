using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 対象に対する行動の結果
    /// </summary>
    public class ActionResultSet : IInitializable
    {
        /// <summary>
        /// 影響を与えてきた者
        /// </summary>
        public Combatant Affecter { get; set; } = null;

        /// <summary>
        /// 有効性
        /// </summary>
        public GameAttribute.Effectiveness Effectiveness { get; set; } = GameAttribute.Effectiveness.Normal;

        /// <summary>
        /// 浄化成功率
        /// </summary>
        public int PurificationSuccessRate { get; set; } = 0;

        /// <summary>
        /// 失敗
        /// </summary>
        public bool Miss { get; set; } = false;

        /// <summary>
        /// ダメージ
        /// </summary>
        public DamageResult Damage { get; private set; } = new();

        /// <summary>
        /// 回復
        /// </summary>
        public HealingResult Healing { get; private set; } = new();

        /// <summary>
        /// 追加ステータス効果リスト
        /// </summary>
        public readonly List<EffectData.StatusEffect> AddedStatusEffects = new();

        /// <summary>
        /// 削除ステータス効果リスト
        /// </summary>
        public readonly List<StatusEffect> RemovedStatusEffects = new();

        /// <summary>
        /// 浄化された
        /// </summary>
        public bool? Purified { get; set; } = false;

        /// <summary>
        /// 交代の対象にされた
        /// </summary>
        public bool IsChangeTarget { get; set; } = false;

        public void Initialize()
        {
            Affecter = null;

            Effectiveness = GameAttribute.Effectiveness.Normal;
            PurificationSuccessRate = 0;

            Miss = false;

            Purified = null;
            IsChangeTarget = false;

            Damage.Initialize();
            Healing.Initialize();

            AddedStatusEffects.Clear();
            RemovedStatusEffects.Clear();
        }
    }
}
