using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// リセットの管理者
    /// </summary>
    public class ResetManager : MonoBehaviour
    {
        /// <summary>
        /// リセットする
        /// </summary>
        /// <param name="context"></param>
        public void ResetGame(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            SceneChangeManager.ChangeScene(SceneNames.Title);
        }
    }
}
