using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// Tild Map Editorのユーティリティクラス
    /// </summary>
    public static class TiledMapUtility
    {
        /// <summary>
        /// プロパティの値を取得する
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="jToken"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static TValue GetPropertyValue<TValue>(JToken jToken, string propertyName)
        {
            return jToken
                ["properties"]
                .FirstOrDefault(x => x.Value<string>("name") == propertyName)
                .Value<TValue>("value");
        }

        /// <summary>
        /// プロパティの値を取得する
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="jToken"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetPropertyValue<TValue>(JToken jToken, string propertyName, TValue defaultValue = default)
        {
            TValue result = defaultValue;

            JToken value = jToken
                ["properties"]?
                .FirstOrDefault(x => x["name"]?.ToString() == propertyName)?
                ["value"];

            if (value is not null)
            {
                result = value.ToObject<TValue>();
            }

            return result;
        }

        /// <summary>
        /// プロパティの値を取得する
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(JToken property)
        {
            return property["value"].ToObject<TValue>();
        }
    }
}
