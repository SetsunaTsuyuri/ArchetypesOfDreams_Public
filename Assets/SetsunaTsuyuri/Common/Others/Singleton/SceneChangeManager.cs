using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// シーンID
    /// </summary>
    public enum SceneId
    {
        Title = 0,
        MyRoom = 1,
        DungeonSelection = 2,
        Dungeon = 3
    }

    /// <summary>
    /// シーン遷移の管理者
    /// </summary>
    public class SceneChangeManager : Singleton<SceneChangeManager>, IInitializable
    {
        /// <summary>
        /// シーン切り替えの最中である
        /// </summary>
        bool _isChangingScene = false;

        /// <summary>
        /// シーンを変更する
        /// </summary>
        /// <param name="id">シーンID</param>
        /// <param name="callback">シーン変更後に呼び出す関数</param>
        public static void StartChange(SceneId id, UnityAction<Scene, LoadSceneMode> callback = null)
        {
            // シーン変更中なら中止する
            if (Instance._isChangingScene)
            {
                return;
            }

            CancellationTokenSource source = new();
            ChangeSceneAsync(id, callback, source.Token).Forget();
        }

        /// <summary>
        /// シーンを変更する(非同期)
        /// </summary>
        /// <param name="id">シーンID</param>
        /// <param name="callback">シーン変更後に呼び出す関数</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async UniTask ChangeSceneAsync(SceneId id, UnityAction<Scene, LoadSceneMode> callback, CancellationToken token)
        {
            // フラグON
            Instance._isChangingScene = true;

            // フェードアウト
            await FadeManager.FadeOut(token);

            // 関数登録
            if (callback is not null)
            {
                SceneManager.sceneLoaded += callback;
            }

            // シーンロード
            SceneManager.LoadScene((int)id);

            // フラグOFF
            Instance._isChangingScene = false;
        }
    }
}
