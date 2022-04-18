using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.Scenario.Attribute
{
    /// <summary>
    /// コマンド
    /// </summary>
    public enum Command
    {
        None = 0,
        Delay,
        Label,
        ClearSentence,
        Screen,
        Name,
        Actor,
        Background,
        Selection,
        BGM,
        SE
    }

    /// <summary>
    /// 画面コマンド
    /// </summary>
    public enum CommandOfScreen
    {
        None = 0,
        FadeIn,
        FadeOut
    }

    /// <summary>
    /// 演者コマンド
    /// </summary>
    public enum CommandOfActor
    {
        None = 0,
        NameOnly,
        FadeIn,
        FadeOut,
        FadeOutAll
    }

    /// <summary>
    /// 選択肢コマンド
    /// </summary>
    public enum CommandOfSelection
    {
        None = 0,
        Case,
        End
    }

    /// <summary>
    /// BGMコマンド
    /// </summary>
    public enum CommandOfBGM
    {
        None = 0,
        Play,
        Stop
    }

    /// <summary>
    /// SEコマンド
    /// </summary>
    public enum CommandOfSE
    {
        None = 0,
        Play,
        Stop,
        StopAll
    }
}

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// コマンドの実行内容
    /// </summary>
    [Serializable]
    public class CommandData
    {
        /// <summary>
        /// 文章
        /// </summary>
        public string Sentence { get; private set; } = string.Empty;

        /// <summary>
        /// コマンド
        /// </summary>
        public Attribute.Command CommandAttribute { get; private set; } = Attribute.Command.None;

        /// <summary>
        /// 画面コマンド
        /// </summary>
        public Attribute.CommandOfScreen CommandOfScreen { get; private set; } = Attribute.CommandOfScreen.None;

        /// <summary>
        /// 演者コマンド
        /// </summary>
        public Attribute.CommandOfActor CommandOfActor { get; private set; } = Attribute.CommandOfActor.None;

        /// <summary>
        /// 選択肢コマンド
        /// </summary>
        public Attribute.CommandOfSelection CommandOfSelection { get; private set; } = Attribute.CommandOfSelection.None;

        /// <summary>
        /// BGMコマンド
        /// </summary>
        public Attribute.CommandOfBGM CommandOfBGM { get; private set; } = Attribute.CommandOfBGM.None;

        /// <summary>
        /// SEコマンド
        /// </summary>
        public Attribute.CommandOfSE CommandOfSE { get; private set; } = Attribute.CommandOfSE.None;

        /// <summary>
        /// 演者の表情
        /// </summary>
        public Attribute.Expression Expression { get; private set; } = Attribute.Expression.NoChange;

        /// <summary>
        /// 演者の表示方法
        /// </summary>
        public Attribute.Display Display { get; private set; } = Attribute.Display.NoChange;

        /// <summary>
        /// 演者の位置
        /// </summary>
        public Attribute.Position Position { get; private set; } = Attribute.Position.Auto;

        /// <summary>
        /// 演者・背景の名前
        /// </summary>
        public string Name { get; private set; } = null;

        /// <summary>
        /// 時間
        /// </summary>
        public float? Duration { get; private set; } = null;

        /// <summary>
        /// 演者ID
        /// </summary>
        public int? ActorId { get; private set; } = 0;

        /// <summary>
        /// 名前を伏せる
        /// </summary>
        public bool Anonymous { get; private set; } = false;

        /// <summary>
        /// 処理が終わるまで待たない
        /// </summary>
        public bool NoWait { get; private set; } = false;

        /// <summary>
        /// BGMのデータ名
        /// </summary>
        public string BGMName { get; private set; } = string.Empty;

        /// <summary>
        /// SEのデータ名
        /// </summary>
        public string SEName { get; private set; } = string.Empty;

        /// <summary>
        /// ループ回数
        /// </summary>
        public bool IsLoop { get; private set; } = false;

        /// <summary>
        /// 選択肢の名前
        /// </summary>
        public string SelectionName { get; private set; } = null;

        /// <summary>
        /// 選択肢のジャンプ先索引
        /// </summary>
        public int CaseIndex { get; private set; } = 0;

        public CommandData(List<string> strings)
        {
            string sentence = strings.GetValueOrDefault(0);
            string command = strings.GetValueOrDefault(1);
            string arg0 = strings.GetValueOrDefault(2);
            string arg1 = strings.GetValueOrDefault(3);
            string arg2 = strings.GetValueOrDefault(4);
            string arg3 = strings.GetValueOrDefault(5);
            string arg4 = strings.GetValueOrDefault(6);
            string arg5 = strings.GetValueOrDefault(7);
            string arg6 = strings.GetValueOrDefault(8);
            string arg7 = strings.GetValueOrDefault(9);

            // 文章
            if (CommandAttribute != Attribute.Command.Delay &&
                CommandAttribute != Attribute.Command.Label &&
                CommandAttribute != Attribute.Command.Selection)
            {
                Sentence = sentence;
            }

            // コマンド
            if (!string.IsNullOrEmpty(command) && Enum.TryParse(command, out Attribute.Command result))
            {
                CommandAttribute = result;
            }

            switch (CommandAttribute)
            {
                case Attribute.Command.Delay:
                    OnCommandOfDelay(arg0);
                    break;

                case Attribute.Command.Label:
                    break;

                case Attribute.Command.Screen:
                    OnCommandOfScreen(arg0, arg1, arg2);
                    break;

                case Attribute.Command.Name:
                    OnCommandOfName(arg0);
                    break;

                case Attribute.Command.Actor:
                    OnCommandOfActor(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                    break;

                case Attribute.Command.Background:
                    OnCommandOfBackground(arg0, arg1, arg2);
                    break;

                case Attribute.Command.Selection:
                    OnCommandOfSelection(sentence, arg0, arg1);
                    break;

                case Attribute.Command.BGM:
                    OnCommandOfBGM(arg0, arg1, arg2);
                    break;

                case Attribute.Command.SE:
                    OnCommandOfSE(arg0, arg1, arg2, arg3);
                    break;
            }
        }

        private void OnCommandOfDelay(string arg0)
        {
            if (!string.IsNullOrEmpty(arg0) && float.TryParse(arg0, out float delayTime))
            {
                Duration = delayTime;
            }
        }

        private void OnCommandOfName(string arg0)
        {
            // 名前
            if (!string.IsNullOrEmpty(arg0))
            {
                Name = arg0;
            }
        }

        private void OnCommandOfActor(string arg0, string arg1, string arg2, string arg3, string arg4, string arg5, string arg6, string arg7)
        {
            // 名前
            if (!string.IsNullOrEmpty(arg0))
            {
                Name = arg0;
            }

            // 表情
            if (!string.IsNullOrEmpty(arg1) && Enum.TryParse(arg1, out Attribute.Expression expression))
            {
                Expression = expression;
            }

            // コマンド引数
            if (!string.IsNullOrEmpty(arg2) && Enum.TryParse(arg2, out Attribute.CommandOfActor command))
            {
                CommandOfActor = command;
            }

            // 位置
            if (!string.IsNullOrEmpty(arg3) && Enum.TryParse(arg3, out Attribute.Position position))
            {
                Position = position;
            }

            // 演者ID
            if (!string.IsNullOrEmpty(arg4) && int.TryParse(arg4, out int actorId))
            {
                ActorId = actorId;
            }

            // 表示方法
            if (!string.IsNullOrEmpty(arg5) && Enum.TryParse(arg5, out Attribute.Display display))
            {
                Display = display;
            }

            // 名前を伏せる
            if (!string.IsNullOrEmpty(arg6))
            {
                Anonymous = true;
            }

            // 待機しない
            if (!string.IsNullOrEmpty(arg7))
            {
                NoWait = true;
            }
        }

        private void OnCommandOfScreen(string arg0, string arg1, string arg2)
        {
            // コマンド引数
            if (!string.IsNullOrEmpty(arg0) && Enum.TryParse(arg0, out Attribute.CommandOfScreen command))
            {
                CommandOfScreen = command;
            }

            // 時間
            if (!string.IsNullOrEmpty(arg1) && float.TryParse(arg1, out float duration))
            {
                Duration = duration;
            }

            // 待機しない
            if (!string.IsNullOrEmpty(arg2))
            {
                NoWait = true;
            }
        }

        private void OnCommandOfBackground(string arg0, string arg1, string arg2)
        {
            // 名前
            if (!string.IsNullOrEmpty(arg0))
            {
                Name = arg0;
            }

            // 時間
            if (!string.IsNullOrEmpty(arg1) && float.TryParse(arg1, out float duration))
            {
                Duration = duration;
            }

            // 待機しない
            if (!string.IsNullOrEmpty(arg2))
            {
                NoWait = true;
            }
        }

        private void OnCommandOfSelection(string sentence, string arg0, string arg1)
        {
            // コマンド引数
            if (!string.IsNullOrEmpty(arg0) && Enum.TryParse(arg0, out Attribute.CommandOfSelection command))
            {
                CommandOfSelection = command;
            }

            // 選択肢のジャンプ先索引
            if (!string.IsNullOrEmpty(arg1) && int.TryParse(arg1, out int caseIndex))
            {
                CaseIndex = caseIndex;
            }

            if (CommandOfSelection == Attribute.CommandOfSelection.None)
            {
                // 選択肢名
                SelectionName = sentence;

                // 待機しない
                NoWait = true;
            }
        }

        private void OnCommandOfBGM(string arg0, string arg1, string arg2)
        {
            // コマンド引数
            if (!string.IsNullOrEmpty(arg0) && Enum.TryParse(arg0, out Attribute.CommandOfBGM command))
            {
                CommandOfBGM = command;
            }

            // BGMデータの名前
            if (!string.IsNullOrEmpty(arg1))
            {
                BGMName = arg1;
            }

            // フェード時間
            if (!string.IsNullOrEmpty(arg2) && float.TryParse(arg2, out float duration))
            {
                Duration = duration;
            }

            // 待機しない
            NoWait = true;
        }

        private void OnCommandOfSE(string arg0, string arg1, string arg2, string arg3)
        {
            // コマンド引数
            if (!string.IsNullOrEmpty(arg0) && Enum.TryParse(arg0, out Attribute.CommandOfSE command))
            {
                CommandOfSE = command;
            }

            // SEデータの名前
            if (!string.IsNullOrEmpty(arg1))
            {
                SEName = arg1;
            }

            // 時間
            if (!string.IsNullOrEmpty(arg2) && float.TryParse(arg2, out float duration))
            {
                Duration = duration;
            }

            // ループする
            if (!string.IsNullOrEmpty(arg3))
            {
                IsLoop = true;
            }

            // 待機しない
            NoWait = true;
        }

        /// <summary>
        /// 文章がある
        /// </summary>
        /// <returns></returns>
        public bool HasSentence()
        {
            return !string.IsNullOrEmpty(Sentence);
        }

        /// <summary>
        /// 演者の発言がある
        /// </summary>
        /// <returns></returns>
        public bool IsSpeach()
        {
            return HasSentence() && CommandAttribute == Attribute.Command.Actor;
        }

        /// <summary>
        /// 選択肢の分岐点である
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public bool IsSelectionCase(int index)
        {
            return CommandAttribute == Attribute.Command.Selection &&
                   CommandOfSelection == Attribute.CommandOfSelection.Case &&
                   CaseIndex == index;
        }

        /// <summary>
        /// 選択肢の終了点である
        /// </summary>
        /// <returns></returns>
        public bool IsSelectionEnd()
        {
            return CommandAttribute == Attribute.Command.Selection &&
                   CommandOfSelection == Attribute.CommandOfSelection.End;
        }
    }
}
