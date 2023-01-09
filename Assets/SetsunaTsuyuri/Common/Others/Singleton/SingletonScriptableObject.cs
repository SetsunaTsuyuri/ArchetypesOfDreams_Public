using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// シングルトンなScriptableObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        static T s_instance = null;

        /// <summary>
        /// インスタンス
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!s_instance)
                {
                    s_instance = Resources.Load<T>(typeof(T).Name);
                }

                return s_instance;
            }
        }
    }
}
