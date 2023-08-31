using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public class DungeonStatusesController : DictionaryController<int, bool>
    {
        public override void Initialize()
        {
            base.Initialize();

            DungeonData[] dungeons = MasterData.GetDungeons();
            foreach (var dungeon in dungeons)
            {
                Dictionary.Add(dungeon.Id, false);
            }

            // 初期開放ダンジョン
            TrySetValue(2, true);
        }
    }
}
