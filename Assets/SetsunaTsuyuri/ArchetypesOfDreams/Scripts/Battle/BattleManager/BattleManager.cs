using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
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
    /// 戦闘の管理者
    /// </summary>
    public partial class BattleManager : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        public static BattleManager InstanceInActiveScene { get; private set; } = null;

        /// <summary>
        /// 戦闘UIの管理者
        /// </summary>
        [field: SerializeField]
        public BattleUIManager BattleUI { get; private set; } = null;

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
        /// 味方の管理者
        /// </summary>
        public AllyContainersManager Allies { get; private set; } = null;

        /// <summary>
        /// 敵の管理者
        /// </summary>
        public EnemyContainersManager Enemies { get; private set; } = null;

        /// <summary>
        /// 現在のターン
        /// </summary>
        public int Turn { get; private set; } = 0;

        /// <summary>
        /// 逃走を禁じる
        /// </summary>
        public bool ForbidEscaping { get; private set; } = false;

        /// <summary>
        /// 行動者コンテナ
        /// </summary>
        public CombatantContainer Actor { get; private set; } = null;

        /// <summary>
        /// 行動者の行動内容
        /// </summary>
        public ActionModel ActorAction { get; private set; } = null;

        /// <summary>
        /// この戦いで得られる経験値
        /// </summary>
        public int RewardExperience { get; private set; } = 0;

        /// <summary>
        /// 有限ステートマシン
        /// </summary>
        public StateMachine<BattleManager> State { get; private set; } = null;

        /// <summary>
        /// ゲームコマンド
        /// </summary>
        GameCommand gameCommand = null;

        /// <summary>
        /// 戦闘が終わった
        /// </summary>
        public bool IsOver { get; private set; } = false;

        private void Awake()
        {
            Allies = GetComponentInChildren<AllyContainersManager>();
            Enemies = GetComponentInChildren<EnemyContainersManager>();

            SetUpStateMachine();

            InstanceInActiveScene = this;
        }

        /// <summary>
        /// ステートマシンの設定を行う
        /// </summary>
        private void SetUpStateMachine()
        {
            State = new StateMachine<BattleManager>(this);
            State.Add<Sleep>();
            State.Add<Preparation>();
            State.Add<BattleStart>();
            State.Add<TimeAdvancing>();
            State.Add<ActionStart>();
            State.Add<CommandSelection>();
            State.Add<TargetSelection>();
            State.Add<ActionExecution>();
            State.Add<ActionEnd>();
            State.Add<BattleEnd>();
        }

        private void Start()
        {
            // 各UIをセットアップする
            BattleUI.SetUpButtons(this);

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

            // ターン数
            Turn = 0;

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
            AudioManager.PlayBgm("通常戦闘");

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
                AudioManager.PlayBgm("通常戦闘");
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

            BattleResultType result = BattleResultType.Win;
            return result;
        }

        /// <summary>
        /// 戦闘を行う
        /// </summary>
        /// <param name="command">ゲームコマンド</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask ExecuteBattle(GameCommand command, CancellationToken token)
        {
            // 休眠状態以外の場合は戦闘を行わない
            if (!(State.Current is Sleep))
            {
                return;
            }

            // 初期化する
            Initialize();

            // ゲームコマンド
            gameCommand = command;

            // 準備状態へ移行する
            State.Change<Preparation>();

            // 戦闘終了まで待つ
            await UniTask.WaitUntil(() => IsOver, cancellationToken: token);
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
        /// スキルが選択されたときの処理
        /// </summary>
        /// <param name="skill">スキル</param>
        public void OnSkillSelected(ActionModel skill)
        {
            ActorAction = skill;

            // 対象選択ステートへ移行する
            State.Change<TargetSelection>();
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
        /// PlayerInput「Move」発動時のUnityEvent
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            if (State.Current is TargetSelection)
            {
                ChangeTarget(context);
            }
        }

        /// <summary>
        /// PlayerInput「Fire」発動時のUnityEvent
        /// </summary>
        /// <param name="context"></param>
        public void OnFire(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            if (State.Current is TargetSelection)
            {
                DecideTarget();
            }
        }

        /// <summary>
        /// PlayerInput「Cancel」発動時のUnityEvent
        /// </summary>
        /// <param name="context"></param>
        public void OnCancel(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            if (State.Current is TargetSelection)
            {
                CancelTargetSelection();
            }
        }

        /// <summary>
        /// 対象を変更する
        /// </summary>
        /// <param name="context"></param>
        private void ChangeTarget(InputAction.CallbackContext context)
        {
            // 対象がいなければ中止する
            if (Targetables == null || Targetables.Length == 0)
            {
                return;
            }

            // 非単体対象なら中止する
            if (targetIsNotSingle)
            {
                return;
            }

            // 入力に応じて対象を隣に移す
            Targetables[targetIndex].IsTargeted = false;

            Vector2 input = context.ReadValue<Vector2>();
            if (input == Vector2.right || input == Vector2.down)
            {
                targetIndex++;
                if (targetIndex >= Targetables.Length)
                {
                    targetIndex = 0;
                }
            }
            else if (input == Vector2.left || input == Vector2.up)
            {
                targetIndex--;
                if (targetIndex < 0)
                {
                    targetIndex = Targetables.Length - 1;
                }
            }

            Targetables[targetIndex].IsTargeted = true;
        }

        /// <summary>
        /// 対象を決定する
        /// </summary>
        private void DecideTarget()
        {
            // 行動実行ステートへ非同期的に移行する
            ToActionExecutionStateAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        /// <summary>
        /// 行動実行ステートへ移行する
        /// </summary>
        /// <param name="token">トークン</param>
        /// <returns></returns>
        private async UniTask ToActionExecutionStateAsync(CancellationToken token)
        {
            await UniTask.Yield(token);
            await UniTask.Yield(token);
            State.Change<ActionExecution>();
        }

        /// <summary>
        /// 対象選択をキャンセルする
        /// </summary>
        private void CancelTargetSelection()
        {
            // 対象から外す
            CombatantContainer[] targets = Targetables
                .Where(x => x.IsTargeted)
                .ToArray();

            foreach (var target in targets)
            {
                target.IsTargeted = false;
            }

            // コマンド選択ステートへ非同期的に移行する
            ToCommandSelectionAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        /// <summary>
        /// コマンド選択ステートへ移行する
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask ToCommandSelectionAsync(CancellationToken token)
        {
            // 2回待たないとステート遷移直後にボタンのキャンセルが発生する？
            await UniTask.Yield(token);
            await UniTask.Yield(token);
            State.Change<CommandSelection>();
        }
    }
}
