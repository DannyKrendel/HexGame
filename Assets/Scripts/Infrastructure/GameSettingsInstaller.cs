using System;
using HexGame.Gameplay;
using UnityEngine;
using Zenject;

namespace HexGame.Infrastructure
{
    [CreateAssetMenu(menuName = "Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField] private GameSettings _gameSettings;

        public override void InstallBindings()
        {
            Container
                .BindInstance(_gameSettings)
                .AsSingle()
                .NonLazy();
        }
    }
    
    [Serializable]
    public struct GameSettings
    {
        public GameObject GameCameraPrefab;
        public GameObject PlayerPrefab;
    }
}