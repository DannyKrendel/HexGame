using System;
using HexGame.Gameplay;
using UnityEngine;
using Zenject;

namespace HexGame.Infrastructure
{
    [CreateAssetMenu(menuName = "Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField] private GameInstallerSettings _gameInstallerSettings;
        [SerializeField] private GameSettings _gameSettings;

        public override void InstallBindings()
        {
            BindGameInstallerSettings();
            BindGameSettings();
        }

        private void BindGameInstallerSettings()
        {
            Container
                .BindInstance(_gameInstallerSettings)
                .AsSingle()
                .NonLazy();
        }

        private void BindGameSettings()
        {
            Container
                .BindInstance(_gameSettings)
                .AsSingle()
                .NonLazy();
        }
    }
}