﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵コンテナの管理者
    /// </summary>
    public class EnemyContainersManager : CombatantContainersManager<EnemyContainer>
    {
        /// <summary>
        /// 敵が複数存在する場合、この数値が大きい程左右に広がる
        /// </summary>
        [SerializeField]
        float _enemyDistance = 1.0f;

        public override bool CanFight()
        {
            // 戦闘可能なコンテナが存在する
            return Members
                .Any(x => x.ContainsFightable());
        }

        /// <summary>
        /// ランダムエンカウントで出現する敵を作る
        /// </summary>
        /// <param name="map">マップ</param>
        /// <param name="allies">味方</param>
        public void CreateEnemies(Map map, AllyContainersManager allies)
        {
            // 敵の数
            int numberOfEnemies = allies.CountCombatants();

            // レベル
            int level = map.BasicEnemyLevel;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                // ID
                int id = Random.Range(map.MinEnemyId, map.MaxEnemyId + 1);
                Debug.Log($"Selected enemy id is {id} min {map.MinEnemyId} max {map.MaxEnemyId}");
                Members[i].CreateNightmare(id, level);
            }
        }

        /// <summary>
        /// イベントで出現する敵を作る
        /// </summary>
        /// <param name="battleEvent">バトルイベント</param>
        public void CreateEnemies(BattleEvent battleEvent)
        {
            int id = battleEvent.EnemyGroupId;
            EnemyGroupData enemyGroupData = MasterData.GetEnemyGroupData(id);

            // 敵を作る
            CreateEnemies(enemyGroupData);
        }

        /// <summary>
        /// 敵を作る
        /// </summary>
        /// <param name="enemyGroupData">敵グループデータ</param>
        private void CreateEnemies(EnemyGroupData enemyGroupData)
        {
            if (enemyGroupData.Enemies.Length > Members.Length)
            {
                Debug.LogError("敵グループデータの敵の数がコンテナの数よりも大きいです");
                return;
            }

            for (int i = 0; i < enemyGroupData.Enemies.Length; i++)
            {
                // 敵データを取得する
                EnemyData enemyData = enemyGroupData.Enemies[i];

                // レベル
                int level = enemyData.Level;
                int min = GameSettings.Combatants.MinLevel;
                int max = GameSettings.Combatants.MaxLevel;
                level = Mathf.Clamp(level, min, max);

                // 敵ナイトメアを生成する
                Members[i].CreateEnemyNightmare(enemyData, level);
            }
        }

        /// <summary>
        /// 戦闘者を格納しているコンテナを全て取得する
        /// </summary>
        /// <returns></returns>
        public EnemyContainer[] GetContainersWithContents()
        {
            return Members.
                Where(x => x.ContainsCombatant()).
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
