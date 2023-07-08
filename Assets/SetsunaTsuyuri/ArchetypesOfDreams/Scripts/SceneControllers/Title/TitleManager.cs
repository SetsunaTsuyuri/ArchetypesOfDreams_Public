using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// タイトルシーンの管理者
    /// </summary>
    public class TitleManager : MonoBehaviour
    {
        /// <summary>
        /// UI
        /// </summary>
        [SerializeField]
        TitleUIManager _ui = null;

        private void Start()
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            StartAsync(token).Forget();
        }

        private async UniTask StartAsync(CancellationToken token)
        {
            await FadeManager.FadeOut(0.0f, token);

            _ui.SetUp();

            AudioManager.PlayBgm(BgmId.Title);

            await FadeManager.FadeIn(token);

            _ui.TitleMenu.BeSelected();
        }
    }
}
