using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘結果の種類
    /// </summary>
    public enum BattleResultType
    {
        /// <summary>
        /// 勝利
        /// </summary>
        Win = 0,

        /// <summary>
        /// 敗北
        /// </summary>
        Lose = 1,

        /// <summary>
        /// 中止
        /// </summary>
        Canceled = 2
    }

    /// <summary>
    /// 戦闘
    /// </summary>
    public partial class Battle : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        public static Battle InstanceInActiveScene { get; private set; } = null;

        /// <summary>
        /// 戦闘中である
        /// </summary>
        /// <returns></returns>
        public static bool IsRunning
        {
            get
            {
                bool result = false;
                if (InstanceInActiveScene)
                {
                    result = InstanceInActiveScene.State.Current is not Sleep;
                }

                return result;
            }
        }

        /// <summary>
        /// 戦闘UI
        /// </summary>
        [field: SerializeField]
        public BattleUI BattleUI { get; private set; } = null;

        /// <summary>
        /// カメラコントローラー
        /// </summary>
        [SerializeField]
        CameraController _cameraController = null;

        /// <summary>
        /// カメラのトランスフォーム
        /// </summary>
        [SerializeField]
        Transform _cameraTransform = null;

        /// <summary>
        /// 味方
        /// </summary>
        [field: SerializeField]
        public AlliesParty Allies { get; private set; } = null;

        /// <summary>
        /// 敵
        /// </summary>
        public EnemiesParty Enemies { get; private set; } = null;

        /// <summary>
        /// 行動順
        /// </summary>
        public CombatantContainer[] OrderOfActions { get; private set; } = { };

        /// <summary>
        /// 行動者コンテナ
        /// </summary>
        public CombatantContainer Actor { get; private set; } = null;

        /// <summary>
        /// 得られる経験値
        /// </summary>
        public int RewardExperience { get; private set; } = 0;

        /// <summary>
        /// ステートマシン
        /// </summary>
        public StateMachine<Battle> State { get; private set; } = null;

        /// <summary>
        /// 逃走を禁じる
        /// </summary>
        public bool ForbidEscaping { get; private set; } = false;

        /// <summary>
        /// 戦闘が終わった
        /// </summary>
        public bool IsOver { get; private set; } = false;

        /// <summary>
        /// 戦闘結果
        /// </summary>
        BattleResultType _result = BattleResultType.Win;

        private void Awake()
        {
            Enemies = GetComponentInChildren<EnemiesParty>(true);

            SetUpStateMachine();

            InstanceInActiveScene = this;
        }

        /// <summary>
        /// ステートマシンの設定を行う
        /// </summary>
        private void SetUpStateMachine()
        {
            State = new StateMachine<Battle>(this);
            State.Add<Sleep>();
            State.Add<Preparation>();
            State.Add<BattleStart>();
            State.Add<TimeAdvancing>();
            State.Add<TurnStart>();
            State.Add<CommandSelection>();
            State.Add<ActionExecution>();
            State.Add<TurnEnd>();
            State.Add<BattleEnd>();
        }

        private void Start()
        {
            // 各UIをセットアップする
            BattleUI.SetUp(this);

            // スリープステートへ移行する
            State.Change<Sleep>();
        }

        private void Update()
        {
            State.Update();
        }

        public void Initialize()
        {
            // 戦闘終了フラグ
            IsOver = false;

            // 経験値
            RewardExperience = 0;

            // カメラの視点
            _cameraController.Target = _cameraTransform;
        }

        /// <summary>
        /// ランダムエンカウントによる戦闘を行う
        /// </summary>
        /// <param name="map"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<BattleResultType> ExecuteRandomBattle(Map map, CancellationToken token)
        {
            await FadeManager.FadeOut(token);

            Enemies.Initialize();
            Enemies.CreateEnemies(map, Allies);

            // 通常戦闘BGMを再生する
            AudioManager.PlayBgm(BgmId.NormalBattle);

            return await ExecuteBattle(token);
        }

        /// <summary>
        /// イベント戦闘を行う
        /// </summary>
        /// <param name="battleEvent"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<BattleResultType> ExecuteEventBattle(BattleEvent battleEvent, CancellationToken token)
        {
            await FadeManager.FadeOut(token);

            Enemies.Initialize();
            Enemies.CreateEnemies(battleEvent);

            // ボス戦以外の場合
            if (!battleEvent.IsBossBattle)
            {
                // 通常戦闘BGMを再生する
                AudioManager.PlayBgm(BgmId.NormalBattle);
            }

            return await ExecuteBattle(token);
        }

        /// <summary>
        /// 戦闘を行う
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask<BattleResultType> ExecuteBattle(CancellationToken token)
        {
            Initialize();

            // 準備状態へ移行する
            State.Change<Preparation>();

            // 戦闘終了まで待つ
            await UniTask.WaitUntil(() => IsOver, cancellationToken: token);

            return _result;
        }

        /// <summary>
        /// 戦闘続行可能である
        /// </summary>
        /// <returns></returns>
        private bool CanContinue()
        {
            return Allies.CanFight() && Enemies.CanFight();
        }

        /// <summary>
        /// 敵が倒された、または浄化されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnEnemyKnockedOutOrPurified(CombatantContainer container)
        {
            // 経験値増加
            RewardExperience += container.Combatant.GetRewardExperience();
        }

        /// <summary>
        /// 行動順を決定する
        /// </summary>
        private void UpdateOrderOfActions()
        {
            // 優先順位
            // 再行動可能者→待機時間(小)→素早さ(大)→味方→ID(小)
            OrderOfActions = Allies.GetFightables()
                .Concat(Enemies.GetFightables())
                .OrderByDescending(x => x == Actor)
                .ThenBy(x => x.Combatant.WaitTime)
                .ThenByDescending(x => x.Combatant.Speed)
                .ThenByDescending(x => x is AllyContainer)
                .ThenBy(x => x.Id)
                .ToArray();

            // UI更新
            BattleUI.OrderOfActions.UpdateDisplay(OrderOfActions);
        }
    }
}
