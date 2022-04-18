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
    /// 戦闘の管理者
    /// </summary>
    public partial class BattleManager : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// 戦闘UIの管理者
        /// </summary>
        [field: SerializeField]
        public BattleUIManager BattleUI { get; private set; } = null;

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
        /// 現在のターンの行動順
        /// </summary>
        public List<CombatantContainer> OrderOfActions { get; private set; } = new List<CombatantContainer>();

        /// <summary>
        /// 行動中の戦闘者コンテナ
        /// </summary>
        public CombatantContainer Performer { get; private set; } = null;

        /// <summary>
        /// 行動中の戦闘者が使用するスキル
        /// </summary>
        public Skill SkillToBeUsed { get; private set; } = null;

        /// <summary>
        /// この戦いで得られる経験値
        /// </summary>
        public int Experience { get; private set; } = 0;

        /// <summary>
        /// 有限ステートマシン
        /// </summary>
        public FiniteStateMachine<BattleManager> State { get; private set; }

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
            SetUpStateMachine();

            Allies = GetComponentInChildren<AllyContainersManager>();
            Enemies = GetComponentInChildren<EnemyContainersManager>();
        }

        /// <summary>
        /// ステートマシンの設定を行う
        /// </summary>
        private void SetUpStateMachine()
        {
            State = new FiniteStateMachine<BattleManager>(this);

            State.Add<Sleep>();
            State.Add<Preparation>();
            State.Add<BattleStart>();
            State.Add<TurnStart>();
            State.Add<ActionStart>();
            State.Add<CommandSelection>();
            State.Add<TargetSelection>();
            State.Add<ActionExecution>();
            State.Add<ActionEnd>();
            State.Add<TurnEnd>();
            State.Add<BattleEnd>();
            State.ProhibitAdding();
            State.Change<Sleep>();
        }

        private void Start()
        {
            // 各UIをセットアップする
            BattleUI.SetUpButtons(this);
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
            Experience = 0;

            // 敵
            Enemies.Initialize();
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
        /// 行動順を更新する
        /// </summary>
        private void UpdateOrderOfActions()
        {
            OrderOfActions = Allies.Members
                .Concat(Enemies.Members as CombatantContainer[])
                .Where(x => x.ContainsFightable())
                .OrderBy(a => a.Combatant.ActionPriority)
                .ThenBy(a => a.Combatant.BaseSpeed + a.Combatant.RandomSpeed)
                .ToList();
        }

        /// <summary>
        /// 戦闘者全員のランダムに変わる素早さを更新する
        /// </summary>
        private void UpdateRandomSpeedOfCombatants()
        {
            Allies.UpdateRandomSpeedOfCombatants();
            Enemies.UpdateRandomSpeedOfCombatants();
        }

        /// <summary>
        /// スキルが選択されたときの処理
        /// </summary>
        /// <param name="skill">スキル</param>
        public void OnSkillSelected(Skill skill)
        {
            SkillToBeUsed = skill;

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
            Experience += container.Combatant.Experience;
            Debug.Log($"経験値: {Experience}");
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
                Targetables.IncrementIndexLoop(ref targetIndex);
            }
            else if (input == Vector2.left || input == Vector2.up)
            {
                Targetables.DecrementIndexLoop(ref targetIndex);
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
        /// <param name="token">トークン</param>
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
