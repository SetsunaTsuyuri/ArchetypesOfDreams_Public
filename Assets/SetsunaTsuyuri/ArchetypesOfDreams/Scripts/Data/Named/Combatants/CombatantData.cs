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
        public GameAttribute.Emotion Emotion = GameAttribute.Emotion.None;

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
        /// AI
        /// </summary>
        public CombatantAIElementData[] AIElements = { };

        /// <summary>
        /// スプライト名
        /// </summary>
        public string SpriteName = string.Empty;

        /// <summary>
        /// スプライトの拡大倍率
        /// </summary>
        public float SpriteScale = 1.0f;

        /// <summary>
        /// X軸味方スプライトオフセット
        /// </summary>
        public float AllySpriteOffsetX = 0.0f;

        /// <summary>
        /// Y軸味方スプライトオフセット
        /// </summary>
        public float AllySpriteOffsetY = 0.0f;

        /// <summary>
        /// 通常攻撃アニメーションID
        /// </summary>
        public int AttackAnimationId = 0;

        /// <summary>
        /// 敵UI表示位置オフセット
        /// </summary>
        [field: SerializeField]
        public Vector2 EnemyUIPositionOffset { get; private set; } = Vector2.zero;

        /// <summary>
        /// 敵として倒したときに得られる経験値
        /// </summary>
        public int RewardExperience = 100;

        /// <summary>
        /// 敵として倒したときに得られる精気
        /// </summary>
        public int RewardSpirit = 10;
    }
}
