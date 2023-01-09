using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// スキルデータグループ
    /// </summary>
    [CreateAssetMenu(fileName = "Skills", menuName = "Data/Skills")]
    public class SkillDataGroup : DataGroup<SkillData>
    {
        /// <summary>
        /// 通常攻撃ID
        /// </summary>
        [SerializeField]
        int _normalAttackId = 0;

        /// <summary>
        /// 通常攻撃スキルデータ
        /// </summary>
        public SkillData NormalAttack => this[_normalAttackId];

        /// <summary>
        /// 防御ID
        /// </summary>
        [SerializeField]
        int _defenseId = 1;

        /// <summary>
        /// 防御スキルデータ
        /// </summary>
        public SkillData Defense => this[_defenseId];

        /// <summary>
        /// 浄化ID
        /// </summary>
        [SerializeField]
        int _purificationId = 2;

        /// <summary>
        /// 浄化スキルデータ
        /// </summary>
        public SkillData Purification => this[_purificationId];

        /// <summary>
        /// 交代ID
        /// </summary>
        [SerializeField]
        int _changeId = 3;

        /// <summary>
        /// 交代スキルデータ
        /// </summary>
        public SkillData Change => this[_changeId];
    }
}
