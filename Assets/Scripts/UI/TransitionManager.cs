using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HexGame.Gameplay;
using PolyternityStuff.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace HexGame.UI
{
    public class TransitionManager
    {
        private readonly SceneReference _transitionScene;

        private TransitionScreen _currentTransition;
        
        public bool IsInTransition { get; private set; }

        public TransitionManager(SceneReference transitionScene, TransitionEventBus transitionEventBus)
        {
            _transitionScene = transitionScene;
            transitionEventBus.Subscribe(TransitionEventBus.EventType.BeforeAction, StartTransition);
            transitionEventBus.Subscribe(TransitionEventBus.EventType.AfterAction, EndTransition);
        }

        private async UniTask StartTransition(CancellationToken cancellationToken = default)
        {
            await SceneManager.LoadSceneAsync(_transitionScene.BuildIndex, LoadSceneMode.Additive);
            IsInTransition = true;
            _currentTransition = Object.FindObjectOfType<TransitionScreen>();
            
            if (_currentTransition == null)
                Debug.LogError($"There is no TransitionScreen in scene {_transitionScene.ScenePath}");
            else
                await _currentTransition.Show();
        }

        private async UniTask EndTransition(CancellationToken cancellationToken = default)
        {
            if (_currentTransition == null)
                Debug.LogError($"There is no TransitionScreen in scene {_transitionScene.ScenePath}");
            else
                await _currentTransition.Hide();
            
            await SceneManager.UnloadSceneAsync(_transitionScene.BuildIndex);
            IsInTransition = false;
        }
    }
}