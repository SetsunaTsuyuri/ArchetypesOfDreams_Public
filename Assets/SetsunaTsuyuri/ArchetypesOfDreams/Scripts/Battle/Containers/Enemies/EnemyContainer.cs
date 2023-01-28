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
        /// <param name="battle">戦闘の管理者</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask OnPurified(Battle battle, CancellationToken token)
        {
            // 敵スプライトの浄化時の処理
            EnemySprite.OnPurified();

            // 浄化されたときのゲームイベント
            if (onPurified)
            {
                onPurified.Invoke(this);
            }

            // 浄化されたときの処理
            battle.OnEnemyKnockedOutOrPurified(this);

            // 仲間に加わる
            await battle.Allies.AddPurifiedEnemy(this, battle.BattleUI.AlliesUI.ReleaseButtons, token);
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
            AudioManager.PlaySE("敵消滅");
        }

        /// <summary>
        /// ナイトメアを作る
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="level">レベル</param>
        public void CreateNightmare(int id, int level)
        {
            NightmareData nightmareData = MasterData.GetNightmareData(id);
            if (nightmareData is not null)
            {
                Nightmare nightmare = new()
                {
                    DataId = id,
                    Level = level
                };

                nightmare.Initialize();

                Combatant = nightmare;
            }
        }

        /// <summary>
        /// 敵のナイトメアを作る
        /// </summary>
        /// <param name="enemyData">敵データ</param>
        /// <param name="level">レベル</param>
        public void CreateEnemyNightmare(EnemyData enemyData, int level)
        {
            // IDを元にデータを取得する
            NightmareData nightmareData = MasterData.GetNightmareData(enemyData.Id);

            // ナイトメアを作る
            Nightmare nightmare = new Nightmare()
            {
                DataId = nightmareData.Id,
                Level = level,
                IsLeader = enemyData.IsLeader,
                HasBossResistance = enemyData.HasBossResistance
            };

            // ナイトメアを初期化する
            nightmare.Initialize();

            // コンテナに作ったナイトメアを格納する
            Combatant = nightmare;
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
