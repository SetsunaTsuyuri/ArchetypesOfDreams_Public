using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者コンテナの管理者
    /// </summary>
    /// <typeparam name="TContainer">戦闘者コンテナの型</typeparam>
    public abstract class CombatantContainersManager<TContainer> : MonoBehaviour, IInitializable, ICombatantContainerManager
        where TContainer : CombatantContainer
    {
        /// <summary>
        /// メンバー
        /// </summary>
        [field: SerializeField]
        public TContainer[] Members { get; private set; } = null;

        private void Awake()
        {
            // IDを割り振る
            CombatantContainer[] containers = GetAllContainers().ToArray();
            for (int i = 0; i < containers.Length; i++)
            {
                containers[i].Id = i;
            }
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="enemies">敵</param>
        public void SetUp(ICombatantContainerManager enemies)
        {
            var containers = GetAllContainers();
            foreach (var container in containers)
            {
                container.SetUp(this, enemies);
            }
        }

        public virtual void Initialize()
        {
            var containers = GetAllContainers();
            foreach (var container in containers)
            {
                container.Initialize();
            }
        }

        /// <summary>
        /// 戦闘開始時の処理
        /// </summary>
        public virtual void OnBattleStart()
        {
            foreach (var member in Members)
            {
                member.OnBattleStart();
            }
        }

        /// <summary>
        /// 戦闘不能になっている解放可能な戦闘者を解放する
        /// </summary>
        public void ReleaseKnockedOutReleasables()
        {
            CombatantContainer[] containers = GetAllContainers()
                .Where(x => x.ContainsCombatant() && x.Combatant.IsKnockedOut())
                .Where(x => x.ContainsReleasable())
                .ToArray();

            foreach (var container in containers)
            {
                container.Release();
            }
        }

        /// <summary>
        /// 戦闘者コンテナの中身を空いているコンテナに入れる
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        /// <returns>入れられたらtrueを返す</returns>
        public bool TryInjectingCombatant(CombatantContainer container)
        {
            bool result = false;

            // 利用可能なコンテナが見つかった場合
            CombatantContainer available = FindAvailable();
            if (available)
            {
                // そのコンテナに戦闘者を加える
                container.InjectCombatantInto(available);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// 利用可能なコンテナを探す
        /// </summary>
        /// <returns></returns>
        public virtual TContainer FindAvailable()
        {
            return Members.FirstOrDefault(x => x.IsAvailable());
        }

        /// <summary>
        /// 全てのコンテナを取得する
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TContainer> GetAllContainers()
        {
            return Members;
        }

        /// <summary>
        /// 全ての戦闘者を取得する
        /// </summary>
        /// <returns></returns>
        public Combatant[] GetAllCombatants()
        {
            Combatant[] result = GetAllContainers()
                .Where(x => x.ContainsCombatant())
                .Select(x => x.Combatant)
                .ToArray();

            return result;
        }

        /// <summary>
        /// 戦闘者の数を数える
        /// </summary>
        /// <returns></returns>
        public int CountCombatants()
        {
            return Members
                .Count(x => x.ContainsCombatant());
        }

        /// <summary>
        /// 戦闘可能なコンテナを取得する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CombatantContainer> GetFightables()
        {
            return Members.Where(x => x.ContainsFightable());
        }

        public IEnumerable<CombatantContainer> GetNonEmptyContainers()
        {
            return GetAllContainers().Where(x => x.ContainsCombatant());
        }

        public IEnumerable<CombatantContainer> GetActionables()
        {
            return GetAllContainers().Where(x => x.ContainsActionable());
        }

        public bool ContainsTargetables(CombatantContainer user, EffectData effect)
        {
            return GetContainers(effect.TargetPosition)
                .Any(x => (effect.CanTargetUser || x != user) && x.ContainsTargetable(effect.TargetCondition));
        }

        public IEnumerable<CombatantContainer> GetTargetables(CombatantContainer user, EffectData effect)
        {
            return GetContainers(effect.TargetPosition)
                .Where(x => (effect.CanTargetUser || x != user) && x.ContainsTargetable(effect.TargetCondition));
        }

        /// <summary>
        /// コンテナを取得する
        /// </summary>
        /// <param name="position">対象にできるコンテナの立場</param>
        /// <returns></returns>
        protected abstract IEnumerable<CombatantContainer> GetContainers(TargetPosition position);

        /// <summary>
        /// 行動対象にできるコンテナを取得する
        /// </summary>
        /// <param name="condition">対象にできる戦闘者の状態</param>
        /// <returns></returns>
        public CombatantContainer[] GetTargetables(TargetCondition condition)
        {
            return Members
                .Where(x => x.ContainsTargetable(condition))
                .ToArray();
        }

        /// <summary>
        /// 全ての戦闘者のステータスを初期化する
        /// </summary>
        public void InitializeCombatantsStatus()
        {
            Combatant[] combatants = GetAllCombatants();

            foreach (var combatant in combatants)
            {
                combatant.InitializeStatus();
            }
        }

        /// <summary>
        /// 戦闘可能である
        /// </summary>
        /// <returns></returns>
        public abstract bool CanFight();
    }
}
