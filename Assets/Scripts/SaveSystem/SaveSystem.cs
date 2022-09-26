using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HexGame.Storage;
using UnityEngine;

namespace HexGame.SaveSystem
{
    public class SaveSystem
    {
        private readonly IStorage<SaveData> _storage;
        private readonly string _savePath;
        
        public SaveData CurrentSave { get; private set; }
        
        public SaveSystem(IStorage<SaveData> storage, string savePath)
        {
            _storage = storage;
            _savePath = Application.persistentDataPath + savePath;
        }

        public async UniTask Save()
        {
            await _storage.Save(_savePath, CurrentSave);
        }

        public async UniTask Load()
        {
            CurrentSave = await _storage.Load(_savePath);
        }

        public void CreateSave(int levelCount)
        {
            CurrentSave = new SaveData
            {
                LevelDataList = new List<LevelData>()
            };

            for (int i = 1; i <= levelCount; i++)
            {
                CurrentSave.LevelDataList.Add(new LevelData
                {
                    LevelId = i
                });
            }
        }
    }
}