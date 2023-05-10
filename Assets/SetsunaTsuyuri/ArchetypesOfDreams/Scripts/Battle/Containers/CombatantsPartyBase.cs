using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者パーティの基底クラス
    /// </summary>
    public abstract class CombatantsPartyBase : MonoBehaviour
    {
        /// <summary>
        /// メンバーの誰かがスキルを持っている
        /// </summary>
        public bool HasAnySkill
        {
            get => GetNonEmptyContainers().Any(x => x.HasAnySkill);
        }

        /// <summary>
        /// 全てのコンテナを取得する
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<CombatantContainer> GetAllContainers();

        /// <summary>
        /// 全ての空ではないコンテナを取得する
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<CombatantContainer> GetNonEmptyContainers();

        /// <summary>
        /// 対象にできるコンテナが存在する
        /// </summary>
        /// <param name="user">効果の使用者コンテナ</param>
        /// <param name="effect">効果データ</param>
        /// <returns></returns>
        public abstract bool ContainsTargetables(CombatantContainer user, EffectData effect);

        /// <summary>
        /// 対象にできるコンテナを取得する
        /// </summary>
        /// <param name="user">効果の使用者コンテナ</param>
        /// <param name="effect">効果データ</param>
        /// <returns></returns>
        public abstract IEnumerable<CombatantContainer> GetTargetables(CombatantContainer user, EffectData effect);
    }
}
