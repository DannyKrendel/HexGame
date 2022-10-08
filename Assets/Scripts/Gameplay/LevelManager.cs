using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using HexGame.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace HexGame.Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        public int LevelCount => _levels.Count;
        public int CurrentLevelId { get; private set; } = -1;

        private TransitionEventBus _transitionEventBus;

        private Dictionary<int, Level> _levels;

        [Inject]
        private void Construct(TransitionEventBus transitionEventBus)
        {
            _transitionEventBus = transitionEventBus;
        }
        
        private void Awake()
        {
            InitializeLevels();
            
            #if UNITY_EDITOR
            // loading a level if entered play mode from a level scene
            var sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            foreach (var (id, level) in _levels)
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
            if (_levels.TryGetValue(level, out var scene))
            {
                CurrentLevelId = level;
                await _transitionEventBus.Raise(TransitionEventBus.EventType.BeforeAction);
                await SceneManager.LoadSceneAsync(scene.BuildIndex, LoadSceneMode.Additive);
                await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                await _transitionEventBus.Raise(TransitionEventBus.EventType.AfterAction);
            }
        }
        
        private void InitializeLevels()
        {
            _levels = new Dictionary<int, Level>();
            for (int i = 0, j = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (scenePath.Contains("Level"))
                    _levels.Add(j++, new Level { BuildIndex = i, Name = scenePath });
            }
        }

        private struct Level
        {
            public int BuildIndex;
            public string Name;
        }
    }
}