using System;
using Cysharp.Threading.Tasks;
using HexGame.SaveSystem;
using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class SaveManager : MonoBehaviour
    {
        private SaveSystem.SaveSystem _saveSystem;
        private LevelManager _levelManager;
        
        [Inject]
        private void Construct(SaveSystem.SaveSystem saveSystem, LevelManager levelManager)
        {
            _saveSystem = saveSystem;
            _levelManager = levelManager;
        }

        private void Awake()
        {
            LoadOrCreateSave().Forget();
        }

        public void UpdateLevelData(int levelId, bool isCompleted, int collectedFishCount, int allFishCount)
        {
            var levelDataList = _saveSystem.CurrentSave.LevelDataList;
            for (int i = 0; i < levelDataList.Count; i++)
            {
                if (levelDataList[i].LevelId == levelId)
                {
                    var oldLevelData = levelDataList[i];
                    var newLevelData = new LevelData
                    {
                        LevelId = levelId,
                        IsCompleted = oldLevelData.IsCompleted || isCompleted,
                        CollectedFishCount = Mathf.Max(oldLevelData.CollectedFishCount, collectedFishCount),
                        AllFishCount = allFishCount
                    };
                    levelDataList[i] = newLevelData;

                    break;
                }
            }
        }
        
        public async UniTask Save() => await _saveSystem.Save();

        private async UniTask LoadOrCreateSave()
        {
            await _saveSystem.Load();
            if (_saveSystem.CurrentSave == null)
                _saveSystem.CreateSave(_levelManager.LevelCount);
        }
    }
}