using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵パーティ
    /// </summary>
    public class EnemiesParty : CombatantsParty<EnemyContainer>
    {
        /// <summary>
        /// ランダムエンカウントで出現する敵の数のばらつき
        /// </summary>
        [SerializeField]
        int _randomBattleEnemies = 1;

        /// <summary>
        /// 敵が複数存在する場合、この数値が大きい程左右に広がる
        /// </summary>
        [SerializeField]
        float _enemyDistance = 1.0f;

        public override bool CanFight()
        {
            // 戦闘可能なコンテナが存在する
            return Members.Any(x => x.ContainsFightable);
        }

        protected override IEnumerable<CombatantContainer> GetReserveMembers()
        {
            return Enumerable.Empty<CombatantContainer>();
        }

        /// <summary>
        /// 初期化し敵を作る
        /// </summary>
        /// <param name="enemyGroupId"></param>
        public void InitializeAndCreateEnemies(int enemyGroupId)
        {
            Initialize();

            EnemyGroupData enemyGroupData = MasterData.GetEnemyGroupData(enemyGroupId);

            if (enemyGroupData.Enemies.Length > Members.Length)
            {
                Debug.LogError("敵グループデータの敵の数がコンテナの数よりも大きいです");
                return;
            }

            for (int i = 0; i < enemyGroupData.Enemies.Length; i++)
            {
                // 敵データを取得する
                EnemyData enemyData = enemyGroupData.Enemies[i];

                // 敵ナイトメアを生成する
                Members[i].CreateNightmare(enemyData);
            }
        }

        /// <summary>
        /// 初期化し敵を作る
        /// </summary>
        /// <param name="dungeon"></param>
        /// <param name="allies"></param>
        public void InitializeAndCreateEnemies(Dungeon dungeon, AlliesParty allies)
        {
            Initialize();

            // 敵データ配列
            RandomEncounterEnemyData[] enemies = dungeon.GetRandomEncounterEnemies();

            // 敵の出現数
            int allyNumber = allies.CountCombatants();
            int enemyNumber = DecideEnemyNumber(allyNumber);

            float[] enemyWeigths = enemies
                .Select(x => x.Ratio)
                .ToArray();

            int[] enemyIndexes = RandomUtility.Weighted(enemyWeigths, enemyNumber);
            for (int i = 0; i < enemyNumber; i++)
            {
                int index = enemyIndexes[i];
                RandomEncounterEnemyData enemy = enemies[index];
                Members[i].CreateNightmare(enemy.EnemyId, enemy.Level);
            }
        }

        /// <summary>
        /// 敵の出現数を決定する
        /// </summary>
        /// <param name="allyNumber"></param>
        /// <returns></returns>
        private int DecideEnemyNumber(int allyNumber)
        {
            int random = Random.Range(-_randomBattleEnemies, _randomBattleEnemies + 1);
            int result = Mathf.Clamp(allyNumber + random, 1, Members.Length);
            return result;
        }

        /// <summary>
        /// 戦闘者を格納しているコンテナを全て取得する
        /// </summary>
        /// <returns></returns>
        public EnemyContainer[] GetContainersWithContents()
        {
            return Members.
                Where(x => x.ContainsCombatant).
                ToArray();
        }

        /// <summary>
        /// 敵の位置を調整する
        /// </summary>
        public void AdjsutEnemiesPosition()
        {
            // 戦闘者を格納している敵コンテナ配列
            EnemyContainer[] containers = GetContainersWithContents();
            if (!containers.Any())
            {
                return;
            }

            // 敵の位置オフセット
            // 左端から右端にかけて、敵1体毎に同じ間隔を空ける
            float offset = 1.0f / (containers.Length + 1);
            for (int i = 0; i < containers.Length; i++)
            {
                // 敵コンテナ
                EnemyContainer container = containers[i];

                // X座標
                float x = Mathf.Lerp(
                    -_enemyDistance,
                    _enemyDistance,
                    offset * (i + 1));

                // 座標を変更する
                container.AdjustPosition(x);
            }
        }
    }
}
