using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 詳細ステータスUI
    /// </summary>
    public class DetailedStatusUI : GameUI
    {
        /// <summary>
        /// 名前
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _name = null;

        /// <summary>
        /// レベル
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _level = null;

        /// <summary>
        /// 現在HP
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _currentHP = null;

        /// <summary>
        /// 最大HP
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _maxHP = null;

        /// <summary>
        /// 現在DP
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _currentDP = null;

        /// <summary>
        /// 最大DP
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _maxDP = null;

        /// <summary>
        /// 現在GP
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _currentGP = null;

        /// <summary>
        /// 最大GP
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _maxGP = null;

        /// <summary>
        /// 力
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _power = null;

        /// <summary>
        /// 技
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _technique = null;

        /// <summary>
        /// 速さ
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _speed = null;

        /// <summary>
        /// 命中
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _accuracy = null;

        /// <summary>
        /// 回避
        /// </summary>
        [SerializeField]
        TextMeshProUGUI _evasion = null;

        /// <summary>
        /// 表示を更新する
        /// </summary>
        /// <param name="combatant"></param>
        public void UpdateView(Combatant combatant)
        {
            // 名前
            _name.text = combatant.Data.Name;

            // レベル
            _level.text = combatant.Level.ToString();

            // HP
            _currentHP.text = combatant.CurrentHP.ToString();
            _maxHP.text = combatant.MaxHP.ToString();
            
            // DP
            _currentDP.text = combatant.CurrentDP.ToString();
            _maxDP.text = combatant.MaxDP.ToString();

            // GP
            _currentGP.text = combatant.CurrentGP.ToString();
            _maxGP.text = combatant.MaxGP.ToString();

            // 力
            _power.text = combatant.Power.ToString();

            // 技
            _technique.text = combatant.Technique.ToString();

            // 速さ
            _speed.text = combatant.Speed.ToString();

            // 命中
            _accuracy.text = combatant.Accuracy.ToString();

            // 回避
            _evasion.text = combatant.Evasion.ToString();
        }
    }
}
