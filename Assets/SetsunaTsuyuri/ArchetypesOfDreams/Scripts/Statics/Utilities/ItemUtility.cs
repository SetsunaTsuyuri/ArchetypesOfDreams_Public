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
        /// 使用可能なアイテムを持っている
        /// </summary>
        /// <returns></returns>
        public static bool HasAnyUsableItem()
        {
            bool result = RuntimeData.Items
                .Where(x => x > 0)
                .Any();

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
            return RuntimeData.Items[id];
        }

        /// <summary>
        /// アイテムを獲得する
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <param name="value">獲得数</param>
        public static void ObtainItem(int id, int value = 1)
        {
            RuntimeData.Items[id] += value;
        }

        /// <summary>
        /// アイテムを消費する
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <param name="value">消費数</param>
        public static void ConsumeItem(int id, int value = 1)
        {
            RuntimeData.Items[id] -= value;
        }

        /// <summary>
        /// 所有しているアイテムを行動内容にして取得する
        /// </summary>
        /// <param name="user">使用者</param>
        /// <returns></returns>
        public static ActionModel[] GetActionModels()
        {
            List<ActionModel> actions = new();
            int[] items = RuntimeData.Items;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] > 0)
                {
                    ActionModel action = new(MasterData.Items[i]);
                    actions.Add(action);
                }

            }
            return actions.ToArray();
        }
    }
}
