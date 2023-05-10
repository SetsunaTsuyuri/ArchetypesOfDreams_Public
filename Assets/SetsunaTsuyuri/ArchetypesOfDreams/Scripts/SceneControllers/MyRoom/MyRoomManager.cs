using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 自室シーンの管理者
    /// </summary>
    public class MyRoomManager : MonoBehaviour
    {
        /// <summary>
        /// UI
        /// </summary>
        [SerializeField]
        MyRoomUIManager _ui = null;

        [SerializeField]
        SerializableKeyValuePair<int, string>[] _storyEvents = { }; 

        private void Start()
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            StartAsync(token).Forget();
        }

        private async UniTask StartAsync(CancellationToken token)
        {
            // 味方セットアップ
            AlliesParty allies = GetComponentInChildren<AlliesParty>();
            allies.SetUp(null);

            // 戦闘者配列を味方コンテナへ移す
            allies.TransferCombatantsViriableDataToContainers();

            // 味方全員を全回復する
            allies.InitializeCombatantsStatus();

            // UIをセットアップする
            _ui.SetUp(allies);

            // UIを非表示にする
            _ui.SetUIsEnabled(false);

            // オートセーブを行う
            SaveDataManager.AutoSave();

            // 実行すべき本編イベントがなければこの時点でUIを表示して
            if (string.IsNullOrEmpty(GetStoryEventName()))
            {
                // UI表示
                _ui.SetUIsEnabled(true);
                
                // BGM再生
                AudioManager.PlayBgm(BgmId.MyRoom);

                // フェードイン
                await FadeManager.FadeIn(token);
            }
            else
            {
                // フェードイン
                await FadeManager.FadeIn(token);

                // 本編イベントを実行する
                await ResolveStoryEvents(token);

                // フェードインアウト
                await FadeManager.FadeOut(token);
                await FadeManager.FadeIn(token);

                // オートセーブする
                SaveDataManager.AutoSave();

                // UI表示
                _ui.SetUIsEnabled(true);
                
                // BGM再生
                AudioManager.PlayBgm(BgmId.MyRoom);
            }

            // 自室メニューを選択する
            _ui.MyRoomMenu.BeSelected();
        }

        /// <summary>
        /// 本編イベントを解決する
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTask ResolveStoryEvents(CancellationToken token)
        {
            string storyEvent = GetStoryEventName();
            while (!string.IsNullOrEmpty(storyEvent))
            {
                await GameEventsManager.Resolve(storyEvent, token);
                storyEvent = GetStoryEventName();
            }
        }

        /// <summary>
        /// 実行すべき本編イベントの名前を取得する
        /// </summary>
        /// <returns></returns>
        private string GetStoryEventName()
        {
            string result = null;

            int progress = VariableData.Progresses.Get(ProgressId.Story);
            SerializableKeyValuePair<int, string> storyEvent = _storyEvents.FirstOrDefault(x => x.Key == progress);
            if (storyEvent is not null)
            {
                result = storyEvent.Value;
            }

            return result;
        }
    }
}
