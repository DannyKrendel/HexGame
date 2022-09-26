using System;

namespace HexGame.SaveSystem
{
    [Serializable]
    public struct LevelData
    {
        public int LevelId;
        public bool IsCompleted;
        public int CollectedFishCount;
        public int AllFishCount;
    }
}