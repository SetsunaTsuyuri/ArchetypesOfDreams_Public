using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// シーン遷移の管理者
    /// </summary>
    public class SceneChangeManager : Singleton<SceneChangeManager>, IInitializable
    {
        /// <summary>
        /// シーン切り替えの最中である
        /// </summary>
        bool isChangingScene = false;

        /// <summary>
        /// シーンを変更する
        /// </summary>
        /// <param name="name">シーンの名前</param>
        public static void ChangeScene(string name)
        {
            // シーン変更中なら中止する
            if (Instance.isChangingScene)
            {
                return;
            }

            CancellationTokenSource source = new CancellationTokenSource();
            Instance.ChangeSceneAsync(name, source.Token).Forget();
        }

        /// <summary>
        /// シーンを変更する(非同期)
        /// </summary>
        /// <param name="name">シーンの名前</param>
        /// <param name="token">トークン</param>
        /// <returns></returns>
        public async UniTask ChangeSceneAsync(string name, CancellationToken token)
        {
            // フラグON
            isChangingScene = true;

            // フェードアウト
            await FadeManager.FadeOut(token);

            // シーンロード
            SceneManager.LoadScene(name);

            // フェードイン
            await FadeManager.FadeIn(token);

            // フラグOFF
            isChangingScene = false;
        }
    }
}
