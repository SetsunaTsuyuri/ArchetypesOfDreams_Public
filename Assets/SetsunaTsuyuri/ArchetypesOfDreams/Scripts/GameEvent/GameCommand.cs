using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームコマンド(実行すべき内容を纏めたもの)
    /// ★削除予定 2022/09/10時点
    /// </summary>
    [Serializable]
    public class GameCommand
    {
        // ここから↓は不要

        /// <summary>
        /// コマンドの属性
        /// </summary>
        [field: SerializeField]
        public Attribute.GameCommand Command { get; private set; } = Attribute.GameCommand.None;

        /// <summary>
        /// 敵グループ及び再生するシナリオのID
        /// </summary>
        [field: SerializeField]
        public int Id { get; private set; } = -1;

        /// <summary>
        /// シナリオの属性
        /// </summary>
        [field: SerializeField]
        public Attribute.Scenario ScenarioAttribute { get; private set; } = Attribute.Scenario.MainStory;

        /// <summary>
        /// 敵グループの選び方
        /// </summary>
        [field: SerializeField]
        public Attribute.EnemyGroup EnemyGroup { get; private set; } = Attribute.EnemyGroup.Fixed;

        /// <summary>
        /// 敵のレベル補正
        /// </summary>
        [field: SerializeField]
        public int LevelCorrection { get; private set; } = 0;

        /// <summary>
        /// ボス戦闘である
        /// </summary>
        [field: SerializeField]
        public bool IsBossBattle { get; private set; } = false;

        /// <summary>
        /// プレイヤーが存在するセクション
        /// </summary>
        public int CurrentPlayerSection { get; set; } = 0;
    }
}
