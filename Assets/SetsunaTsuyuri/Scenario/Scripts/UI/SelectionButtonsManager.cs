using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.Scenario
{
    public class SelectionButtonsManager : MonoBehaviour
    {
        /// <summary>
        /// ボタンの親
        /// </summary>
        LayoutGroup buttonsParent = null;

        /// <summary>
        /// 選択肢ボタン配列
        /// </summary>
        Button[] buttons = null;

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="scenario">シナリオの管理者</param>
        public void SetUp(ScenarioManager scenario)
        {
            buttonsParent = GetComponentInChildren<LayoutGroup>(true);

            // ボタンにイベントリスナーを登録し、それぞれを非表示にする
            buttons = buttonsParent.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < buttons.Length; i++)
            {
                int local = i;
                buttons[i].onClick.AddListener(() => scenario.OnSelectionButtonPressed(local));
                buttons[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// ボタンを1つアクティブにする
        /// </summary>
        public void ShowAButton(string name)
        {
            // 非アクティブのボタンを1つ探し出す
            Button button = buttons.FirstOrDefault(b => !b.isActiveAndEnabled);
            if (button)
            {
                // ボタンのテキストを選択肢の名前にして表示する
                TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
                text.text = name;
                button.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// ボタンを隠す
        /// </summary>
        public void Hide()
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// いずれかのボタンが押されるまで待つ
        /// </summary>
        /// <param name="tasks">UniTaskリスト</param>
        /// <param name="token"></param>
        public async UniTask WaitUntilAnyButtonPressed(CancellationToken token)
        {
            await UniTask.WhenAny(buttons
                .Where(b => b.isActiveAndEnabled)
                .Select(t => t.OnClickAsync(token))
            );
        }
    }
}
