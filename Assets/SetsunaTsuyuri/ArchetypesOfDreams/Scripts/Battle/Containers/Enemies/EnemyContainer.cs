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
        /// 浄化されたときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithCombatantContainer onPurified = null;

        /// <summary>
        /// 倒されたときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithCombatantContainer onKnockedOut = null;

        /// <summary>
        /// 座標が設定されたときのゲームイベント
        /// </summary>
        [SerializeField]
        GameEventWithCombatantContainer onPositionSet = null;

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

        public override void OnConditionSet()
        {
            base.OnConditionSet();

            // 敵スプライトの健康状態設定時の処理
            EnemySprite.OnConditionSet(Combatant);
        }

        public override void OnAction(ActionInfo model)
        {
            base.OnAction(model);

            EnemySprite.Blink();
        }

        public override void OnDamage()
        {
            base.OnDamage();

            EnemySprite.Shake();
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

            // 浄化されたときのゲームイベント
            if (onPurified)
            {
                onPurified.Invoke(this);
            }

            if (Battle.InstanceInActiveScene)
            {
                Battle battle = Battle.InstanceInActiveScene;
                battle.OnEnemyKnockedOutOrPurified(this);
                await battle.Allies.AddPurifiedEnemy(this, battle.BattleUI.AlliesUI.ReleaseButtons, token);
            }
        }

        /// <summary>
        /// 倒されたときの処理
        /// </summary>
        /// <returns></returns>
        public void OnKnockedOut()
        {
            // 倒されたときのゲームイベント
            onKnockedOut.Invoke(this);

            // 効果音
            AudioManager.PlaySE(SEId.Collapse);
        }

        /// <summary>
        /// ナイトメアを作る
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="level">レベル</param>
        public void CreateNightmare(int id, int level)
        {
            NightmareData nightmareData = MasterData.GetNightmareData(id);
            if (nightmareData is null)
            {
                return;
            }

            Nightmare nightmare = new()
            {
                DataId = id,
                Level = level
            };

            Combatant = nightmare;

            nightmare.Initialize();
            EnemySprite.UpdateSprite(Combatant);
        }

        /// <summary>
        /// 敵のナイトメアを作る
        /// </summary>
        /// <param name="enemyData">敵データ</param>
        /// <param name="level">レベル</param>
        public void CreateEnemyNightmare(EnemyData enemyData, int level)
        {
            NightmareData nightmareData = MasterData.GetNightmareData(enemyData.Id);
            if (nightmareData is null)
            {
                return;
            }

            Nightmare nightmare = new()
            {
                DataId = nightmareData.Id,
                Level = level,
                IsLeader = enemyData.IsLeader,
                HasBossResistance = enemyData.HasBossResistance
            };

            Combatant = nightmare;

            nightmare.Initialize();
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

            // イベント呼び出し
            if (onPositionSet)
            {
                onPositionSet.Invoke(this);
            }
        }
    }
}
