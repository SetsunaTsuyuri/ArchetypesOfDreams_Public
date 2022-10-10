using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 解放ボタンの管理者
    /// </summary>
    public class ReleaseButtonsManager : SelectableGameUI<CombatantContainerButton>
    {
        /// <summary>
        /// 更新した後選択する
        /// </summary>
        /// <param name="allies">味方の管理者</param>
        public void UpdateButtonsAndSelect(AllyContainersManager allies)
        {
            CombatantContainer[] containers = allies.GetPurifiedEnemyAndAllMembers();

            for (int i = 0; i < containers.Length; i++)
            {
                _buttons[i].SetUp(allies, containers[i]);
            }

            UpdateButtonNavigationsToLoop();
            Select();
        }
    }
}
