using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵UIの管理者
    /// </summary>
    public class EnemyUIManager : CombatantContainerUIManager<EnemyUI, EnemyContainersManager, EnemyContainer>
    {
        /// <summary>
        /// メインカメラ
        /// </summary>
        Camera _mainCamera = null;

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
        }

        /// <summary>
        /// 浄化されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnPurified(CombatantContainer container)
        {
            EnemyUI ui = uiArray[container.Id];
            ui.DeactivateAndHide();
        }

        /// <summary>
        /// 座標が設定されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnPositionSet(CombatantContainer container)
        {
            EnemyUI ui = uiArray[container.Id];
            ui.transform.position = RectTransformUtility.WorldToScreenPoint(
                _mainCamera,
                container.transform.position
                + (Vector3)GameSettings.Enemies.UIPositionOffset
                + (Vector3)container.Combatant.Data.EnemyUIPositionOffset);
        }
    }
}
