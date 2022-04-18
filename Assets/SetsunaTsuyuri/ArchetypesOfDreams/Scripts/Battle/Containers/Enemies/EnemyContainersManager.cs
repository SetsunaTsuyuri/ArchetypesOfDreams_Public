using System.Collections;
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
        float enemyDistance = 3.0f;

        public override bool CanFight()
        {
            // 戦闘可能な夢渡りが存在する
            return Members
                .Any(x => x.ContainsFightable());
        }

        /// <summary>
        /// 敵を作る
        /// </summary>
        /// <param name="command">ゲームコマンド</param>
        public void CreateEnemies(GameCommand command)
        {
            // ダンジョンのデータ
            DungeonData dungeon = RuntimeData.DungeonToPlay;
            
            // セクションのデータ
            DungeonSectionData section = dungeon.DungeonSections[command.CurrentPlayerSection];

            // 敵グループID
            int id = command.EnemyGroup switch
            {
                Attribute.EnemyGroup.Random => Random.Range(section.MinRandomGroupId, section.MaxRandomGroupId),
                _ => command.Id
            };

            // 敵グループ
            EnemyGroupData enemyGroup = MasterData.EnemyGroups.GetValue(id);

            if (enemyGroup.Enemies.Length > Members.Length)
            {
                Debug.LogError("敵グループデータの敵の数がコンテナの数よりも大きいです");
                return;
            }

            for (int i = 0; i < enemyGroup.Enemies.Length; i++)
            {
                // 敵データを取得する
                EnemyData enemyData = enemyGroup.Enemies[i];

                // レベル計算
                int level = dungeon.BasicEnemyLevel;
                level += command.LevelCorrection;
                level += enemyData.LevelCorrection;
                level = Mathf.Clamp(level, GameSettings.Combatants.MinLevel, GameSettings.Combatants.MaxLevel);

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

            // それらが存在しない場合、処理の必要なし
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
                    -enemyDistance,
                    enemyDistance,
                    offset * (i + 1));

                // 座標を変更する
                container.AdjustPosition(x);
            }
        }
    }
}
