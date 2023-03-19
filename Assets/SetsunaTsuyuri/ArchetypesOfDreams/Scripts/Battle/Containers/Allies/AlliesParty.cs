using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 味方パーティ
    /// </summary>
    public class AlliesParty : CombatantsParty<AllyContainer>
    {
        /// <summary>
        /// 味方UIの管理者
        /// </summary>
        [field: SerializeField]
        public AlliesUI AlliesUI { get; private set; } = null;

        /// <summary>
        /// 控えのメンバー
        /// </summary>
        [field: SerializeField]
        public AllyContainer[] ReserveMembers { get; private set; } = null;

        /// <summary>
        /// 浄化した敵
        /// </summary>
        public CombatantContainer PurifiedEnemy { get; private set; } = null;

        /// <summary>
        /// 解放する戦闘者コンテナ
        /// </summary>
        public CombatantContainer ToBeReleased { get; set; } = null;

        /// <summary>
        /// メニュー画面での最初の使用者
        /// </summary>
        /// <returns></returns>
        public CombatantContainer MenuUser => Members[0];

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            foreach (var reserve in ReserveMembers)
            {
                reserve.OnBattleStartReserve();
            }
        }

        protected override IEnumerable<CombatantContainer> GetContainers(TargetPosition position)
        {
            if (position == TargetPosition.Reserves)
            {
                return ReserveMembers;
            }

            return Members;
        }

        /// <summary>
        /// 戦闘終了時の処理
        /// </summary>
        public void OnBattleEnd()
        {
            Combatant[] combatants = GetAllCombatants();
            foreach (var combatant in combatants)
            {
                combatant.OnBattleEnd();
            }
        }

        /// <summary>
        /// 勝利時の処理
        /// </summary>
        /// <param name="experience">経験値</param>
        public void OnWin(int experience)
        {
            Combatant[] combatants = GetAllCombatants();
            foreach (var combatant in combatants)
            {
                combatant.OnWin(experience);
            }
        }

        public override bool CanFight()
        {
            // 戦闘可能な夢渡りが存在する
            return Members
                .Where(x => x.ContainsFightable)
                .Any(x => x.Combatant is DreamWalker);
        }

        /// <summary>
        /// 保存された戦闘者配列をコンテナに移す
        /// </summary>
        public void TransferCombatantsViriableDataToContainers()
        {
            // コンテナ初期化
            var containers = GetAllContainers();
            foreach (var container in containers)
            {
                container.Initialize();
            }

            // ロード
            AddCombatants(Members, VariableData.Allies);
            AddCombatants(ReserveMembers, VariableData.ReserveAllies);
        }

        /// <summary>
        /// 戦闘者をコンテナに入れる
        /// </summary>
        /// <param name="containers">戦闘者コンテナ配列</param>
        /// <param name="combatants">戦闘者配列</param>
        public void AddCombatants(CombatantContainer[] containers, List<Combatant> combatants)
        {
            int min = Math.Min(containers.Length, combatants.Count);
            for (int i = 0; i < min; i++)
            {
                containers[i].Combatant = combatants[i];
            }
        }

        private void OnDestroy()
        {
            var allies = Members
                .Where(x => x.ContainsCombatant)
                .Select(x => x.Combatant);

            VariableData.SetAllies(allies);

            var reserveAllies = ReserveMembers
                .Where(x => x.ContainsCombatant)
                .Select(x => x.Combatant);

            VariableData.SetReserveAllies(reserveAllies);
        }

        public override AllyContainer FindAvailable()
        {
            AllyContainer result = base.FindAvailable();

            if (!result)
            {
                result = ReserveMembers.FirstOrDefault(x => x.IsAvailable());
            }

            return result;
        }

        public override IEnumerable<CombatantContainer> GetAllContainers()
        {
            var result = base.GetAllContainers()
                .Concat(ReserveMembers);

            return result;
        }

        /// <summary>
        /// 浄化した敵コンテナを含む全ての味方を取得する
        /// </summary>
        /// <returns></returns>
        public CombatantContainer[] GetPurifiedEnemyAndAllMembers()
        {
            List<CombatantContainer> result = new List<CombatantContainer>
            {
                PurifiedEnemy
            };
            result.AddRange(Members);
            result.AddRange(ReserveMembers);

            return result.ToArray();
        }

        /// <summary>
        /// 浄化した敵を仲間に加える
        /// </summary>
        /// <param name="purified">敵コンテナ</param>
        /// <param name="releaseButtons">解放ボタンの管理者</param>
        /// <param name="token">トークン</param>
        /// <returns></returns>
        public async UniTask AddPurifiedEnemy(CombatantContainer purified, ReleaseButtonsManager releaseButtons, CancellationToken token)
        {
            // 仲間に加える
            if (!TryInjectPurified(purified))
            {
                // 仲間に加わることができなかった場合
                // 誰を解放するか決める
                PurifiedEnemy = purified;
                releaseButtons.UpdateButtonsAndSelect(this);
                await releaseButtons.WaitForAnyButtonPressed(token);
                releaseButtons.Hide();

                // 選択したコンテナに格納されている戦闘者を解放する
                ToBeReleased.Release();

                // 浄化された敵コンテナに戦闘者が格納されているなら、
                // それ以外のいずれかのコンテナが解放され、空きができたと考えられる
                // そこでもう一度仲間に加える
                if (purified.ContainsCombatant)
                {
                    TryInjectPurified(purified);
                }
            }
        }

        /// <summary>
        /// 歩いたときの処理
        /// </summary>
        public void OnWalk()
        {
            VariableData.Steps++;

            Combatant[] combatants = GetAllCombatants();
            foreach (var combatant in combatants)
            {
                combatant.OnWalk();
            }
        }
    }
}
