using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexGame.Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        public int LevelCount => _levelScenePaths.Count;
        public int CurrentLevelId { get; private set; } = -1;

        private Dictionary<int, Level> _levelScenePaths;
        
        private void Awake()
        {
            InitializeLevels();
            #if UNITY_EDITOR
            var sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            foreach (var (id, level) in _levelScenePaths)
            {
                if (level.BuildIndex == sceneBuildIndex)
                {
                    CurrentLevelId = id;
                    break;
                }
            }
            #endif
        }
        
        public async UniTask LoadLevel(int level)
        {
            if (_levelScenePaths.TryGetValue(level, out var scene))
            {
                CurrentLevelId = level;
                await SceneManager.LoadSceneAsync(scene.BuildIndex);
            }
        }
        
        private void InitializeLevels()
        {
            _levelScenePaths = new Dictionary<int, Level>();
            for (int i = 0, j = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (scenePath.Contains("Level"))
                    _levelScenePaths.Add(j++, new Level { BuildIndex = i, Name = scenePath });
            }
        }

        private struct Level
        {
            public int BuildIndex;
            public string Name;
        }
    }
}