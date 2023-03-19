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
        /// コマンド選択開始時のイベント(削除予定)
        /// </summary>
        [SerializeField]
        GameEventWithBattleManager onPlayerControlledCombatantCommandSelection = null;

        /// <summary>
        /// コマンド選択終了時のイベント(削除予定)
        /// </summary>
        [SerializeField]
        GameEvent onCommandSelectionExit = null;

        /// <summary>
        /// 行動終了開始時のイベント(削除予定)
        /// </summary>
        [SerializeField]
        GameEvent onTurnEndEnter = null;

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
        /// 行動内容
        /// </summary>
        public ActionInfo ActorAction { get; set; } = null;

        /// <summary>
        /// 行動の対象
        /// </summary>
        public CombatantContainer[] ActorActionTargets { get; set; } = null;

        /// <summary>
        /// 得られる経験値
        /// </summary>
        public int RewardExperience { get; private set; } = 0;

        /// <summary>
        /// 逃走を禁じる
        /// </summary>
        public bool ForbidEscaping { get; private set; } = false;

        /// <summary>
        /// 戦闘が終わった
        /// </summary>
        public bool IsOver { get; private set; } = false;

        /// <summary>
        /// 戦闘続行可能である
        /// </summary>
        /// <returns></returns>
        private bool CanContinue => Allies.CanFight() && Enemies.CanFight();

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
        }

        public void Initialize()
        {
            // 戦闘中フラグ
            _isRunning = true;

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

            // 戦闘開始
            await StartBattle(token);

            while (CanContinue)
            {
                // 時間経過
                AdvanceTime();
                if (Actor == null)
                {
                    break;
                }

                // ターン開始
                await StartTurn(token);

                while (Actor.ContainsActionable)
                {
                    // 行動決定
                    await DecideActorAction(token);
                    if (ActorAction is not null)
                    {
                        // 行動実行
                        await ExecuteActorAction(token);
                    }
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

            // UI表示
            BattleUI.Show();

            // 位置表示
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
            Actor = null;

            // 行動順を更新する
            UpdateOrderOfActions();

            // 行動者を決定する
            Actor = OrderOfActions.FirstOrDefault();
        }

        /// <summary>
        /// ターン開始
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask StartTurn(CancellationToken token)
        {
            await Actor.OnTurnStart(token);
        }

        /// <summary>
        /// 行動決定
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask DecideActorAction(CancellationToken token)
        {
            ActorAction = null;
            ActorActionTargets = null;

            // 行動者が操作可能な場合
            if (Actor.ContainsPlayerControlled())
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
                return ActorAction is not null && ActorActionTargets is not null;
            }

            // コマンド更新
            BattleUI.UpdateUI(this);

            // コマンド選択
            BattleUI.BattleCommands.BeSelected();

            // イベント実行
            onPlayerControlledCombatantCommandSelection.Invoke(this);

            await UniTask.WaitUntil(
                ActionHasBeenDecided,
                cancellationToken: token);

            // コマンド非表示
            BattleUI.BattleCommands.Hide();

            // イベント実行
            onCommandSelectionExit.Invoke();
        }

        /// <summary>
        /// AIによる行動決定
        /// </summary>
        private void DecideByAI()
        {
            // 行動決定
            ActorAction = Actor.Combatant.DecideAction();
            if (ActorAction is null)
            {
                return;
            }

            // 対象決定
            CombatantContainer[] targetables = Actor.GetTargetables(ActorAction.Effect).ToArray();
            ActorActionTargets = GetTargets(Actor, targetables, ActorAction.Effect);
        }

        /// <summary>
        /// 行動実行
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask ExecuteActorAction(CancellationToken token)
        {
            await Actor.Act(ActorAction, ActorActionTargets, token);
            UpdateOrderOfActions();
        }

        /// <summary>
        /// ターン終了の処理
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask EndTurn(CancellationToken token)
        {
            onTurnEndEnter.Invoke();
            await Actor.OnTurnEnd(token);
        }

        /// <summary>
        /// 戦闘終了
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask EndBattle(CancellationToken token)
        {
            // 戦闘終了時の処理
            Allies.OnBattleEnd();

            if (Allies.CanFight())
            {
                Allies.OnWin(RewardExperience);

                _result = BattleResultType.Win;
                Debug.Log("味方の勝ち");
            }
            else
            {
                _result = BattleResultType.Lose;
                Debug.Log("敵の勝ち");
            }

            // ★ 待機
            await UniTask.Delay(300);

            // フェードアウト
            await FadeManager.FadeOut(token);

            Enemies.Initialize();

            // アセット解放
            await Resources.UnloadUnusedAssets();
            token.ThrowIfCancellationRequested();

            // UIを隠す
            BattleUI.Hide();

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
