using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘結果の種類
    /// </summary>
    public enum BattleResultType
    {
        None = 0,

        /// <summary>
        /// 勝利
        /// </summary>
        Win = 1,

        /// <summary>
        /// 敗北
        /// </summary>
        Lose = 2,

        /// <summary>
        /// 中止
        /// </summary>
        Canceled = 3
    }

    /// <summary>
    /// 戦闘
    /// </summary>
    public partial class Battle : MonoBehaviour, IInitializable
    {
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
                    result = InstanceInActiveScene._isRunning;
                }

                return result;
            }
        }

        /// <summary>
        /// 戦闘中である
        /// </summary>
        bool _isRunning = false;

        /// <summary>
        /// 味方が逃走する
        /// </summary>
        bool _alliesEscape = false;

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
        public CombatantContainer ActiveContainer { get; private set; } = null;

        /// <summary>
        /// 行動内容
        /// </summary>
        public ActionInfo ActiveContainerAction { get; set; } = null;

        /// <summary>
        /// 行動の対象
        /// </summary>
        public CombatantContainer[] ActiveContainerActionTargets { get; set; } = null;

        /// <summary>
        /// 得られる経験値
        /// </summary>
        public int RewardExperience { get; private set; } = 0;

        /// <summary>
        /// 得られる精気
        /// </summary>
        public int RewardSpirit { get; private set; } = 0;

        /// <summary>
        /// 味方が逃走可能である
        /// </summary>
        public bool AlliesCanEscape { get; private set; } = false;

        /// <summary>
        /// 戦闘が終わった
        /// </summary>
        public bool IsOver { get; private set; } = false;

        /// <summary>
        /// 戦闘続行可能である
        /// </summary>
        /// <returns></returns>
        private bool CanContinue
        {
            get
            {
                return !_alliesEscape
                    && Allies.CanFight()
                    && Enemies.CanFight();
            }
        }

        /// <summary>
        /// 行動者が行動できる
        /// </summary>
        private bool ActiveContainerCanAct
        {
            get
            {
                return !_alliesEscape
                    && ActiveContainer
                    && ActiveContainer.ContainsActionable;
            }
        }

        /// <summary>
        /// 戦闘結果
        /// </summary>
        BattleResultType _result = BattleResultType.Win;

        private void Awake()
        {
            Enemies = GetComponentInChildren<EnemiesParty>();
            InstanceInActiveScene = this;
        }

        private void Start()
        {
            // 各UIをセットアップする
            BattleUI.SetUp(this);

            // 味方逃走
            MessageBrokersManager.AlliesEscape.Receive<Empty>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(_ => _alliesEscape = true);
        }

        /// <summary>
        /// ランダムエンカウントによる戦闘を行う
        /// </summary>
        /// <param name="map"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<BattleResultType> ExecuteRandomEncounterBattle(Dungeon dungeon, CancellationToken token)
        {
            AudioManager.PlaySE(SEId.BattleStart);

            await FadeManager.FadeOut(token);

            AlliesCanEscape = true;

            Enemies.InitializeAndCreateEnemies(dungeon, Allies);

            AudioManager.PlayBgm(dungeon.Data.BattleBgmId);

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
            if (battleEvent.BgmId > 0)
            {
                AudioManager.SaveBgm();
                AudioManager.StopBgm(1.0f);
            }

            AudioManager.PlaySE(SEId.BattleStart);

            await FadeManager.FadeOut(token);

            AlliesCanEscape = battleEvent.AlliesCanEscape;

            Enemies.InitializeAndCreateEnemies(battleEvent.EnemyGroupId);

            if (battleEvent.BgmId > 0)
            {
                AudioManager.PlayBgm(battleEvent.BgmId);
            }

            BattleResultType battleResult = await ExecuteBattle(token);

            if (battleEvent.BgmId > 0)
            {
                AudioManager.LoadBgm(1.0f);
            }

            return battleResult;
        }

        public void Initialize()
        {
            // 戦闘中フラグ
            _isRunning = true;

            // 味方逃走フラグ
            _alliesEscape = false;

            // 戦闘終了フラグ
            IsOver = false;

            // 経験値
            RewardExperience = 0;

            // カメラの視点
            _cameraController.Target = _cameraTransform;
        }

        /// <summary>
        /// 戦闘を行う
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask<BattleResultType> ExecuteBattle(CancellationToken token)
        {
            Initialize();

            // 戦闘開始
            await StartBattle(token);

            // 戦闘ループ
            while (CanContinue)
            {
                // 時間経過
                AdvanceTime();

                // 行動者がいなければ戦闘終了
                if (!ActiveContainer)
                {
                    break;
                }

                // ターン開始
                await StartTurn(token);

                // 行動ループ
                while (ActiveContainerCanAct)
                {
                    // 行動決定
                    await DecideActorAction(token);

                    // 行動実行
                    if (ActiveContainerAction is not null)
                    {
                        await ExecuteActorAction(token);
                    }
                }

                // 味方が逃走するなら終了
                if (_alliesEscape)
                {
                    break;
                }

                // ターン終了
                await EndTurn(token);
            }

            // 戦闘終了
            await EndBattle(token);

            return _result;
        }

        /// <summary>
        /// 戦闘開始
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask StartBattle(CancellationToken token)
        {
            await FadeManager.FadeOut(token);

            // 通知
            MessageBrokersManager.BattleStart.Publish(this);

            // UI表示
            BattleUI.SetEnabled(true);

            // 位置調整
            Enemies.AdjsutEnemiesPosition();

            await FadeManager.FadeIn(token);

            Allies.OnBattleStart();
            Enemies.OnBattleStart();
        }

        /// <summary>
        /// 時間経過
        /// </summary>
        /// <returns></returns>
        private void AdvanceTime()
        {
            // 戦闘可能なコンテナ配列
            CombatantContainer[] fightables = GetFightables();

            // 経過時間
            int elapsedTime = fightables.Min(x => x.Combatant.WaitTime);

            // 時間を進める
            foreach (var fightable in fightables)
            {
                fightable.OnTimeElapsed(elapsedTime);
            }

            // 行動者を初期化する
            ActiveContainer = null;

            // 行動順を更新する
            UpdateOrderOfActions();

            // 行動者を決定する
            ActiveContainer = OrderOfActions.FirstOrDefault();
        }

        /// <summary>
        /// ターン開始
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask StartTurn(CancellationToken token)
        {
            await ActiveContainer.OnTurnStart(token);
        }

        /// <summary>
        /// 行動決定
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask DecideActorAction(CancellationToken token)
        {
            ActiveContainerAction = null;
            ActiveContainerActionTargets = null;

            // 行動者が操作可能な場合
            if (ActiveContainer.ContainsPlayerControlled())
            {
                // プレイヤーによる行動決定
                await DecideByPlayer(token);
            }
            else
            {
                // AIによる行動決定
                DecideByAI();
            }
        }

        /// <summary>
        /// プレイヤーによる行動決定
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask DecideByPlayer(CancellationToken token)
        {
            // 行動が決定された
            bool ActionHasBeenDecided()
            {
                return ActiveContainerAction is not null && ActiveContainerActionTargets is not null;
            }

            // コマンド更新
            BattleUI.UpdateUI(this);

            // コマンド選択
            BattleUI.BattleCommands.BeSelected();

            // 行動決定まで待機
            await UniTask.WaitUntil(
                ActionHasBeenDecided,
                cancellationToken: token);

            // コマンド非表示
            BattleUI.BattleCommands.SetEnabled(false);
        }

        /// <summary>
        /// AIによる行動決定
        /// </summary>
        private void DecideByAI()
        {
            // 行動決定
            ActiveContainerAction = ActiveContainer.Combatant.DecideAction();
            if (ActiveContainerAction is null)
            {
                return;
            }

            // 対象決定
            CombatantContainer[] targetables = ActiveContainer.GetTargetables(ActiveContainerAction.Effect).ToArray();
            ActiveContainerActionTargets = GetTargets(ActiveContainer, targetables, ActiveContainerAction.Effect);
        }

        /// <summary>
        /// 行動実行
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask ExecuteActorAction(CancellationToken token)
        {
            await ActiveContainer.Act(ActiveContainerAction, ActiveContainerActionTargets, token);
            UpdateOrderOfActions();
        }

        /// <summary>
        /// ターン終了の処理
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask EndTurn(CancellationToken token)
        {
            await ActiveContainer.OnTurnEnd(token);
        }

        /// <summary>
        /// 戦闘終了
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask EndBattle(CancellationToken token)
        {
            // 戦闘結果決定
            if (!_alliesEscape)
            {
                if (Allies.CanFight())
                {
                    _result = BattleResultType.Win;
                }
                else
                {
                    _result = BattleResultType.Lose;
                }

                // 待機
                await TimeUtility.Wait(GameSettings.WaitTime.BattleEnd, token);
            }
            else
            {
                _result = BattleResultType.Canceled;
            }

            // フェードアウト
            await FadeManager.FadeOut(token);

            // 戦闘終了時の処理
            Allies.OnBattleEnd();

            if (_result == BattleResultType.Win)
            {
                VariableData.Energy += RewardSpirit;
                Allies.OnWin(RewardExperience);
                Debug.Log("勝利");
            }

            // 戦闘終了通知
            MessageBrokersManager.BattleEnd.Publish(this);

            // 敵初期化
            Enemies.Initialize();

            // UIを隠す
            BattleUI.SetEnabled(false);

            // ★ 暫定
            await UniTask.Delay(500);

            // カメラ視点を元に戻す
            _cameraController.ToPrevious();

            // 戦闘終了フラグON
            IsOver = true;

            // 戦闘中フラグOFF
            _isRunning = false;
        }

        /// <summary>
        /// 戦闘可能なコンテナを取得する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private CombatantContainer[] GetFightables()
        {
            CombatantContainer[] fightables = Allies
                .GetFightables()
                .Concat(Enemies.GetFightables())
                .ToArray();

            return fightables;
        }

        /// <summary>
        /// 対象を取得する
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="targetables"></param>
        /// <param name="effect"></param>
        private CombatantContainer[] GetTargets(CombatantContainer actor, CombatantContainer[] targetables, EffectData effect)
        {
            if (!targetables.Any()
                || effect.TargetSelection != TargetSelectionType.Single)
            {
                return targetables;
            }
            else
            {
                int index = actor.Combatant.DecideTargetIndex(targetables);
                CombatantContainer[] targets = { targetables[index] };

                return targets;
            }
        }

        /// <summary>
        /// 敵が倒された、または浄化されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnEnemyKnockedOutOrPurified(CombatantContainer container)
        {
            // 経験値増加
            RewardExperience += container.Combatant.CalculateRewardExperience();
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
                .OrderByDescending(x => x == ActiveContainer)
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
