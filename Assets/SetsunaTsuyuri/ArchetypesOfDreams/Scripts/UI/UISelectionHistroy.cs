using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// UIの選択履歴
    /// </summary>
    public class UISelectionHistroy
    {
        /// <summary>
        /// 現在選択されているUI
        /// </summary>
        SelectableGameUIBase _currentSelected = null;

        /// <summary>
        /// 選択できるUI配列
        /// </summary>
        readonly SelectableGameUIBase[] _selectables = null; 

        /// <summary>
        /// UI選択履歴
        /// </summary>
        readonly Stack<SelectableGameUIBase> _historyStack = new();

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="selectables">選択可能なUI配列</param>
        public UISelectionHistroy(SelectableGameUIBase[] selectables)
        {
            _selectables = selectables;

            foreach (var selectable in _selectables)
            {
                // 選択
                selectable.Selected += s => _currentSelected = s;

                // キャンセル
                selectable.Canceled += Back;

                // スタック
                selectable.OnStack += t => Stack(t);
            }
        }

        /// <summary>
        /// UIをスタックする
        /// </summary>
        /// <param name="type"></param>
        public void Stack(System.Type type)
        {
            if (_currentSelected != null)
            {
                _historyStack.Push(_currentSelected);
            }

            SelectableGameUIBase next = _selectables.FirstOrDefault(x => x.GetType() == type);
            if (next != null)
            {
                next.BeSelected();
            }
            else
            {
                Debug.LogError("UI is not found.");
            }
        }

        /// <summary>
        /// 前のUIに戻る
        /// </summary>
        public void Back()
        {
            if (!_historyStack.Any())
            {
                return;
            }

            SelectableGameUIBase previous = _historyStack.Pop();
            previous.BeSelected();
        }

        /// <summary>
        /// 履歴を消す
        /// </summary>
        public void Clear()
        {
            _historyStack.Clear();
        }
    }
}
