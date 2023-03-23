using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// アイテムの関数集
    /// </summary>
    public static class ItemUtility
    {
        /// <summary>
        /// アイテムを持っている
        /// </summary>
        /// <returns></returns>
        public static bool HasAnyItem()
        {
            bool result = VariableData.ItemsDictionary.Any(x => x.Value > 0);
            return result;
        }

        /// <summary>
        /// 指定したIDのアイテムを所有している
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <returns></returns>
        public static bool HasItem(int id)
        {
            return GetNumberOfItems(id) > 0;
        }

        /// <summary>
        /// 指定したIDのアイテムの数を取得する
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <returns></returns>
        public static int GetNumberOfItems(int id)
        {
            return VariableData.ItemsDictionary[id];
        }

        /// <summary>
        /// アイテムを獲得する
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <param name="value">獲得数</param>
        public static void ObtainItem(int id, int value = 1)
        {
            VariableData.ItemsDictionary[id] += value;
        }

        /// <summary>
        /// アイテムを消費する
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <param name="value">消費数</param>
        public static void ConsumeItem(int id, int value = 1)
        {
            VariableData.ItemsDictionary[id] -= value;
        }

        /// <summary>
        /// 所有しているアイテムを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, int>> GetOwnedItems()
        {
            return VariableData.ItemsDictionary.Where(x => x.Value > 0);
        }

        /// <summary>
        /// 所有しているアイテムIDを取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<int> GetOwnedItemIds()
        {
            return GetOwnedItems().Select(x => x.Key);
        }

        /// <summary>
        /// 所有しているアイテムを行動内容にして取得する
        /// </summary>
        /// <param name="user">使用者</param>
        /// <returns></returns>
        public static ActionInfo[] GetActionModels()
        {
            var ids = GetOwnedItemIds();

            List<ActionInfo> actions = new();
            foreach (var id in ids)
            {
                ActionInfo action = new(MasterData.GetItemData(id));
                actions.Add(action);
            }

            return actions.ToArray();
        }
    }
}
