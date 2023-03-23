using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// シナリオの管理者
    /// </summary>
    public class ScenarioManager : MonoBehaviour, IInitializable
    {
        public static ScenarioManager InstanceInActiveScene = null;

        /// <summary>
        /// コメント行として扱う文字
        /// </summary>
        [SerializeField]
        char commentChar = '*';

        /// <summary>
        /// 自動的に文章を読み進める
        /// </summary>
        [field: SerializeField]
        public bool Auto { get; set; } = false;

        /// <summary>
        /// 演者の名前を伏せる場合に表示される名前
        /// </summary>
        [SerializeField]
        string anonymous = "???";

        /// <summary>
        /// 自動的にメッセージを送る際に最低限待つ時間
        /// </summary>
        [SerializeField]
        public float waitTimeForAuto = 3.0f;

        /// <summary>
        /// 1文字毎の待機時間
        /// </summary>
        [SerializeField]
        public float waitTimePerCharacter = 0.1f;

        /// <summary>
        /// 背景データグループ
        /// </summary>
        [SerializeField]
        BackgroundDataGroup backgroundDataGroup = null;

        /// <summary>
        /// メッセージ速度
        /// </summary>
        [SerializeField]
        float waitTimeForMessage = 0.05f;

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
        /// 名前テキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI nameText = null;

        /// <summary>
        /// メッセージテキスト
        /// </summary>
        [SerializeField]
        TextMeshProUGUI sentenceText = null;

        /// <summary>
        /// フェード画像
        /// </summary>
        [SerializeField]
        ImageController screenFade = null;

        /// <summary>
        /// 演者の管理者
        /// </summary>
        [SerializeField]
        ActorsManager actors = null;

        /// <summary>
        /// 選択肢ボタンの管理者
        /// </summary>
        [SerializeField]
        SelectionButtonsManager selectionButtons = null;

        /// <summary>
        /// シナリオ再生開始時のイベント
        /// </summary>
        [SerializeField]
        GameEvent onScenarioPlayingStart = null;

        /// <summary>
        /// シナリオ再生終了時のイベント
        /// </summary>
        [SerializeField]
        GameEvent onScenarioPlayingEnd = null;

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

            actors.SetUp(this);
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
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask Play(int actorId, int expressionId, string name, string message, CancellationToken token)
        {
            IsPlaying = true;
            onScenarioPlayingStart.Invoke();

            sentenceText.text = string.Empty;
            ChangeNameText(name);

            Show();
            await DisplaySentenceAsync(message, token);

            IsPlaying = false;
            onScenarioPlayingEnd.Invoke();
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

            // イベント呼び出し
            onScenarioPlayingStart.Invoke();

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

            // イベント呼び出し
            onScenarioPlayingEnd.Invoke();

            // フラグOFF
            IsPlaying = false;
        }

        public void Initialize()
        {
            ClearNameAndSentence();
            actors.Initialize();
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
                tasks.Add(DisplaySentenceAsync(command.Sentence, token));

                // 演者の台詞または名前表示コマンドでなければ名前欄を空欄にする
                if (command.CommandAttribute != Attribute.Command.Actor &&
                    command.CommandAttribute != Attribute.Command.Name)
                {
                    ChangeNameText(string.Empty);
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
                    ClearNameAndSentence();
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
            string name = anonymous;

            // 名前が設定されているなら、その名前にする
            if (!string.IsNullOrEmpty(command.Name))
            {
                name = command.Name;
            }

            // 名前表示変更
            ChangeNameText(name);
        }

        private void OnCommandOfActor(List<UniTask> tasks, CommandData command, CancellationToken token)
        {
            // 対象の演者
            Actor target = null;
            switch (command.CommandOfActor)
            {
                // 名前のみ
                case Attribute.CommandOfActor.NameOnly:
                    string name = actors.FindNameOrEmpty(command);
                    ChangeNameText(name);
                    break;

                // フェードイン
                case Attribute.CommandOfActor.FadeIn:
                    target = actors.FindAndSetData(command);
                    tasks.Add(actors.FadeIn(target, token));
                    break;

                // フェードアウト
                case Attribute.CommandOfActor.FadeOut:
                    target = actors.Find(command);
                    tasks.Add(actors.FadeOut(target, token));
                    break;

                // 全てフェードアウト
                case Attribute.CommandOfActor.FadeOutAll:
                    target = actors.Find(command);
                    tasks.Add(actors.FadeOutAll(token));
                    break;

                // 演者取得
                default:
                    target = actors.Find(command);

                    // 見つからなければフェードインで作る
                    if (!target)
                    {
                        target = actors.FindAndSetData(command);
                        tasks.Add(actors.FadeIn(target, token));
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
                        ChangeNameText(anonymous);
                    }
                    else
                    {
                        // 演者の名前表示
                        ChangeNameText(target.Data.DisplayName);
                    }
                }

                // 強調
                if (command.HasSentence())
                {
                    actors.Spotlight(target);
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
                        jumpTo = () => IEnumerableUtility.IndexOf(currentCommands, command => command.IsSelectionEnd(), local);
                    }
                    else
                    {
                        // 選択中フラグON
                        isSelecting = true;

                        // ジャンプ先設定 選択肢分岐点の次へ
                        int local = commandsIndex;
                        jumpTo = () => IEnumerableUtility.IndexOf(currentCommands, command => command.IsSelectionCase(requestedCaseIndex), local) + 1;

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

        private async UniTask DisplaySentenceAsync(string sentence, CancellationToken token)
        {
            // 表示できる文字数をゼロにする
            sentenceText.maxVisibleCharacters = 0;

            // テキストを更新する
            sentenceText.text = sentence;

            // GetParsedText()の前に更新する
            sentenceText.ForceMeshUpdate();

            // タグを除いた文字数
            int maxCharacters = sentenceText.GetParsedText().Length;

            // 表示する文字数
            int displayCharacters = 0;

            // 1文字ずつ表示する
            while (displayCharacters < maxCharacters)
            {
                displayCharacters++;
                sentenceText.maxVisibleCharacters = displayCharacters;

                float time = 0.0f;
                bool skip = false;
                while (time < waitTimeForMessage)
                {
                    time += Time.deltaTime;
                    await UniTask.Yield(token);

                    // スキップ
                    if (IsRequestedToSkip())
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip)
                {
                    break;
                }
            }

            // 全ての文字を表示する
            sentenceText.maxVisibleCharacters = maxCharacters;
            await UniTask.Yield(token);

            // 自動モード時の待機時間カウンター
            float timeCountForAuto = 0.0f;

            // 文字数に応じて待機時間を変える
            float maxTimeCountForAuto = sentence.Length * waitTimePerCharacter;

            // 待機時間が短すぎないようにする
            if (maxTimeCountForAuto < waitTimeForAuto)
            {
                maxTimeCountForAuto = waitTimeForAuto;
            }

            // 入力 or (自動モードのみ)待機時間終了を待つ
            while (!IsRequestedToSkip())
            {
                if (Auto && timeCountForAuto >= maxTimeCountForAuto)
                {
                    break;
                }
                else if (timeCountForAuto < maxTimeCountForAuto)
                {
                    timeCountForAuto += Time.deltaTime;
                }

                await UniTask.Yield(token);
            }
        }

        /// <summary>
        /// 文章を空にする
        /// </summary>
        private void ClearNameAndSentence()
        {
            nameText.text = "";
            sentenceText.text = "";
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
        /// 名前の表示を変更する
        /// </summary>
        /// <param name="name">名前</param>
        public void ChangeNameText(string name)
        {
            nameText.text = name;
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
