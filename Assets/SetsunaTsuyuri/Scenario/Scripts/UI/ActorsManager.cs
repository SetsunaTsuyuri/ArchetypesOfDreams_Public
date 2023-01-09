using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.Scenario
{
    /// <summary>
    /// 演者の管理者
    /// </summary>
    public class ActorsManager : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// 出演者たちのデータ
        /// </summary>
        [SerializeField]
        ActorDataGroup actorDataGroup = null;

        /// 演者リスト
        /// </summary>
        Actor[] actors = null;

        /// <summary>
        /// 演者の自動指定位置配列
        /// </summary>
        [SerializeField]
        Attribute.Position[] autoPositions = { Attribute.Position.Left, Attribute.Position.Right };

        /// <summary>
        /// 位置の自動指定候補となる演者配列
        /// </summary>
        Actor[] autoPositionActors = null;

        /// <summary>
        /// 演者の位置自動指定インデックス
        /// </summary>
        int autoPositionIndex = 0;

        /// <summary>
        /// 演者ID自動指定インデックス
        /// </summary>
        int autoActorIdIndex = 0;

        public void Initialize()
        {
            foreach (var actor in actors)
            {
                actor.Initialize();
            }
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="scenario">シナリオの管理者</param>
        public void SetUp(ScenarioManager scenario)
        {
            // 演者を取得する
            actors = GetComponentsInChildren<Actor>();

            // 位置の自動指定で取得する演者の候補
            autoPositionActors = new Actor[autoPositions.Length];
            for (int i = 0; i < autoPositions.Length; i++)
            {
                Actor autoTarget = actors.FirstOrDefault(a => a.Position == autoPositions[i]);
                if (autoTarget != null)
                {
                    autoPositionActors[i] = autoTarget;
                }
            }

            // スキップ処理を登録する
            ImageController[] imageControllers = GetComponentsInChildren<ImageController>();
            foreach (var controller in imageControllers)
            {
                controller.IsRequestedToSkip += scenario.IsRequestedToSkip;
            }
        }

        /// <summary>
        /// 演者を探す
        /// </summary>
        /// <param name="command">コマンドデータ</param>
        /// <returns>取得不可の場合はnull</returns>
        public Actor Find(CommandData command)
        {
            // 演者IDが指定されている場合
            if (command.ActorId.HasValue)
            {
                // 名前と演者IDの両方が一致している演者を取得する
                return actors.FirstOrDefault(a => a.HasDataAndMatch(command.Name) && a.Id == (int)command.ActorId);
            }
            else
            {
                // 名前が一致している演者を取得する
                return actors.FirstOrDefault(a => a.HasDataAndMatch(command.Name));
            }
        }

        /// <summary>
        /// 演者の名前をデータから探し、見つからなければ空文字を返す
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <returns></returns>
        public string FindNameOrEmpty(CommandData command)
        {
            string result = string.Empty;

            ActorData data = actorDataGroup.GetDataOrDefault(command.Name);
            if (data != null)
            {
                result = data.DisplayName;
            }

            return result;
        }

        /// <summary>
        /// 演者を探し、それにデータの設定を行う
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <returns></returns>
        public Actor FindAndSetData(CommandData command)
        {
            // データ
            ActorData data = actorDataGroup.GetDataOrDefault(command.Name);

            // 演者
            Actor actor;

            // 位置の指定
            if (command.Position == Attribute.Position.Auto)
            {
                actor = actors.FirstOrDefault(a => a.Position == autoPositions[autoPositionIndex]);
                autoPositionIndex = (autoPositionIndex + 1) % autoPositions.Length;
            }
            else
            {
                actor = actors.FirstOrDefault(a => a.Position == command.Position);
            }

            if (actor)
            {
                // 演者IDを設定する
                if (command.ActorId.HasValue)
                {
                    actor.Id = (int)command.ActorId;
                }
                else
                {
                    actor.Id = autoActorIdIndex;
                    autoActorIdIndex = (autoActorIdIndex + 1) % autoPositions.Length;
                }

                // データを設定する
                actor.Data = data;
            }

            return actor;
        }

        /// <summary>
        /// 話し手を強調する
        /// </summary>
        /// <param name="speaker">話し手</param>
        public void Spotlight(Actor speaker)
        {
            foreach (var actor in actors)
            {
                // 話し手を明るくする
                if (speaker == actor)
                {
                    actor.ToBrightColor();
                }
                else // 話し手以外は暗くする
                {
                    actor.ToDarkColor();
                }
            }
        }

        /// <summary>
        /// 演者をフェードインする
        /// </summary>
        /// <param name="actor">演者</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask FadeIn(Actor actor, CancellationToken token)
        {
            if (actor)
            {
                // フェードイン
                await actor.FadeIn(token);
            }
            else
            {
                Debug.LogError("actor is null.");
            }
        }

        /// <summary>
        /// 演者をフェードアウトする
        /// </summary>
        /// <param name="actor">演者</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask FadeOut(Actor actor, CancellationToken token)
        {
            if (actor)
            {
                // フェードアウト
                await actor.FadeOut(token);
            }
            else
            {
                Debug.LogError("actor is null.");
            }
        }

        /// <summary>
        /// 全ての演者をフェードアウトする
        /// </summary>
        /// <param name="duration">フェード時間</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask FadeOutAll(CancellationToken token)
        {
            Actor[] appearedActors = actors
                .Where(a => a.HasData())
                .ToArray();

            List<UniTask> tasks = new List<UniTask>();
            foreach (var actor in appearedActors)
            {
                tasks.Add(actor.FadeOut(token));
            }

            await UniTask.WhenAll(tasks);
        }
    }
}
