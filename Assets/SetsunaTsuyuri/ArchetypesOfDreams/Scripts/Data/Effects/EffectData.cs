using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 効果のデータ
    /// </summary>
    [System.Serializable]
    public class EffectData
    {
        /// <summary>
        /// どの立場の者の中から
        /// </summary>
        [field: SerializeField]
        public Effect.TargetPosition TargetPosition { get; private set; }

        /// <summary>
        /// どのように選ぶか
        /// </summary>
        [field: SerializeField]
        public Effect.TargetSelection TargetSelection { get; private set; }

        /// <summary>
        /// ステータスへの影響
        /// </summary>
        [field: SerializeField]
        public DataOfEffectOnStatus[] EffectsOnStatus { get; private set; }

        /// <summary>
        /// 実行回数
        /// </summary>
        [field: SerializeField]
        public int Executions { get; private set; }

        /// <summary>
        /// 浄化スキルである
        /// </summary>
        [field: SerializeField]
        public bool IsPurification { get; private set; }

        /// <summary>
        /// 交代スキルである
        /// </summary>
        [field: SerializeField]
        public bool IsChange { get; private set; }

        /// <summary>
        /// 防御スキルである
        /// </summary>
        [field: SerializeField]
        public bool IsDefending { get; private set; }

        /// <summary>
        /// 攻撃的なスキルである
        /// </summary>
        [field: SerializeField]
        public bool IsOffensive { get; private set; }

        /// <summary>
        /// 攻撃の属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Attack Attack { get; private set; }

        /// <summary>
        /// 感情の属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Emotion Emotion { get; private set; }

        /// <summary>
        /// 弱点を突いたとき、精神力の追加ダメージ
        /// </summary>
        [field: SerializeField]
        public int SoulDamage { get; private set; } = 1;

        /// <summary>
        /// 命中率
        /// </summary>
        [field: SerializeField]
        public int Hit { get; private set; } = 100;

        /// <summary>
        /// クリティカル率
        /// </summary>
        [field: SerializeField]
        public int Critical { get; private set; } = 0;

        /// <summary>
        /// ステータスに影響する
        /// </summary>
        /// <returns></returns>
        public bool AffectStatus()
        {
            return EffectsOnStatus.Length > 0;
        }
    }
}
