using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 敵コンテナUIの管理UI
    /// </summary>
    public class EnemiesUI : CombatantsUI<EnemyUI, EnemiesParty, EnemyContainer>
    {
        /// <summary>
        /// メインカメラ
        /// </summary>
        Camera _mainCamera = null;

        /// <summary>
        /// キャンバスのレクトトランスフォーム
        /// </summary>
        RectTransform _canvasRect = null; 

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
            _canvasRect = _canvas.GetComponent<RectTransform>();
        }

        /// <summary>
        /// 浄化されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnPurified(CombatantContainer container)
        {
            EnemyUI ui = _uiArray[container.Id];
            ui.DeactivateAndHide();
        }

        /// <summary>
        /// 座標が設定されたときの処理
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void OnPositionSet(CombatantContainer container)
        {
            EnemyUI ui = _uiArray[container.Id];

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
                _mainCamera,
                container.transform.position
                + (Vector3)GameSettings.Enemies.UIPositionOffset
                + (Vector3)container.Combatant.Data.EnemyUIPositionOffset);

            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPoint, _mainCamera, out Vector2 localPoint))
            {
                ui.transform.localPosition = localPoint;
            }
        }
    }
}
