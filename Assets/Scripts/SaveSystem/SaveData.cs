using System;
using System.Collections.Generic;

namespace HexGame.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public List<LevelData> LevelDataList;
    }
}