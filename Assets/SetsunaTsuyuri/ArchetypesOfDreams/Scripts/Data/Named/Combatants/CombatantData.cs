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
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// 顔スプライト
        /// </summary>
        [field: SerializeField]
        public Sprite FaceSprite { get; private set; }

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
        public BasicStatusData Status { get; private set; }

        /// <summary>
        /// 近接武器スキル
        /// </summary>
        [field: SerializeField]
        public int[] MeleeWeaponSkills { get; private set; }

        /// <summary>
        /// 遠隔武器スキル
        /// </summary>
        [field: SerializeField]
        public int[] RangedWeaponSkills { get; private set; }

        /// <summary>
        /// 特殊スキル
        /// </summary>
        [field: SerializeField]
        public int[] SpecialSkills { get; private set; }

        /// <summary>
        /// 精神
        /// </summary>
        [field: SerializeField]
        public SoulData[] Souls { get; private set; }

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
