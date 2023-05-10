using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public abstract class GameEventBase : IGameEvent
    {
        /// <summary>
        /// Intにする
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected int ToInt(string[] columns, int index, int defaultValue = default)
        {
            int result = defaultValue;

            if (columns.Length > index
                && int.TryParse(columns[index], out int parsed))
            {
                result = parsed;
            }

            return result;
        }

        /// <summary>
        /// Floatにする
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="index">index</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected float ToFloat(string[] columns, int index, float defaultValue = default)
        {
            float result = defaultValue;

            if (columns.Length > index
                && float.TryParse(columns[index], out float parsed))
            {
                result = parsed;
            }

            return result;
        }

        /// <summary>
        /// Enumにする
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="columns"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected TEnum ToEnum<TEnum>(string[] columns, int index, TEnum defaultValue = default) where TEnum : struct
        {
            TEnum result = defaultValue;

            if (columns.Length > index
                && System.Enum.TryParse(columns[index], out TEnum parsed))
            {
                result = parsed;
            }

            return result;
        }

        /// <summary>
        /// boolにする
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected bool ToBool(string[] columns, int index, bool defaultValue = default)
        {
            bool result = defaultValue;

            if (columns.Length > index
                && bool.TryParse(columns[index], out bool parsed))
            {
                result = parsed;
            }

            return result;
        }

        /// <summary>
        /// Stringを取得する
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected string GetString(string[] columns, int index, string defaultValue = "")
        {
            string result = defaultValue;

            if (columns.Length > index)
            {
                result = columns[index];
            }

            return result;
        }

        public abstract UniTask Resolve(CancellationToken token);
    }
}
