using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者のデータ
    /// </summary>
    [System.Serializable]
    public abstract class CombatantData : NameDescriptionData
    {
        /// <summary>
        /// 感情属性
        /// </summary>
        public Attribute.Emotion Emotion = Attribute.Emotion.None;

        /// <summary>
        /// 最大HP
        /// </summary>
        public int HP = 1000;

        /// <summary>
        /// 最大DP
        /// </summary>
        public int DP = 100;

        /// <summary>
        /// 最大GP
        /// </summary>
        public int GP = 10;

        /// <summary>
        /// 力
        /// </summary>
        public int Power = 100;

        /// <summary>
        /// 技
        /// </summary>
        public int Technique = 100;

        /// <summary>
        /// 素早さ
        /// </summary>
        public int Speed = 100;

        /// <summary>
        /// 命中
        /// </summary>
        public int Accuracy = 100;

        /// <summary>
        /// 回避
        /// </summary>
        public int Evasion = 0;

        /// <summary>
        /// 習得スキル
        /// </summary>
        public SkillAcquisitionData[] Skills = { };

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
        /// 通常攻撃の属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Skill NormalAttack { get; private set; }

        /// <summary>
        /// 敵として倒したときに得られる経験値
        /// </summary>
        public int RewardExperience = 100;

        /// <summary>
        /// 顔スプライトまたはスプライトを取得する
        /// </summary>
        /// <returns></returns>
        public Sprite GetFaceSpriteOrSprite()
        {
            Sprite sprite = FaceSprite ? FaceSprite : Sprite;
            return sprite;
        }
    }
}
