using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵を格納するコンテナ
    /// </summary>
    public class EnemyContainer : CombatantContainer
    {
        public override Combatant Combatant
        {
            get => base.Combatant;
            set
            {
                base.Combatant = value;
                EnemySprite.OnComabtantSet(value);
            }
        }

        public override bool IsTargeted
        {
            get => base.IsTargeted;
            set
            {
                base.IsTargeted = value;
                EnemySprite.OnTargetFlagSet(value);
            }
        }

        /// <summary>
        /// 敵スプライト
        /// </summary>
        public EnemySprite EnemySprite { get; private set; } = null;

        private void Awake()
        {
            EnemySprite = GetComponentInChildren<EnemySprite>();
        }

        public override void Initialize()
        {
            base.Initialize();

            EnemySprite.Initialize();
        }

        public override void OnDamage()
        {
            base.OnDamage();

            AudioManager.PlaySE(SEId.EnemyDamage);
            EnemySprite.Shake();
        }

        public override void OnKnockedOut()
        {
            base.OnKnockedOut();

            AudioManager.PlaySE(SEId.Collapse);
            EnemySprite.Collapse();

            if (Battle.IsRunning && Battle.InstanceInActiveScene)
            {
                Battle.InstanceInActiveScene.OnEnemyKnockedOutOrPurified(this);
            }
        }

        public override void OnActionExecution(ActionInfo action)
        {
            base.OnActionExecution(action);

            AudioManager.PlaySE(SEId.EnemyAction);
            EnemySprite.Blink();
        }

        public override void Escape()
        {
            base.Escape();

            Release();
        }

        /// <summary>
        /// 浄化されたときの処理
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask OnPurified(CancellationToken token)
        {
            // 敵スプライトの浄化時の処理
            EnemySprite.OnPurified();

            if (Battle.InstanceInActiveScene)
            {
                Battle battle = Battle.InstanceInActiveScene;
                battle.OnEnemyKnockedOutOrPurified(this);
                await battle.Allies.AddPurifiedEnemy(this, battle.BattleUI.AlliesUI.ReleaseButtons, token);
            }
        }

        /// <summary>
        /// ナイトメアを作る
        /// </summary>
        /// <param name="enemyData">敵データ</param>
        /// <param name="level">レベル</param>
        public void CreateNightmare(EnemyData enemyData)
        {
            CreateNightmare(
                enemyData.EnemyId,
                enemyData.Level,
                enemyData.IsLeader,
                enemyData.HasBossResistance);
        }

        /// <summary>
        /// ナイトメアを作る
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="level"></param>
        /// <param name="isLeader"></param>
        /// <param name="hasBossResistance"></param>
        public void CreateNightmare(int dataId, int level, bool isLeader = false, bool hasBossResistance = false)
        {
            Combatant = new Nightmare()
            {
                DataId = dataId,
                Level = level,
                IsLeader = isLeader,
                HasBossResistance = hasBossResistance
            };

            Combatant.Initialize();
            EnemySprite.UpdateSprite(Combatant);
        }

        /// <summary>
        /// 位置を調整する
        /// </summary>
        /// <param name="x">新しいX座標</param>
        public void AdjustPosition(float x)
        {
            // 座標設定
            transform.ChangeLocalPositionX(x);

            // 通知
            MessageBrokersManager.EnemyPositionSet.Publish(this);
        }
    }
}
