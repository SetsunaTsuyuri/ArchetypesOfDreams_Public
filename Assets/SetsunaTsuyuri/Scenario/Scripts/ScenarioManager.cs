using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SetsunaTsuyuri.ArchetypesOfDreams;

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// シナリオの管理者
    /// </summary>
    public class ScenarioManager : MonoBehaviour, IInitializable
    {
        public static ScenarioManager InstanceInActiveScene { get; private set; } = null;

        /// <summary>
        /// コメント行として扱う文字
        /// </summary>
        [SerializeField]
        char commentChar = '*';

        /// <summary>
        /// 背景データグループ
        /// </summary>
        [SerializeField]
        BackgroundDataGroup backgroundDataGroup = null;

        /// <summary>
        /// フェード速度(画面)
        /// </summary>
        [SerializeField]
        float waitTimeForScreenFade = 0.5f;

        /// <summary>
        /// フェード速度(背景)
        /// </summary>
        [SerializeField]
        float waitTimeForBackgroundFade = 0.5f;

        /// <summary>
        /// 背景
        /// </summary>
        [SerializeField]
        ImageController background = null;

        /// <summary>
        /// フェード画像
        /// </summary>
        [SerializeField]
        ImageController screenFade = null;

        /// <summary>
        /// 演者の管理者
        /// </summary>
        [SerializeField]
        ActorsManager _actors = null;

        /// <summary>
        /// 文章の管理者
        /// </summary>
        [SerializeField]
        TextsManager _texts = null;

        /// <summary>
        /// 選択肢ボタンの管理者
        /// </summary>
        [SerializeField]
        SelectionButtonsManager selectionButtons = null;

        /// <summary>
        /// 選択中である
        /// </summary>
        bool isSelecting = false;

        /// <summary>
        /// 要求された選択肢の索引
        /// </summary>
        int requestedCaseIndex = 0;

        /// <summary>
        /// コマンドのジャンプ先を決める関数
        /// </summary>
        Func<int> jumpTo = null;

        /// <summary>
        /// シナリオ再生中である
        /// </summary>
        public bool IsPlaying { get; private set; } = false;

        /// <summary>
        /// 現在のコマンド配列
        /// </summary>
        CommandData[] currentCommands = null;

        /// <summary>
        /// コマンドの索引
        /// </summary>
        int commandsIndex = 0;

        /// <summary>
        /// キャンバス
        /// </summary>
        Canvas canvas = null;

        /// <summary>
        /// キャンバスグループ
        /// </summary>
        CanvasGroup canvasGroup = null;

        /// <summary>
        /// タスクリスト
        /// </summary>
        readonly List<UniTask> _tasks = new();

        public static void EndIfPlaying()
        {
            if (!InstanceInActiveScene)
            {
                return;
            }

            InstanceInActiveScene.End();
        }

        private void Awake()
        {
            SetUp();

            InstanceInActiveScene = this;
        }

        /// <summary>
        /// セットアップを行う
        /// </summary>
        private void SetUp()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            Hide();

            _actors.SetUp(this);
            _texts.SetUp(IsRequestedToSkip);
            selectionButtons.SetUp(this);

            screenFade.IsRequestedToSkip += IsRequestedToSkip;
            background.IsRequestedToSkip += IsRequestedToSkip;
        }

        /// <summary>
        /// 表示する
        /// </summary>
        private void Show()
        {
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvas.enabled = true;
        }

        /// <summary>
        /// 隠す
        /// </summary>
        private void Hide()
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvas.enabled = false;
        }

        /// <summary>
        /// タスクリストが終わるまで待機し、終了後にタスクリストをクリアする
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask WhenAllAndClearTasks(CancellationToken token)
        {
            await UniTask.WhenAll(_tasks);
            token.ThrowIfCancellationRequested();

            _tasks.Clear();
        }

        /// <summary>
        /// 再生を開始する
        /// </summary>
        /// <param name="csv">CSVテキスト</param>
        public void StartPlaying(TextAsset csv)
        {
            if (IsPlaying)
            {
                return;
            }

            PlayCsv(csv, this.GetCancellationTokenOnDestroy()).Forget();
        }

        /// <summary>
        /// 再生する
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="expressionId"></param>
        /// <param name="name"></param>
        /// <param name="dialog"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask Play(int actorId, int expressionId, string name, string dialog, CancellationToken token)
        {
            // 開始
            if (!IsPlaying)
            {
                OnSrart();
            }

            // 名前の表示
            string displayName = name;
            if (string.IsNullOrEmpty(displayName) && actorId > 0)
            {
                displayName = _actors.GetDisplayName(actorId);
            }
            _texts.SetNameText(displayName);


            // 演者の表示
            if (actorId > 0)
            {
                _tasks.Add(_actors.Play(actorId, expressionId, token));
            }

            // 文章の表示
            _tasks.Add(_texts.Play(dialog, token));

            await WhenAllAndClearTasks(token);
        }

        /// <summary>
        /// 再生開始時の処理
        /// </summary>
        private void OnSrart()
        {
            Initialize();
            IsPlaying = true;
            Show();

            MessageBrokersManager.ScenarioStart.Publish(this);
        }

        /// <summary>
        /// 再生を終了する
        /// </summary>
        public void End()
        {
            IsPlaying = false;
            Hide();

            MessageBrokersManager.ScenarioEnd.Publish(this);
        }

        /// <summary>
        /// 画面のフェードイン・アウトを行う
        /// </summary>
        /// <param name="command"></param>
        /// <param name="duration"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask FadeScreen(Attribute.CommandOfScreen command, float duration, CancellationToken token)
        {
            switch (command)
            {
                // フェードイン
                case Attribute.CommandOfScreen.FadeIn:
                    await ScreenFadeIn(duration, token);
                    break;

                // フェードアウト
                case Attribute.CommandOfScreen.FadeOut:
                    await ScreenFadeOut(duration, token);
                    break;
            }
        }

        /// <summary>
        /// 非同期的に再生する
        /// </summary>
        /// <param name="csv">CSVテキスト</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask PlayCsv(TextAsset csv, CancellationToken token)
        {
            // フラグON
            IsPlaying = true;

            // 通知
            MessageBrokersManager.ScenarioStart.Publish(this);

            // 初期化
            Initialize();

            // コマンドに変換
            currentCommands = ToCommandArray(csv.text);

            // 表示する
            Show();

            // やることリスト
            List<UniTask> tasks = new List<UniTask>();

            commandsIndex = 0;
            while (commandsIndex < currentCommands.Length)
            {
                // コマンド
                CommandData command = currentCommands[commandsIndex];

                // コマンドの内容に応じて、処理を決定する
                AddThingsToDo(tasks, command, token);

                // 待機しない設定の場合
                if (command.NoWait)
                {
                    // 処理を溜めたまま次のコマンドへ進む
                    commandsIndex++;
                    continue;
                }

                // 処理を実行 全て終了するまで待機する
                await UniTask.WhenAll(tasks);

                // 処理をクリア
                tasks.Clear();

                // コマンドのジャンプ先を決める関数が設定されている場合
                if (jumpTo != null)
                {
                    int next = jumpTo();
                    if (!currentCommands.OutOfRange(next))
                    {
                        commandsIndex = next;
                    }
                    else
                    {
                        commandsIndex++;
                    }

                    // リセット
                    jumpTo = null;
                }
                else
                {
                    commandsIndex++;
                }
            }

            // 隠す
            Hide();

            // 通知
            MessageBrokersManager.ScenarioEnd.Publish(this);

            // フラグOFF
            IsPlaying = false;
        }

        public void Initialize()
        {
            _tasks.Clear();

            _actors.Initialize();
            _texts.Initialize();
        }

        /// <summary>
        /// Stringをコマンドリスト配列に変換する
        /// </summary>
        /// <param name="csv">CSVテキストアセット</param>
        /// <returns></returns>
        private CommandData[] ToCommandArray(string csv)
        {
            List<CommandData> commandList = new List<CommandData>();
            List<List<string>> csvList = CSVReader.ParseCSV(csv);
            foreach (var line in csvList)
            {
                // コメント行は飛ばす
                if (IsCommentRowAndReplace(line))
                {
                    continue;
                }

                // コマンドリストに加える
                commandList.Add(new CommandData(line));
            }

            CommandData[] commands = commandList.ToArray();
            return commands;
        }

        /// <summary>
        /// コメント行であるかどうかを確かめ、文字列の変換も行う
        /// </summary>
        /// <param name="row">行</param>
        /// <returns></returns>
        private bool IsCommentRowAndReplace(List<string> row)
        {
            bool result = false;

            // データ1列目の取得を試みる
            if (row.TryGetValue(0, out string firstColumn))
            {
                // 最初の1文字のみがコメントアウト用の文字ならコメント行として扱う
                char char1st = firstColumn.GetValueOrDefault(0);
                if (char1st == commentChar)
                {
                    // 2文字目もコメントアウト用の文字ならコメント行ではない
                    char char2nd = firstColumn.GetValueOrDefault(1);
                    if (char2nd == commentChar)
                    {
                        // コメントアウト用の文字を1文字に置き換える
                        string commentString = commentChar.ToString();
                        row[0] = row[0].Replace(commentString + commentString, commentString);
                    }
                    else
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        private void AddThingsToDo(List<UniTask> tasks, CommandData command, CancellationToken token)
        {
            if (command.HasSentence() && command.CommandAttribute != Attribute.Command.Selection)
            {
                // 文章表示
                tasks.Add(_texts.Play(command.Sentence, token));

                // 演者の台詞または名前表示コマンドでなければ名前欄を空欄にする
                if (command.CommandAttribute != Attribute.Command.Actor &&
                    command.CommandAttribute != Attribute.Command.Name)
                {
                    _texts.SetNameText(string.Empty);
                }
            }
            switch (command.CommandAttribute)
            {
                case Attribute.Command.Delay:
                    // 待機コマンド
                    OnCommandOfDelay(tasks, command, token);
                    break;

                case Attribute.Command.ClearSentence:
                    // 文章をクリアする
                    _texts.Clear();
                    break;

                case Attribute.Command.Name:
                    // 名前コマンド
                    OnCommandOfName(command);
                    break;

                case Attribute.Command.Actor:
                    // 演者コマンド
                    OnCommandOfActor(tasks, command, token);
                    break;

                case Attribute.Command.Background:
                    // 背景コマンド
                    OnCommandOfBackground(tasks, command, token);
                    break;

                case Attribute.Command.Screen:
                    // 画面コマンド
                    OnCommandOfScreen(tasks, command, token);
                    break;

                case Attribute.Command.BGM:
                    // BGMコマンド
                    OnCommandOfBGM(command);
                    break;

                case Attribute.Command.SE:
                    // SEコマンド
                    OnCommandOfSE(command);
                    break;

                case Attribute.Command.Selection:
                    // 選択肢コマンド
                    OnCommandOfSelection(tasks, command, token);
                    break;
            }
        }

        private void OnCommandOfDelay(List<UniTask> tasks, CommandData command, CancellationToken token)
        {
            // 一定時間待機
            if (command.Duration.HasValue)
            {
                tasks.Add(Delay((float)command.Duration, token));
            }
        }

        private void OnCommandOfScreen(List<UniTask> tasks, CommandData command, CancellationToken token)
        {
            // 時間
            float duration = waitTimeForScreenFade;
            if (command.Duration.HasValue)
            {
                duration = (float)command.Duration;
            }

            switch (command.CommandOfScreen)
            {
                // フェードイン
                case Attribute.CommandOfScreen.FadeIn:
                    tasks.Add(ScreenFadeIn(duration, token));
                    break;

                // フェードアウト
                case Attribute.CommandOfScreen.FadeOut:
                    tasks.Add(ScreenFadeOut(duration, token));
                    break;
            }
        }

        private void OnCommandOfName(CommandData command)
        {
            // 表示する名前
            _texts.SetAnonymousText();

            // 名前が設定されているなら、その名前にする
            if (!string.IsNullOrEmpty(command.Name))
            {
                _texts.SetNameText(command.Name);
            }
        }

        private void OnCommandOfActor(List<UniTask> tasks, CommandData command, CancellationToken token)
        {
            // 対象の演者
            Actor target = null;
            switch (command.CommandOfActor)
            {
                // 名前のみ
                case Attribute.CommandOfActor.NameOnly:
                    string name = _actors.FindNameOrEmpty(command);
                    _texts.SetNameText(name);
                    break;

                // フェードイン
                case Attribute.CommandOfActor.FadeIn:
                    target = _actors.FindAndSetData(command);
                    tasks.Add(_actors.FadeIn(target, token));
                    break;

                // フェードアウト
                case Attribute.CommandOfActor.FadeOut:
                    target = _actors.Find(command);
                    tasks.Add(_actors.FadeOut(target, token));
                    break;

                // 全てフェードアウト
                case Attribute.CommandOfActor.FadeOutAll:
                    target = _actors.Find(command);
                    tasks.Add(_actors.FadeOutAll(token));
                    break;

                // 演者取得
                default:
                    target = _actors.Find(command);

                    // 見つからなければフェードインで作る
                    if (!target)
                    {
                        target = _actors.FindAndSetData(command);
                        tasks.Add(_actors.FadeIn(target, token));
                    }
                    break;
            }

            // 演者を取得できた場合
            if (target)
            {
                // 表示方法変更
                target.ChangeDisplayMethod(command.Display);

                // 表情変更
                target.ChangeExpression(command.Expression);

                // セリフがある場合は、名前欄を変更する
                if (command.HasSentence())
                {
                    if (command.Anonymous)
                    {
                        // 匿名表示
                        _texts.SetAnonymousText();
                    }
                    else
                    {
                        // 演者の名前表示
                        _texts.SetNameText(target.Data.DisplayName);
                    }
                }

                // 強調
                if (command.HasSentence())
                {
                    _actors.Spotlight(target);
                }
            }
        }

        private void OnCommandOfBackground(List<UniTask> tasks, CommandData command, CancellationToken token)
        {
            float duration = waitTimeForBackgroundFade;
            if (command.Duration.HasValue)
            {
                duration = (float)command.Duration;
            }

            tasks.Add(CrossFadeBackground(command.Name, duration, token));
        }

        private void OnCommandOfBGM(CommandData command)
        {
            switch (command.CommandOfBGM)
            {
                // 再生
                case Attribute.CommandOfBGM.Play:

                    if (command.Duration.HasValue)
                    {
                        float duration = (float)command.Duration;
                        AudioManager.PlayBgm(command.BGMName, duration);
                    }
                    else
                    {
                        AudioManager.PlayBgm(command.BGMName);
                    }
                    break;

                // 停止
                case Attribute.CommandOfBGM.Stop:


                    if (command.Duration.HasValue)
                    {
                        float duration = (float)command.Duration;
                        AudioManager.StopBgm(duration);
                    }
                    else
                    {
                        AudioManager.StopBgm();
                    }
                    break;

                default:
                    break;
            }
        }

        private void OnCommandOfSE(CommandData command)
        {
            switch (command.CommandOfSE)
            {
                // 再生
                case Attribute.CommandOfSE.Play:

                    AudioManager.PlaySE(command.SEName);
                    break;

                default:
                    break;
            }
        }

        private void OnCommandOfSelection(List<UniTask> tasks, CommandData command, CancellationToken token)
        {
            switch (command.CommandOfSelection)
            {
                case Attribute.CommandOfSelection.None:
                    selectionButtons.ShowAButton(command.SelectionName);
                    break;

                case Attribute.CommandOfSelection.Case:
                    if (isSelecting)
                    {
                        // ジャンプ先設定 選択肢終了へ
                        int local = commandsIndex;
                        jumpTo = () => currentCommands.IndexOf(command => command.IsSelectionEnd(), local);
                    }
                    else
                    {
                        // 選択中フラグON
                        isSelecting = true;

                        // ジャンプ先設定 選択肢分岐点の次へ
                        int local = commandsIndex;
                        jumpTo = () => currentCommands.IndexOf(command => command.IsSelectionCase(requestedCaseIndex), local) + 1;

                        // いずれかのボタンが押されるのを待つ
                        tasks.Add(selectionButtons.WaitUntilAnyButtonPressed(token));
                    }
                    break;

                case Attribute.CommandOfSelection.End:

                    // 選択中フラグOFF
                    isSelecting = false;
                    break;
            }
        }

        /// <summary>
        /// 選択肢ボタンが押されたときの処理
        /// </summary>
        /// <param name="index">何番目の選択肢か</param>
        public void OnSelectionButtonPressed(int index)
        {
            // 選ばれた選択肢の索引を決定する
            requestedCaseIndex = index;

            // ボタンを隠す
            selectionButtons.Hide();
        }

        /// <summary>
        /// 一定時間待つ
        /// </summary>
        /// <param name="time">待機時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask Delay(float time, CancellationToken token)
        {
            float count = 0.0f;
            while (count < time)
            {
                count += Time.deltaTime;
                await UniTask.Yield(token);

                // 決定を入力されたら飛ばす
                if (IsRequestedToSkip())
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 画面フェードイン
        /// </summary>
        /// <param name="duration">時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask ScreenFadeIn(float duration, CancellationToken token)
        {
            await screenFade.ChangeAlphaAsync(screenFade.MainImage, 1.0f, 0.0f, duration, token);
        }

        /// <summary>
        /// 画面フェードアウト
        /// </summary>
        /// <param name="duration">時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask ScreenFadeOut(float duration, CancellationToken token)
        {
            await screenFade.ChangeAlphaAsync(screenFade.MainImage, 0.0f, 1.0f, duration, token);
        }

        /// <summary>
        /// 背景のクロスフェードを行う
        /// </summary>
        /// <param name="name">時間</param>
        /// <param name="duration">時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask CrossFadeBackground(string name, float duration, CancellationToken token)
        {
            Sprite sprite = null;
            if (!string.IsNullOrEmpty(name))
            {
                sprite = backgroundDataGroup.GetDataOrDefault(name).BasicSprite;
            }

            await background.CrossFade(sprite, duration, token);
        }

        /// <summary>
        /// スキップが要求されている
        /// </summary>
        /// <returns></returns>
        public bool IsRequestedToSkip()
        {
            return InputUtility.Submit();
        }
    }
}
