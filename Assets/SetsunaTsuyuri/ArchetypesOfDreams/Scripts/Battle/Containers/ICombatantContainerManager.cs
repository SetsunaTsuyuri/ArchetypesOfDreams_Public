using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者コンテナの管理者
    /// </summary>
    public interface ICombatantContainerManager
    {
        /// <summary>
        /// 全ての空ではないコンテナを取得する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CombatantContainer> GetNonEmptyContainers();

        /// <summary>
        /// 全ての行動可能なコンテナを取得する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CombatantContainer> GetActionables();

        /// <summary>
        /// 対象にできるコンテナが存在する
        /// </summary>
        /// <param name="user">効果の使用者コンテナ</param>
        /// <param name="effect">効果データ</param>
        /// <returns></returns>
        public bool ContainsTargetables(CombatantContainer user, EffectData effect);

        /// <summary>
        /// 対象にできるコンテナを取得する
        /// </summary>
        /// <param name="user">効果の使用者コンテナ</param>
        /// <param name="effect">効果データ</param>
        /// <returns></returns>
        public IEnumerable<CombatantContainer> GetTargetables(CombatantContainer user, EffectData effect);
    }
}
