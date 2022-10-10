using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者のデータ
    /// </summary>
    [System.Serializable]
    public abstract class CombatantData : DataWithIdAndName
    {
        /// <summary>
        /// スプライト
        /// </summary>
        [field: SerializeField]
        public Sprite Sprite { get; private set; } = null;

        /// <summary>
        /// 顔スプライト
        /// </summary>
        [field: SerializeField]
        public Sprite FaceSprite { get; private set; } = null;

        /// <summary>
        /// スプライトの拡大倍率
        /// </summary>
        [field: SerializeField]
        public float SpriteScale { get; private set; } = 1.0f;

        /// <summary>
        /// 敵UI表示位置オフセット
        /// </summary>
        [field: SerializeField]
        public Vector2 EnemyUIPositionOffset { get; private set; } = Vector2.zero;

        /// <summary>
        /// ステータス
        /// </summary>
        [field: SerializeField]
        public BasicStatusData Status { get; private set; } = null;

        /// <summary>
        /// STR依存スキル
        /// </summary>
        [field: SerializeField]
        public int[] StrengthSkills { get; private set; } = null;

        /// <summary>
        /// TEC依存スキル
        /// </summary>
        [field: SerializeField]
        public int[] TechniqueSkills { get; private set; } = null;

        /// <summary>
        /// 特殊スキル
        /// </summary>
        [field: SerializeField]
        public int[] SpecialSkills { get; private set; } = null;

        /// <summary>
        /// 精神
        /// </summary>
        [field: SerializeField]
        public SoulData[] Souls { get; private set; } = null;

        /// <summary>
        /// STR依存武器の属性
        /// </summary>
        [field: SerializeField]
        public Attribute.StrengthWeapon StrengthWeapon { get; private set; }

        /// <summary>
        /// TEC依存武器の属性
        /// </summary>
        [field: SerializeField]
        public Attribute.TechniqueWeapon TechniqueWeapon { get; private set; }

        /// <summary>
        /// 通常攻撃の属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Skill NormalAttack { get; private set; }

        /// <summary>
        /// 敵として倒したときに得られる経験値
        /// </summary>
        [field: SerializeField]
        public int RewardExperience { get; private set; } = 0;

        /// <summary>
        /// 顔スプライトまたはスプライトを取得する
        /// </summary>
        /// <returns></returns>
        public Sprite GetFaceSpriteOrSprite()
        {
            Sprite result;
            if (FaceSprite)
            {
                result = FaceSprite;
            }
            else
            {
                result = Sprite;
            }

            return result;
        }
    }
}
