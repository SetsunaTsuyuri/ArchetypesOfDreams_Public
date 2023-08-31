using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムの管理クラス
    /// </summary>
    public class ItemsController : DictionaryController<int, int>, IInitializable
    {
        public override void Initialize()
        {
            base.Initialize();

            // ディクショナリーに追加
            var itemIds = MasterData.GetItemIds();
            foreach (var itemId in itemIds)
            {
                Dictionary.Add(itemId, 0);
            }

            // 初期入手アイテム
            Increase(1, 10);
            Increase(2, 10);
            Increase(3, 10);
            Increase(4, 10);
        }

        /// <summary>
        /// 何らかのアイテムを持っている
        /// </summary>
        public bool HasAny
        {
            get => Dictionary.Any(x => x.Value > 0);
        }

        /// <summary>
        /// 全てのアイテム所持数
        /// </summary>
        public IEnumerable<KeyValuePair<int, int>> OwnedItems
        {
            get => Dictionary.Where(x => x.Value > 0);
        }

        /// <summary>
        /// アイテムID
        /// </summary>
        public IEnumerable<int> OwnedItemIds
        {
            get => OwnedItems.Select(x => x.Key);
        }

        /// <summary>
        /// 指定したIDのアイテムを所有している
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <returns></returns>
        public bool Has(int id)
        {
            return GetNumber(id) > 0;
        }

        /// <summary>
        /// 指定したIDのアイテムの所持数を取得する
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <returns></returns>
        public int GetNumber(int id)
        {
            int result = -1;

            if (Dictionary.TryGetValue(id, out int number))
            {
                result = number;
            }

            return result;
        }

        /// <summary>
        /// 増やす
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Increase(int id, int value = 1)
        {
            if (!Dictionary.ContainsKey(id))
            {
                return;
            }
            
            int number = Dictionary[id] + value;
            Dictionary[id] = Clamp(number);
        }

        /// <summary>
        /// 減らす
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Decrease(int id, int value = 1)
        {
            if (!Dictionary.ContainsKey(id))
            {
                return;
            }

            int number = Dictionary[id] - value;
            Dictionary[id] = Clamp(number);
        }

        /// <summary>
        /// クランプする
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int Clamp(int value)
        {
            int result = Mathf.Clamp(value, 0, GameSettings.Items.MaxItems);
            return result;
        }

        /// <summary>
        /// 所有しているアイテムを行動内容にして取得する
        /// </summary>
        /// <param name="user">使用者</param>
        /// <returns></returns>
        public ActionInfo[] GetActionInfos()
        {
            int [] ids = OwnedItemIds.ToArray();
            ActionInfo[] actions = new ActionInfo[ids.Length];
            
            for (int i = 0; i < actions.Length; i++)
            {
                ItemData item = MasterData.GetItemData(ids[i]);
                ActionInfo action = new(item);
                actions[i] = action;
            }

            return actions;
        }
    }
}
