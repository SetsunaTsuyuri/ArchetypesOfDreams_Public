using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// 入力関連の便利な関数集
    /// </summary>
    public static class InputUtility
    {
        public static bool Submit()
        {
            return Input.GetMouseButtonDown(0) ||
                   Input.GetKeyDown(KeyCode.Return) ||
                   Input.GetKeyDown(KeyCode.Space) ||
                   Input.GetKeyDown(KeyCode.J) ||
                   Input.GetKeyDown(KeyCode.Z) ||
                   Input.GetKey(KeyCode.Y);
        }
    }
}
