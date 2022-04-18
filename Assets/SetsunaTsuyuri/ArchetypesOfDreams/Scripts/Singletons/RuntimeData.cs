using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲーム実行中、常に保持するデータ
    /// </summary>
    public class RuntimeData : Singleton<RuntimeData>, IInitializable
    {
        /// <summary>
        /// プレイするダンジョンのデータ
        /// </summary>
        DungeonData dungeonToPlay = null;

        /// <summary>
        /// プレイするダンジョンのデータ
        /// </summary>
        public static DungeonData DungeonToPlay
        {
            get => Instance.dungeonToPlay;
            set => Instance.dungeonToPlay = value;
        }

        public override void Initialize()
        {
            dungeonToPlay = MasterData.Dungeons.GetValue(0);
        }
    }
}
